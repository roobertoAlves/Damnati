using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : States
{
    [Header("A.I Scripts Components")]
    [Space(15)]
    [SerializeField] private CombatStanceState _combatStanceState;

    [Header("A.I Attack")]
    [Space(15)]
    [SerializeField] private EnemyAttackAction[] _enemyAttackAction;
    [SerializeField] private EnemyAttackAction _currentAttack;

    private bool _willDoComboOnNextAttack = false;
    public override States Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorController enemyAnimatorController)
    {
        if(enemyManager.IsInteracting && enemyManager.CanDoCombo == false)
        {
            return this; 
        }
        else if(enemyManager.IsInteracting && enemyManager.CanDoCombo)
        {
            if(_willDoComboOnNextAttack)
            {
                _willDoComboOnNextAttack = false;
                enemyAnimatorController.PlayTargetAnimation(_currentAttack.ActionAnimation, true);
            }
        }

        Vector3 targetDirection = enemyManager.CurrentTarget.transform.position -  enemyManager.transform.position;
        float distanceFromTarget = Vector3.Distance(enemyManager.CurrentTarget.transform.position, enemyManager.transform.position);
        float viewableAngle = Vector3.Angle(targetDirection,  enemyManager.transform.forward);

        HandleRotateTowardsTarget(enemyManager);

        if(enemyManager.IsPerfomingAction)
        {
            return _combatStanceState;
        }

        if(_currentAttack != null)
        {
            if(distanceFromTarget < _currentAttack.MinimumDistanceNeededToAttack)
            {
                return this;
            }
        
            else if(distanceFromTarget < _currentAttack.MaximumDistanceNeededToAttack)
            {
                if(viewableAngle <= _currentAttack.MaximumAttackAngle && viewableAngle >= _currentAttack.MinimumAttackAngle)
                {
                    if(enemyManager.CurrentRecoveryTime <= 0 && enemyManager.IsPerfomingAction == false)
                    {
                        enemyAnimatorController.Anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                        enemyAnimatorController.Anim.SetFloat("Horizontal", 0, 0.1f, Time.deltaTime);
                        enemyAnimatorController.PlayTargetAnimation(_currentAttack.ActionAnimation, true);
                        enemyManager.IsPerfomingAction = true;
                        RollForComboChance(enemyManager);

                        if(_currentAttack.CanCombo && _willDoComboOnNextAttack)
                        {
                            _currentAttack = _currentAttack.ComboAction;
                            return this;
                        }
                        else
                        {
                            enemyManager.CurrentRecoveryTime = _currentAttack.RecoveryTime;
                            _currentAttack = null;
                            return _combatStanceState;
                        }
                    }
                }
            }
        }
        else
        {
            GetNewAttack(enemyManager);
        }

        return _combatStanceState;
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
                   if(_currentAttack != null)
                   {
                        return;
                   }

                   temporaryScore += enemyAttackAction.AttackScore;

                   if(temporaryScore > randomValue)
                   {
                        _currentAttack = enemyAttackAction;
                   }
                }
            } 
        }
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
    private void RollForComboChance(EnemyManager enemyManager)
    {
        float comboChance = Random.Range(0, 100);

        if(enemyManager.AllowAIToPerformCombos && comboChance <= enemyManager.ComboLikelyHood)
        {
            _willDoComboOnNextAttack = true;
        }
    }
}
