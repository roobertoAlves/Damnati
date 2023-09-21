using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PursueTargetState : States
{
    [SerializeField] private CombatStanceState _combatStanceState;
    [SerializeField] private RotateTowardsTargetState _rotateTowardsTargetState;
    public override States Tick(EnemyManager enemy)
    {
        HandleRotateTowardsTarget(enemy);

        if(enemy.IsInteracting)
        {
            return this;
        }

        if(enemy.IsPerfomingAction)
        {
            enemy.Animator.SetFloat("Vertical", 0 , 0.1f, Time.deltaTime);
            return this;
        }
        if(enemy.DistanceFromTarget > enemy.MaximumAggroRadius)
        {
            enemy.Animator.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
        }

        if(enemy.DistanceFromTarget <= enemy.MaximumAggroRadius)
        {
            return _combatStanceState;
        }
        else
        {
            return this;
        }
    }

    private void HandleRotateTowardsTarget(EnemyManager enemy)
    {
        //Rotate manually
        if(enemy.IsPerfomingAction)
        {
            Vector3 direction = enemy.CurrentTarget.transform.position - transform.position;
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
}
