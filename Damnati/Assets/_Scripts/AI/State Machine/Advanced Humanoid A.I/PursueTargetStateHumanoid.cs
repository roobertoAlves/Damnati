using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PursueTargetStateHumanoid : States
{
    private CombatStanceStateHumanoid _combatStanceState;
    private void Awake() 
    {
        _combatStanceState = GetComponent<CombatStanceStateHumanoid>();
    }
    public override States Tick(AICharacterManager aiCharacterManager)
    {
        if(aiCharacterManager.CombatStyle == AICombatStyle.SwordAndShield)
        {
            return ProcessSwordAndShiledCombatStyle(aiCharacterManager);
        }
        else if(aiCharacterManager.CombatStyle == AICombatStyle.Archer)
        {
            return ProcessArcherCombatStyle(aiCharacterManager);
        }
        else
        {
            return this;
        }
    }
    private States ProcessSwordAndShiledCombatStyle(AICharacterManager aiCharacterManager)
    {
        HandleRotateTowardsTarget(aiCharacterManager);

        if(aiCharacterManager.IsInteracting)
        {
            return this;
        }

        if(aiCharacterManager.IsPerfomingAction)
        {
            aiCharacterManager.Animator.SetFloat("Vertical", 0 , 0.1f, Time.deltaTime);
            return this;
        }
        if(aiCharacterManager.DistanceFromTarget > aiCharacterManager.MaximumAggroRadius)
        {
            aiCharacterManager.Animator.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
        }

        if(aiCharacterManager.DistanceFromTarget <= aiCharacterManager.MaximumAggroRadius)
        {
            return _combatStanceState;
        }
        else
        {
            return this;
        }
    }
    private States ProcessArcherCombatStyle(AICharacterManager aiCharacterManager)
    {
        HandleRotateTowardsTarget(aiCharacterManager);

        if(aiCharacterManager.IsInteracting)
        {
            return this;
        }

        if(aiCharacterManager.IsPerfomingAction)
        {
            aiCharacterManager.Animator.SetFloat("Vertical", 0 , 0.1f, Time.deltaTime);
            return this;
        }
        if(aiCharacterManager.DistanceFromTarget > aiCharacterManager.MaximumAggroRadius)
        {
            if(!aiCharacterManager.IsStationaryArcher)
            {
                aiCharacterManager.Animator.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
            }
            else
            {

            }
        }

        if(aiCharacterManager.DistanceFromTarget <= aiCharacterManager.MaximumAggroRadius)
        {
            return _combatStanceState;
        }
        else
        {
            return this;
        }
    }
    private void HandleRotateTowardsTarget(AICharacterManager aiCharacterManager)
    {
        //Rotate manually
        if(aiCharacterManager.IsPerfomingAction)
        {
            Vector3 direction = aiCharacterManager.CurrentTarget.transform.position - transform.position;
            direction.y = 0;
            direction.Normalize();

            if(direction == Vector3.zero)
            {
                direction = transform.forward;
            }

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            aiCharacterManager.transform.rotation = Quaternion.Slerp(aiCharacterManager.transform.rotation, targetRotation, aiCharacterManager.RotationSpeed / Time.deltaTime);
        }

        //Rotate with pathfinding (navmesh)
        else
        {
            Vector3 relativeDirection = transform.InverseTransformDirection(aiCharacterManager.EnemyNavMeshAgent.desiredVelocity);
            Vector3 targetVelocity = aiCharacterManager.EnemyRb.velocity;

            aiCharacterManager.EnemyNavMeshAgent.enabled = true;
            aiCharacterManager.EnemyNavMeshAgent.SetDestination(aiCharacterManager.CurrentTarget.transform.position);
            aiCharacterManager.EnemyRb.velocity = targetVelocity;
            aiCharacterManager.transform.rotation = Quaternion.Slerp(aiCharacterManager.transform.rotation, aiCharacterManager.EnemyNavMeshAgent.transform.rotation, aiCharacterManager.RotationSpeed / Time.deltaTime);
        }
    }
}
