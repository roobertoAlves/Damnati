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

    protected bool _rangeDestinationSet = false;
    protected float _verticalMoveValue = 0;
    protected float _horizontalMoveValue = 0;

    #region GET & SET
    public AttackState AttackingState { get { return _attackState; }}
    public PursueTargetState PersueTarget { get { return _persueTargetState; }}
    public EnemyAttackAction[] EnemyAtttackActions { get { return _enemyAttackAction; }}
    #endregion
    public override States Tick(EnemyManager enemy)
    {
        enemy.Animator.SetFloat("Vertical", _verticalMoveValue, 0.2f, Time.deltaTime);
        enemy.Animator.SetFloat("Horizontal", _horizontalMoveValue, 0.2f, Time.deltaTime);
        _attackState.HasPerformedAttack = false;

        if(enemy.IsInteracting)
        {
            enemy.Animator.SetFloat("Vertical", 0);
            enemy.Animator.SetFloat("Horizontal", 0);
            return this;
        }
        if(enemy.DistanceFromTarget > enemy.MaximumAggroRadius)
        {
            return _persueTargetState;
        }

        if(_rangeDestinationSet)
        {
            _rangeDestinationSet = true;
            DecideCirclingAction(enemy.EnemyAnimatorManager);
        }

        HandleRotateTowardsTarget(enemy);

        if(enemy.IsDead)
        {
            enemy.Animator.SetFloat("Vertical", 0);
            enemy.Animator.SetFloat("Horizontal", 0);
            enemy.CurrentTarget = null;
            return _idleState;
        }
        
        if(enemy.CurrentRecoveryTime <= 0 && _attackState.CurrentAttack != null)
        {
            _rangeDestinationSet = false;
            return _attackState;
        }
    
        else
        {
            GetNewAttack(enemy);
        }

        return this;
    }

    protected void HandleRotateTowardsTarget(EnemyManager enemy)
    {
        //Rotate manually
        if(enemy.IsPerfomingAction)
        {
            Vector3 direction = enemy.CurrentTarget.transform.position - enemy.transform.position;
            direction.y = 0;
            direction.Normalize();

            if(direction == Vector3.zero)
            {
                direction = transform.forward;
            }

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, targetRotation, enemy.RotationSpeed / Time.deltaTime);
        }

        //Rotate with pathfinding (navmesh)
        else
        {
            Vector3 relativeDirection = transform.InverseTransformDirection(enemy.EnemyNavMeshAgent.desiredVelocity);
            Vector3 targetVelocity = enemy.EnemyRb.velocity;

            enemy.EnemyNavMeshAgent.enabled = true;
            enemy.EnemyNavMeshAgent.SetDestination(enemy.CurrentTarget.transform.position);
            enemy.EnemyRb.velocity = targetVelocity;
            enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, enemy.EnemyNavMeshAgent.transform.rotation, enemy.RotationSpeed / Time.deltaTime);
        }
    }
    protected void DecideCirclingAction(EnemyAnimatorManager enemyAnimatorManager)
    {
        WalkAroundTarget(enemyAnimatorManager);
    }
    protected void WalkAroundTarget(EnemyAnimatorManager enemyAnimatorManager)
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
    protected virtual void GetNewAttack(EnemyManager enemy)
    {
        int maxScore = 0;

        for (int i = 0; i < _enemyAttackAction.Length; i++)
        {
            EnemyAttackAction enemyAttackAction = _enemyAttackAction[i];

            if(enemy.DistanceFromTarget <= enemyAttackAction.MaximumDistanceNeededToAttack && enemy.DistanceFromTarget >= enemyAttackAction.MinimumDistanceNeededToAttack)
            {
                if(enemy.ViewableAngle <= enemyAttackAction.MaximumAttackAngle && enemy.ViewableAngle >= enemyAttackAction.MinimumAttackAngle)
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

            if(enemy.DistanceFromTarget <= enemyAttackAction.MaximumDistanceNeededToAttack 
                && enemy.DistanceFromTarget >= enemyAttackAction.MinimumDistanceNeededToAttack)
            {
                if(enemy.ViewableAngle <= enemyAttackAction.MaximumAttackAngle 
                && enemy.ViewableAngle >= enemyAttackAction.MinimumAttackAngle)
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
