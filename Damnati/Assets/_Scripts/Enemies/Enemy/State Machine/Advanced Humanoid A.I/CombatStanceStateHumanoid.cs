using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStanceStateHumanoid : States
{
    private AttackStateHumanoid _attackState;
    private IdleStateHumanoid _idleState;
    private PursueTargetStateHumanoid _pursueTargetState;

    [SerializeField] private ItemBasedAttackAction[] _enemyAttackAction;

    protected bool _rangeDestinationSet = false;
    protected float _verticalMoveValue = 0;
    protected float _horizontalMoveValue = 0;

    private Quaternion _targetDodgeDirection;

    [Header("State Flags")]
    [Space(15)]
    private bool _willPerformBlock = false;
    private bool _willPerformParry = false;
    private bool _willPerformDodge = false;

    private bool _hasPerformedDodge = false;
    private bool _hasPerformedParry = false;
    private bool _hasRandomDodgeDirection = false;
    private bool _hasAmmoLoaded = false;

    #region GET & SET
    public AttackStateHumanoid AttackingState { get { return _attackState; }}
    public PursueTargetStateHumanoid PersueTarget { get { return _pursueTargetState; }}
    public ItemBasedAttackAction[] EnemyAtttackActions { get { return _enemyAttackAction; }}
    #endregion

    private void Awake()
    {
        _attackState = GetComponent<AttackStateHumanoid>();
        _idleState = GetComponent<IdleStateHumanoid>();
        _pursueTargetState = GetComponent<PursueTargetStateHumanoid>();
    }
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
            return _pursueTargetState;
        }

        if(!_rangeDestinationSet)
        {
            _rangeDestinationSet = true;
            DecideCirclingAction(enemy.EnemyAnimatorManager);
        }

        if(enemy.IsDead)
        {
            enemy.Animator.SetFloat("Vertical", 0);
            enemy.Animator.SetFloat("Horizontal", 0);
            enemy.CurrentTarget = null;
            return _idleState;
        }

        if(enemy.AllowAIToPerformParry)
        {
            if(enemy.CurrentTarget.CanBeRiposted)
            {
                CheckForRiposte(enemy);
                return this;
            }
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
            BlockUsingOffHand(enemy);
        }
        if(_willPerformDodge && enemy.CurrentTarget.IsAttacking)
        {
            Dodge(enemy);
        }
        if(enemy.CurrentTarget.IsAttacking)
        {
            if(_willPerformParry && !_hasPerformedParry)
            {
                ParryCurrentTarget(enemy);
                return this;
            }
        }
        
        if(enemy.CurrentRecoveryTime <= 0 && _attackState.CurrentAttack != null)
        {
            ResetStatesFlag();
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
            ResetStatesFlag();
            return _pursueTargetState;
        }

        if(!_rangeDestinationSet)
        {
            _rangeDestinationSet = true;
            DecideCirclingAction(enemy.EnemyAnimatorManager);
        }

        if(enemy.IsDead)
        {
            enemy.Animator.SetFloat("Vertical", 0);
            enemy.Animator.SetFloat("Horizontal", 0);
            enemy.CurrentTarget = null;
            return _idleState;
        }

        if(enemy.AllowAIToPerformDodge)
        {
            RollForDodgeChance(enemy);
        }

        if(_willPerformDodge && enemy.CurrentTarget.IsAttacking)
        {
            Dodge(enemy);
        }

        HandleRotateTowardsTarget(enemy);

        if(!_hasAmmoLoaded)
        {
            DrawArrow(enemy);
            AimAtTargetBeforeFiring(enemy);
        }
        else
        {

        }

        if(enemy.CurrentRecoveryTime <= 0 && _attackState.CurrentAttack != null)
        {
            ResetStatesFlag();
            return _attackState;
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
            ItemBasedAttackAction enemyAttackAction = _enemyAttackAction[i];

            if(enemy.DistanceFromTarget <= enemyAttackAction.MaximumDistanceNeededToAttack 
                && enemy.DistanceFromTarget >= enemyAttackAction.MinimumDistanceNeededToAttack)
            {
                if(enemy.ViewableAngle <= enemyAttackAction.MaximumAttackAngle
                     && enemy.ViewableAngle >= enemyAttackAction.MinimumAttackAngle)
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
    private void BlockUsingOffHand(EnemyManager enemy)
    {
        if(enemy.IsBlocking == false)
        {
            if(enemy.AllowAIToPerformBlock)
            {
                enemy.IsBlocking = true;
                enemy.CharacterInventory.CurrentItemBeingUsed = enemy.CharacterInventory.leftHandWeapon;
                enemy.CharacterCombat.SetBlockingAbsorptionsFromBlockingWeapon();

            }
        }
    }
    private void Dodge(EnemyManager enemy)
    {
        if(!_hasPerformedDodge)
        {
            if(!_hasRandomDodgeDirection)
            {
                float randomDodgeDirection;

                _hasRandomDodgeDirection = true;
                randomDodgeDirection = Random.Range(0, 160);
                _targetDodgeDirection = Quaternion.Euler(enemy.transform.eulerAngles.x, randomDodgeDirection, enemy.transform.eulerAngles.z);
            }

            if(enemy.transform.rotation != _targetDodgeDirection)
            {
                Quaternion targetRotation = Quaternion.Slerp(enemy.transform.rotation, _targetDodgeDirection, 1f);
                enemy.transform.rotation = targetRotation;

                float targetYRotation = _targetDodgeDirection.eulerAngles.y;
                float currentYRotation = enemy.transform.eulerAngles.y;
                float rotationDifference = Mathf.Abs(targetYRotation - currentYRotation);

                if(rotationDifference <= 5)
                {
                    _hasPerformedDodge = true;
                    enemy.transform.rotation = _targetDodgeDirection;
                    enemy.EnemyAnimatorManager.PlayTargetAnimation("Roll", true);
                }
            }
        }
    }
    private void DrawArrow(EnemyManager enemy)
    {
        if(!enemy.IsTwoHandingWeapon)
        {
            enemy.IsTwoHandingWeapon = true;
            enemy.CharacterWeaponSlot.LoadBothWeaponsOnSlots();
        }
        else
        {
            _hasAmmoLoaded = true;
            enemy.CharacterInventory.CurrentItemBeingUsed = enemy.CharacterInventory.rightHandWeapon;
            enemy.CharacterInventory.rightHandWeapon.th_tap_R_Action.PerformAction(enemy);
        }
    }
    private void AimAtTargetBeforeFiring(EnemyManager enemy)
    {
        float timeUntilAmmoIsShotAtTarget = Random.Range(enemy.MinimumTimeToAimAtTarget, enemy.MaximumTimeToAimAtTarget);
        enemy.CurrentRecoveryTime = timeUntilAmmoIsShotAtTarget;
    }
    private void ParryCurrentTarget(EnemyManager enemy)
    {
        if(enemy.CurrentTarget.CanBeParried)
        {
            if(enemy.DistanceFromTarget <= 2)
            {
                _hasPerformedParry = true;
                enemy.IsParrying = true;
                enemy.EnemyAnimatorManager.PlayTargetAnimation("Parry", true);
            }
        }
    }
    private void CheckForRiposte(EnemyManager enemy)
    {
        if(enemy.IsInteracting)
        {
            enemy.Animator.SetFloat("Hor", 0, 0.2f, Time.deltaTime);
            enemy.Animator.SetFloat("Vertical", 0, 0.2f, Time.deltaTime);
            return;
        }

        if(enemy.DistanceFromTarget >= 1.0)
        {
            HandleRotateTowardsTarget(enemy);
            enemy.Animator.SetFloat("Hor", 0, 0.2f, Time.deltaTime);
            enemy.Animator.SetFloat("Vertical", 1, 0.2f, Time.deltaTime);
        }
        else
        {
            enemy.IsBlocking = false;

            if(!enemy.IsInteracting && !enemy.CurrentTarget.IsBeingRiposted)
            {
                enemy.EnemyRb.velocity = Vector3.zero;
                enemy.Animator.SetFloat("Vertical", 0);
                
                enemy.CharacterCombat.AttemptRiposte();
            }
        }
    }
    private void ResetStatesFlag()
    {
        _hasRandomDodgeDirection = false;
        _hasPerformedDodge = false;
        _hasAmmoLoaded = false;
        _hasPerformedParry = false;
        _rangeDestinationSet = false;
        _willPerformBlock = false;
        _willPerformDodge = false;
        _willPerformParry = false;
    }
}
