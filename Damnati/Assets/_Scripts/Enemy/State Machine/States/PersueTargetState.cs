using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersueTargetState : States
{
    [SerializeField] private CombatStanceState _combatStanceState;
    public override States Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorController enemyAnimatorController)
    {
        if(enemyManager.IsInteracting)
        {
            return this;
        }

        if(enemyManager.IsPerfomingAction)
        {
            enemyAnimatorController.Anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
            return this;
        }
        Vector3 targetDirection = enemyManager.CurrentTarget.transform.position -  enemyManager.transform.position;
        float distanceFromTarget = Vector3.Distance(enemyManager.CurrentTarget.transform.position, enemyManager.transform.position);
        float viewableAngle = Vector3.Angle(targetDirection,  enemyManager.transform.forward);
        
        if(distanceFromTarget > enemyManager.MaximumAttackRange)
        {
            enemyAnimatorController.Anim.SetFloat("Vertical", 5, 0.1f, Time.deltaTime);
        }

        HandleRotateTowardsTarget(enemyManager);
        
        if(distanceFromTarget <= enemyManager.MaximumAttackRange)
        {
            return _combatStanceState;
        }
        else
        {
            return this;
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
}
