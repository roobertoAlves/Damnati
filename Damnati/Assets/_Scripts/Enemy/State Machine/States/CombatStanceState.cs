using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CombatStanceState : States
{
    [SerializeField] private AttackState _attackState;
    [SerializeField] private IdleState _idleState;
    [SerializeField] private PursueTargetState _persueTargetState;

    [SerializeField] private EnemyAttackAction[] _enemyAttackAction;

    private bool _rangeDestinationSet = false;
    private float _verticalMoveValue = 0;
    private float _horizontalMoveValue = 0;
    public override States Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorController enemyAnimatorController)
    {
        float distanceFromTarget = Vector3.Distance(enemyManager.CurrentTarget.transform.position, enemyManager.transform.position);
        enemyAnimatorController.Anim.SetFloat("Vertical", _verticalMoveValue, 0.2f, Time.deltaTime);
        enemyAnimatorController.Anim.SetFloat("Horizontal", _horizontalMoveValue, 0.2f, Time.deltaTime);
        _attackState.HasPerformedAttack = false;

        if(enemyManager.IsInteracting)
        {
            enemyAnimatorController.Anim.SetFloat("Vertical", 0);
            enemyAnimatorController.Anim.SetFloat("Horizontal", 0);
            return this;
        }
        if(distanceFromTarget > enemyManager.MaximumAggroRadius)
        {
            return _persueTargetState;
        }

        if(_rangeDestinationSet)
        {
            _rangeDestinationSet = true;
            DecideCirclingAction(enemyAnimatorController);
        }

        HandleRotateTowardsTarget(enemyManager);

        if(enemyManager.CurrentTarget.IsDead)
        {
            enemyAnimatorController.Anim.SetFloat("Vertical", 0);
            enemyAnimatorController.Anim.SetFloat("Horizontal", 0);
            enemyManager.CurrentTarget = null;
            return _idleState;
        }
        
        if(enemyManager.CurrentRecoveryTime <= 0 && _attackState.CurrentAttack != null)
        {
            _rangeDestinationSet = false;
            return _attackState;
        }
    
        else
        {
            GetNewAttack(enemyManager);
        }

        return this;
    }

    private void HandleRotateTowardsTarget(EnemyManager enemyManager)
    {
        //Rotate manually
        if(enemyManager.IsPerfomingAction)
        {
            Vector3 direction = enemyManager.CurrentTarget.transform.position - enemyManager.transform.position;
            direction.y = 0;
            direction.Normalize();

            if(direction == Vector3.zero)
            {
                direction = transform.forward;
            }

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, targetRotation, enemyManager.RotationSpeed / Time.deltaTime);
        }

        //Rotate with pathfinding (navmesh)
        else
        {
            Vector3 relativeDirection = transform.InverseTransformDirection(enemyManager.EnemyNavMeshAgent.desiredVelocity);
            Vector3 targetVelocity = enemyManager.EnemyRb.velocity;

            enemyManager.EnemyNavMeshAgent.enabled = true;
            enemyManager.EnemyNavMeshAgent.SetDestination(enemyManager.CurrentTarget.transform.position);
            enemyManager.EnemyRb.velocity = targetVelocity;
            enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, enemyManager.EnemyNavMeshAgent.transform.rotation, enemyManager.RotationSpeed / Time.deltaTime);
        }
    }
    private void DecideCirclingAction(EnemyAnimatorController enemyAnimatorController)
    {
        WalkAroundTarget(enemyAnimatorController);
    }
    private void WalkAroundTarget(EnemyAnimatorController enemyAnimatorController)
    {
        _verticalMoveValue = 0.5f;
        _horizontalMoveValue = Random.Range(-1, 1);

        if(_horizontalMoveValue <= 1 && _horizontalMoveValue >= 0)
        {
            _horizontalMoveValue = 0.5f;
        }
        else if(_horizontalMoveValue >= -1 && _horizontalMoveValue < 0)
        {
            _horizontalMoveValue = -0.5f;
        }
    }
    private void GetNewAttack(EnemyManager enemyManager)
    {
        Vector3 targetsDirection = enemyManager.CurrentTarget.transform.position -  enemyManager.transform.position;
        float viewableAngle = Vector3.Angle(targetsDirection,  enemyManager.transform.forward);
        float distanceFromTarget = Vector3.Distance(enemyManager.CurrentTarget.transform.position, enemyManager.transform.position);

        int maxScore = 0;

        for (int i = 0; i < _enemyAttackAction.Length; i++)
        {
            EnemyAttackAction enemyAttackAction = _enemyAttackAction[i];

            if(distanceFromTarget <= enemyAttackAction.MaximumDistanceNeededToAttack && distanceFromTarget >= enemyAttackAction.MinimumDistanceNeededToAttack)
            {
                if(viewableAngle <= enemyAttackAction.MaximumAttackAngle && viewableAngle >= enemyAttackAction.MinimumAttackAngle)
                {
                    maxScore += enemyAttackAction.AttackScore;
                }
            }
        }

        int randomValue = Random.Range(0, maxScore);
        int temporaryScore = 0;

        for (int i = 0; i < _enemyAttackAction.Length; i++)
        {
            EnemyAttackAction enemyAttackAction = _enemyAttackAction[i];

            if(distanceFromTarget <= enemyAttackAction.MaximumDistanceNeededToAttack && distanceFromTarget >= enemyAttackAction.MinimumDistanceNeededToAttack)
            {
                if(viewableAngle <= enemyAttackAction.MaximumAttackAngle && viewableAngle >= enemyAttackAction.MinimumAttackAngle)
                {
                   if(_attackState.CurrentAttack != null)
                   {
                        return;
                   }

                   temporaryScore += enemyAttackAction.AttackScore;

                   if(temporaryScore > randomValue)
                   {
                        _attackState.CurrentAttack  = enemyAttackAction;
                   }
                }
            } 
        }
    }   
}
