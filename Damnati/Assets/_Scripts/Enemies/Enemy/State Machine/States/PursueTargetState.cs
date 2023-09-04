using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PursueTargetState : States
{
    [SerializeField] private CombatStanceState _combatStanceState;
    [SerializeField] private RotateTowardsTargetState _rotateTowardsTargetState;
    public override States Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorController enemyAnimatorController)
    {
        Vector3 targetDirection = enemyManager.CurrentTarget.transform.position - enemyManager.transform.position;
        float distanceFromTarget = Vector3.Distance(enemyManager.CurrentTarget.transform.position, enemyManager.transform.position);
        float viewableAngle = Vector3.SignedAngle(targetDirection, enemyManager.transform.forward, Vector3.up);

        HandleRotateTowardsTarget(enemyManager);

        if(enemyManager.IsInteracting)
        {
            return this;
        }

        if(enemyManager.IsPerfomingAction)
        {
            enemyAnimatorController.Anim.SetFloat("Vertical", 0 , 0.1f, Time.deltaTime);
            return this;
        }
        if(distanceFromTarget > enemyManager.MaximumAggroRadius)
        {
            enemyAnimatorController.Anim.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
        }

        if(distanceFromTarget <= enemyManager.MaximumAggroRadius)
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
            Vector3 direction = enemyManager.CurrentTarget.transform.position - transform.position;
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
