using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStanceStateHumanoid : States
{
    [SerializeField] private AttackStateHumanoid _attackState;
    [SerializeField] private IdleState _idleState;
    [SerializeField] private PursueTargetStateHumanoid _persueTargetState;

    [SerializeField] private ItemBasedAttackAction[] _enemyAttackAction;

    protected bool _rangeDestinationSet = false;
    protected float _verticalMoveValue = 0;
    protected float _horizontalMoveValue = 0;

    [Header("State Flags")]
    [Space(15)]
    private bool _willPerformBlock = false;
    private bool _willPerformParry = false;
    private bool _willPerformDodge = false;

    #region GET & SET
    public AttackStateHumanoid AttackingState { get { return _attackState; }}
    public PursueTargetStateHumanoid PersueTarget { get { return _persueTargetState; }}
    public ItemBasedAttackAction[] EnemyAtttackActions { get { return _enemyAttackAction; }}
    #endregion
    public override States Tick(EnemyManager enemy)
    {
        if(enemy.CombatStyle == AICombatStyle.SwordAndShield)
        {
            return ProcessSwordAndShieldCombatStyle(enemy);
        }
        else if(enemy.CombatStyle == AICombatStyle.Archer)
        {
            return ProcessArcherCombatStyle(enemy);
        }
        else
        {
            return this;
        }
    }

    private States ProcessSwordAndShieldCombatStyle(EnemyManager enemy)
    {
        enemy.Animator.SetFloat("Vertical", _verticalMoveValue, 0.2f, Time.deltaTime);
        enemy.Animator.SetFloat("Horizontal", _horizontalMoveValue, 0.2f, Time.deltaTime);

        if(!enemy.IsGrounded || enemy.IsInteracting)
        {
            enemy.Animator.SetFloat("Vertical", 0);
            enemy.Animator.SetFloat("Horizontal", 0);
            return this;
        }

        if(enemy.DistanceFromTarget > enemy.MaximumAggroRadius)
        {
            return _persueTargetState;
        }

        if(!_rangeDestinationSet)
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

        if(enemy.AllowAIToPerformBlock)
        {
            RollForBlockChance(enemy);
        }
        if(enemy.AllowAIToPerformDodge)
        {
            RollForDodgeChance(enemy);
        }
        if(enemy.AllowAIToPerformParry)
        {
            RollForParryChance(enemy);
        }

        if(_willPerformBlock)
        {

        }
        if(_willPerformDodge)
        {

        }
        if(_willPerformParry)
        {

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
    private States ProcessArcherCombatStyle(EnemyManager enemy)
    {
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
            ItemBasedAttackAction enemyAttackAction = _enemyAttackAction[i];

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
            ItemBasedAttackAction enemyAttackAction = _enemyAttackAction[i];

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
    private void RollForBlockChance(EnemyManager enemy)
    {
        int blockChance = Random.Range(0, 100);

        if(blockChance <= enemy.BlockLikelyHood)
        {
            _willPerformBlock = true;
        }
        else
        {
            _willPerformBlock = false;
        }
    }
    private void RollForDodgeChance(EnemyManager enemy)
    {
        int dodgeChance = Random.Range(0, 100);

        if(dodgeChance <= enemy.DodgeLikelyHood)
        {
            _willPerformDodge = true;
        }
        else
        {
            _willPerformDodge = false;
        }
    }
    private void RollForParryChance(EnemyManager enemy)
    {
        int parryChance = Random.Range(0, 100);

        if(parryChance <= enemy.ParryLikelyHood)
        {
            _willPerformParry = true;
        }
        else
        {
            _willPerformParry = false;
        }
    }
    private void ResetStatesFlag()
    {
        _willPerformBlock = false;
        _willPerformDodge = false;
        _willPerformParry = false;
    }
}
