using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CombatStanceState : States
{
    [SerializeField] private AttackState _attackState;
    [SerializeField] private IdleState _idleState;
    [SerializeField] private PursueTargetState _persueTargetState;

    [SerializeField] private AICharacterAttackAction[] _aiCharacterAttackAction;

    protected bool _rangeDestinationSet = false;
    protected float _verticalMoveValue = 0;
    protected float _horizontalMoveValue = 0;

    #region GET & SET
    public AttackState AttackingState { get { return _attackState; }}
    public PursueTargetState PersueTarget { get { return _persueTargetState; }}
    public AICharacterAttackAction[] EnemyAtttackActions { get { return _aiCharacterAttackAction; }}
    #endregion
    public override States Tick(AICharacterManager aiCharacterManager)
    {
        aiCharacterManager.Animator.SetFloat("Vertical", _verticalMoveValue, 0.2f, Time.deltaTime);
        aiCharacterManager.Animator.SetFloat("Horizontal", _horizontalMoveValue, 0.2f, Time.deltaTime);
        _attackState.HasPerformedAttack = false;

        if(aiCharacterManager.IsInteracting)
        {
            aiCharacterManager.Animator.SetFloat("Vertical", 0);
            aiCharacterManager.Animator.SetFloat("Horizontal", 0);
            return this;
        }
        if(aiCharacterManager.DistanceFromTarget > aiCharacterManager.MaximumAggroRadius)
        {
            return _persueTargetState;
        }

        if(!_rangeDestinationSet)
        {
            _rangeDestinationSet = true;
            DecideCirclingAction(aiCharacterManager.AICharacterAnimatorManager);
        }

        HandleRotateTowardsTarget(aiCharacterManager);

        if(aiCharacterManager.IsDead)
        {
            aiCharacterManager.Animator.SetFloat("Vertical", 0);
            aiCharacterManager.Animator.SetFloat("Horizontal", 0);
            aiCharacterManager.CurrentTarget = null;
            return _idleState;
        }
        
        if(aiCharacterManager.CurrentRecoveryTime <= 0 && _attackState.CurrentAttack != null)
        {
            _rangeDestinationSet = false;
            return _attackState;
        }
    
        else
        {
            GetNewAttack(aiCharacterManager);
        }

        return this;
    }

    protected void HandleRotateTowardsTarget(AICharacterManager aiCharacterManager)
    {
        //Rotate manually
        if(aiCharacterManager.IsPerfomingAction)
        {
            Vector3 direction = aiCharacterManager.CurrentTarget.transform.position - aiCharacterManager.transform.position;
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
    protected void DecideCirclingAction(AICharacterAnimatorManager aICharacterAnimatorManager)
    {
        WalkAroundTarget(aICharacterAnimatorManager);
    }
    protected void WalkAroundTarget(AICharacterAnimatorManager aICharacterAnimatorManager)
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
    protected virtual void GetNewAttack(AICharacterManager aiCharacterManager)
    {
        int maxScore = 0;

        for (int i = 0; i < _aiCharacterAttackAction.Length; i++)
        {
            AICharacterAttackAction aiCharacterAttackAction  = _aiCharacterAttackAction[i];

            if(aiCharacterManager.DistanceFromTarget <= aiCharacterAttackAction.MaximumDistanceNeededToAttack && aiCharacterManager.DistanceFromTarget >= aiCharacterAttackAction.MinimumDistanceNeededToAttack)
            {
                if(aiCharacterManager.ViewableAngle <= aiCharacterAttackAction.MaximumAttackAngle && aiCharacterManager.ViewableAngle >= aiCharacterAttackAction.MinimumAttackAngle)
                {
                    maxScore += aiCharacterAttackAction.AttackScore;
                }
            }
        }

        int randomValue = Random.Range(0, maxScore);
        int temporaryScore = 0;

        for (int i = 0; i < _aiCharacterAttackAction.Length; i++)
        {
           AICharacterAttackAction aiCharacterAttackAction  = _aiCharacterAttackAction[i];

            if(aiCharacterManager.DistanceFromTarget <= aiCharacterAttackAction.MaximumDistanceNeededToAttack 
                && aiCharacterManager.DistanceFromTarget >= aiCharacterAttackAction.MinimumDistanceNeededToAttack)
            {
                if(aiCharacterManager.ViewableAngle <= aiCharacterAttackAction.MaximumAttackAngle 
                && aiCharacterManager.ViewableAngle >= aiCharacterAttackAction.MinimumAttackAngle)
                {
                   if(_attackState.CurrentAttack != null)
                   {
                        return;
                   }

                   temporaryScore += aiCharacterAttackAction .AttackScore;

                   if(temporaryScore > randomValue)
                   {
                        _attackState.CurrentAttack  = aiCharacterAttackAction ;
                   }
                }
            } 
        }
    }   
}
