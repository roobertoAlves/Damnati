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
    public override States Tick(AICharacterManager aiCharacterManager)
    {
        if(aiCharacterManager.CombatStyle == AICombatStyle.SwordAndShield)
        {
            //Debug.Log("Call function");
            return ProcessSwordAndShieldCombatStyle(aiCharacterManager);
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
    private States ProcessSwordAndShieldCombatStyle(AICharacterManager aiCharacterManager)
    {
        if(!aiCharacterManager.IsGrounded || aiCharacterManager.IsInteracting)
        {
            aiCharacterManager.Animator.SetFloat("Vertical", 0);
            aiCharacterManager.Animator.SetFloat("Horizontal", 0);
            return this;
        }
        
        if(aiCharacterManager.IsDead)
        {
            aiCharacterManager.Animator.SetFloat("Vertical", 0);
            aiCharacterManager.Animator.SetFloat("Horizontal", 0);
            aiCharacterManager.CurrentTarget = null;
            return _idleState;
        }

        if(aiCharacterManager.DistanceFromTarget > aiCharacterManager.MaximumAggroRadius)
        {
            return _pursueTargetState;
        }

        if(!_rangeDestinationSet)
        {
            _rangeDestinationSet = true;
            DecideCirclingAction(aiCharacterManager.AICharacterAnimatorManager);
        }

        if(aiCharacterManager.AllowAIToPerformBlock)
        {
            //Debug.Log("AI Block");
            RollForBlockChance(aiCharacterManager);
        }

        if(aiCharacterManager.AllowAIToPerformDodge)
        {
            //Debug.Log("AI Dodge");
            RollForDodgeChance(aiCharacterManager);
        }

        if(aiCharacterManager.AllowAIToPerformParry)
        {
            //Debug.Log("AI Parry");
            RollForParryChance(aiCharacterManager);
        }
        
        if(aiCharacterManager.AllowAIToPerformParry)
        {
            if(aiCharacterManager.CurrentTarget.CanBeRiposted)
            {
                CheckForRiposte(aiCharacterManager);
                return this;
            }
        }

        if(_willPerformBlock)
        {
            BlockUsingOffHand(aiCharacterManager);
        }

        if(_willPerformDodge && aiCharacterManager.CurrentTarget.IsAttacking)
        {
            Dodge(aiCharacterManager);
        }
        
        if(aiCharacterManager.CurrentTarget.IsAttacking)
        {
            if(_willPerformParry && !_hasPerformedParry)
            {
                ParryCurrentTarget(aiCharacterManager);
                return this;
            }
        }

        if(aiCharacterManager.CurrentRecoveryTime <= 0 && _attackState.CurrentAttack != null)
        {
            ResetStatesFlag();
            return _attackState;
        }
        else
        {
            GetNewAttack(aiCharacterManager);
        }

        HandleMovement(aiCharacterManager);

        return this;

    }
    private States ProcessArcherCombatStyle(AICharacterManager aiCharacterManager)
    {
        if(!aiCharacterManager.IsGrounded || aiCharacterManager.IsInteracting)
        {
            aiCharacterManager.Animator.SetFloat("Vertical", 0);
            aiCharacterManager.Animator.SetFloat("Horizontal", 0);
            return this;
        }

        if(aiCharacterManager.DistanceFromTarget > aiCharacterManager.MaximumAggroRadius)
        {
            ResetStatesFlag();
            return _pursueTargetState;
        }

        if(!_rangeDestinationSet)
        {
            _rangeDestinationSet = true;
            DecideCirclingAction(aiCharacterManager.AICharacterAnimatorManager);
        }

        if(aiCharacterManager.IsDead)
        {
            ResetStatesFlag();
            aiCharacterManager.Animator.SetFloat("Vertical", 0);
            aiCharacterManager.Animator.SetFloat("Horizontal", 0);
            aiCharacterManager.CurrentTarget = null;
            return _idleState;
        }

        if(aiCharacterManager.AllowAIToPerformDodge)
        {
            RollForDodgeChance(aiCharacterManager);
        }

        if(_willPerformDodge && aiCharacterManager.CurrentTarget.IsAttacking)
        {
            Dodge(aiCharacterManager);
        }

        HandleRotateTowardsTarget(aiCharacterManager);

        if(!_hasAmmoLoaded)
        {
            DrawArrow(aiCharacterManager);
            AimAtTargetBeforeFiring(aiCharacterManager);
        }
        else
        {

        }

        if(aiCharacterManager.CurrentRecoveryTime <= 0 && _hasAmmoLoaded)
        {
            ResetStatesFlag();
            return _attackState;
        }

        if(aiCharacterManager.IsStationaryArcher)
        {
            aiCharacterManager.Animator.SetFloat("Vertical", 0, 0.2f, Time.deltaTime);
            aiCharacterManager.Animator.SetFloat("Horizontal", 0, 0.2f, Time.deltaTime);
        }
        else
        {
            HandleMovement(aiCharacterManager);
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

        for (int i = 0; i < _enemyAttackAction.Length; i++)
        {
            ItemBasedAttackAction enemyAttackAction = _enemyAttackAction[i];

            if(aiCharacterManager.DistanceFromTarget <= enemyAttackAction.MaximumDistanceNeededToAttack 
                && aiCharacterManager.DistanceFromTarget >= enemyAttackAction.MinimumDistanceNeededToAttack)
            {
                if(aiCharacterManager.ViewableAngle <= enemyAttackAction.MaximumAttackAngle
                    && aiCharacterManager.ViewableAngle >= enemyAttackAction.MinimumAttackAngle)
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

            if(aiCharacterManager.DistanceFromTarget <= enemyAttackAction.MaximumDistanceNeededToAttack 
                && aiCharacterManager.DistanceFromTarget >= enemyAttackAction.MinimumDistanceNeededToAttack)
            {
                if(aiCharacterManager.ViewableAngle <= enemyAttackAction.MaximumAttackAngle 
                && aiCharacterManager.ViewableAngle >= enemyAttackAction.MinimumAttackAngle)
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
    private void RollForBlockChance(AICharacterManager aiCharacterManager)
    {
        int blockChance = Random.Range(0, 100);

        if(blockChance <= aiCharacterManager.BlockLikelyHood)
        {
            _willPerformBlock = true;
        }
        else
        {
            _willPerformBlock = false;
        }
    }
    private void RollForDodgeChance(AICharacterManager aiCharacterManager)
    {
        int dodgeChance = Random.Range(0, 100);

        if(dodgeChance <= aiCharacterManager.DodgeLikelyHood)
        {
            _willPerformDodge = true;
        }
        else
        {
            _willPerformDodge = false;
        }
    }
    private void RollForParryChance(AICharacterManager aiCharacterManager)
    {
        int parryChance = Random.Range(0, 100);

        if(parryChance <= aiCharacterManager.ParryLikelyHood)
        {
            _willPerformParry = true;
        }
        else
        {
            _willPerformParry = false;
        }
    }
    private void BlockUsingOffHand(AICharacterManager aiCharacterManager)
    {
        if(aiCharacterManager.IsBlocking == false)
        {
            if(aiCharacterManager.AllowAIToPerformBlock)
            {
                aiCharacterManager.IsBlocking = true;
                aiCharacterManager.CharacterInventory.CurrentItemBeingUsed = aiCharacterManager.CharacterInventory.leftHandWeapon;
                aiCharacterManager.CharacterCombat.SetBlockingAbsorptionsFromBlockingWeapon();

            }
        }
    }
    private void Dodge(AICharacterManager aiCharacterManager)
    {
        if(!_hasPerformedDodge)
        {
            if(!_hasRandomDodgeDirection)
            {
                float randomDodgeDirection;

                _hasRandomDodgeDirection = true;
                randomDodgeDirection = Random.Range(0, 360);
                _targetDodgeDirection = Quaternion.Euler(aiCharacterManager.transform.eulerAngles.x, randomDodgeDirection, aiCharacterManager.transform.eulerAngles.z);
            }

            if(aiCharacterManager.transform.rotation != _targetDodgeDirection)
            {
                Quaternion targetRotation = Quaternion.Slerp(aiCharacterManager.transform.rotation, _targetDodgeDirection, 1f);
                aiCharacterManager.transform.rotation = targetRotation;

                float targetYRotation = _targetDodgeDirection.eulerAngles.y;
                float currentYRotation = aiCharacterManager.transform.eulerAngles.y;
                float rotationDifference = Mathf.Abs(targetYRotation - currentYRotation);

                if(rotationDifference <= 5)
                {
                    _hasPerformedDodge = true;
                    aiCharacterManager.transform.rotation = _targetDodgeDirection;
                    aiCharacterManager.AICharacterAnimatorManager.PlayTargetAnimation("Roll", true);
                }
            }
        }
    }
    private void DrawArrow(AICharacterManager aiCharacterManager)
    {
        if(!aiCharacterManager.IsTwoHandingWeapon)
        {
            aiCharacterManager.IsTwoHandingWeapon = true;
            aiCharacterManager.CharacterWeaponSlot.LoadBothWeaponsOnSlots();
        }
        else
        {
            _hasAmmoLoaded = true;
            aiCharacterManager.CharacterInventory.CurrentItemBeingUsed = aiCharacterManager.CharacterInventory.rightHandWeapon;
            aiCharacterManager.CharacterInventory.rightHandWeapon.th_tap_R_Action.PerformAction(aiCharacterManager);
        }
    }
    private void AimAtTargetBeforeFiring(AICharacterManager aiCharacterManager)
    {
        float timeUntilAmmoIsShotAtTarget = Random.Range(aiCharacterManager.MinimumTimeToAimAtTarget, aiCharacterManager.MaximumTimeToAimAtTarget);
        aiCharacterManager.CurrentRecoveryTime = timeUntilAmmoIsShotAtTarget;
    }
    private void ParryCurrentTarget(AICharacterManager aiCharacterManager)
    {
        if(aiCharacterManager.CurrentTarget.CanBeParried)
        {
            if(aiCharacterManager.DistanceFromTarget <= 2)
            {
                _hasPerformedParry = true;
                aiCharacterManager.IsParrying = true;
                aiCharacterManager.AICharacterAnimatorManager.PlayTargetAnimation("Parry", true);
            }
        }
    }
    private void CheckForRiposte(AICharacterManager aiCharacterManager)
    {
        if(aiCharacterManager.IsInteracting)
        {
            aiCharacterManager.Animator.SetFloat("Horizontal", 0, 0.2f, Time.deltaTime);
            aiCharacterManager.Animator.SetFloat("Vertical", 0, 0.2f, Time.deltaTime);
            return;
        }

        if(aiCharacterManager.DistanceFromTarget >= 1.0)
        {
            HandleRotateTowardsTarget(aiCharacterManager);
            aiCharacterManager.Animator.SetFloat("Horizontal", 0, 0.2f, Time.deltaTime);
            aiCharacterManager.Animator.SetFloat("Vertical", 1, 0.2f, Time.deltaTime);
        }
        else
        {
            aiCharacterManager.IsBlocking = false;

            if(!aiCharacterManager.IsInteracting && !aiCharacterManager.CurrentTarget.IsBeingRiposted)
            {
                aiCharacterManager.EnemyRb.velocity = Vector3.zero;
                aiCharacterManager.Animator.SetFloat("Vertical", 0);

                aiCharacterManager.CharacterCombat.AttemptRiposte();
            }
        }
    }
    private void HandleMovement(AICharacterManager aiCharacterManager)
    {
        if(aiCharacterManager.DistanceFromTarget <= aiCharacterManager.StoppingDistance)
        {
            aiCharacterManager.Animator.SetFloat("Vertical", 0, 0.2f, Time.deltaTime);
            aiCharacterManager.Animator.SetFloat("Horizontal", _horizontalMoveValue, 0.2f, Time.deltaTime);
            
        }
        else
        {
            aiCharacterManager.Animator.SetFloat("Vertical", _verticalMoveValue, 0.2f, Time.deltaTime);
            aiCharacterManager.Animator.SetFloat("Horizontal", _horizontalMoveValue, 0.2f, Time.deltaTime);
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
