using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PursueTargetState : States
{
    [SerializeField] private CombatStanceState _combatStanceState;

    public override States Tick(AICharacterManager aiCharacter)
    {
        HandleRotateTowardsTarget(aiCharacter);

        if(aiCharacter.IsInteracting)
        {
            return this;
        }

        if(aiCharacter.IsPerfomingAction)
        {
            aiCharacter.Animator.SetFloat("Vertical", 0 , 0.1f, Time.deltaTime);
            return this;
        }
        if(aiCharacter.DistanceFromTarget > aiCharacter.MaximumAggroRadius)
        {
            aiCharacter.EnemyNavMeshAgent.destination = aiCharacter.CurrentTarget.transform.position;
        }

        if(aiCharacter.DistanceFromTarget <= aiCharacter.MaximumAggroRadius)
        {
            return _combatStanceState;
        }
        else
        {
            return this;
        }
    }

    private void HandleRotateTowardsTarget(AICharacterManager aiCharacter)
    {
        //Rotate manually
        if(aiCharacter.IsPerfomingAction)
        {
            Vector3 direction = aiCharacter.CurrentTarget.transform.position - transform.position;
            direction.y = 0;
            direction.Normalize();

            if(direction == Vector3.zero)
            {
                direction = transform.forward;
            }

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            aiCharacter.transform.rotation = Quaternion.Slerp(aiCharacter.transform.rotation, targetRotation, aiCharacter.RotationSpeed / Time.deltaTime);
        }

        //Rotate with pathfinding (navmesh)
        else
        {
            Vector3 relativeDirection = transform.InverseTransformDirection(aiCharacter.EnemyNavMeshAgent.desiredVelocity);
            Vector3 targetVelocity = aiCharacter.EnemyRb.velocity;

            aiCharacter.EnemyNavMeshAgent.enabled = true;
            aiCharacter.EnemyNavMeshAgent.SetDestination(aiCharacter.CurrentTarget.transform.position);
            aiCharacter.EnemyRb.velocity = targetVelocity;
            aiCharacter.transform.rotation = Quaternion.Slerp(aiCharacter.transform.rotation, aiCharacter.EnemyNavMeshAgent.transform.rotation, aiCharacter.RotationSpeed / Time.deltaTime);
        }
    }
}
