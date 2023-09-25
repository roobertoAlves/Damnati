using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackStateHumanoid : States
{
    [Header("A.I Scripts Components")]
    [Space(15)]
    private CombatStanceStateHumanoid _combatStanceState;
    private PursueTargetStateHumanoid _pursueTargetState;
    private RotateTowardsTargetStateHumanoid _rotateTowardsTargetState;
    private ItemBasedAttackAction _currentAttack;

    private bool _willDoComboOnNextAttack = false;
    [SerializeField] private bool _hasPerformedAttack = false;


    #region GET & SET
    public ItemBasedAttackAction CurrentAttack { get { return _currentAttack; } set { _currentAttack = value; }}
    public bool HasPerformedAttack { get { return _hasPerformedAttack; } set { _hasPerformedAttack = value; }}
    #endregion
    private void Awake()
    {
        _combatStanceState = GetComponent<CombatStanceStateHumanoid>();
        _rotateTowardsTargetState = GetComponent<RotateTowardsTargetStateHumanoid>();
        _pursueTargetState = GetComponent<PursueTargetStateHumanoid>();
    }
    public override States Tick(AICharacterManager aiCharacterManager)
    {
        if(aiCharacterManager.CombatStyle == AICombatStyle.SwordAndShield)
        {
            return ProcessSwordAndShieldCombatySyle(aiCharacterManager);
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

    private States ProcessSwordAndShieldCombatySyle(AICharacterManager aiCharacterManager)
    {       
        RotateTowardsTargetWhilstAttacking(aiCharacterManager);

        if(aiCharacterManager.DistanceFromTarget > aiCharacterManager.MaximumAggroRadius)
        {
            return _pursueTargetState;
        }
        
        if(_willDoComboOnNextAttack && aiCharacterManager.CanDoCombo)
        {
            AttackTargetWithCombo(aiCharacterManager);
        }

        if(!_hasPerformedAttack)
        {
            AttackTarget(aiCharacterManager);
            RollForComboChance(aiCharacterManager);
        }

        if(_willDoComboOnNextAttack && _hasPerformedAttack)
        {
            return this;
        }

        ResetStatesFlag(); 
        return _rotateTowardsTargetState;
    }
    private States ProcessArcherCombatStyle(AICharacterManager aiCharacterManager)
    {
        Debug.Log("Attack 1");
        RotateTowardsTargetWhilstAttacking(aiCharacterManager);

        if(aiCharacterManager.IsInteracting)
        {
            return this;
        }

        if(!aiCharacterManager.IsHoldingArrow)
        {
            ResetStatesFlag();
            return _combatStanceState;
        }

        Debug.Log("Attack 2");

        if(aiCharacterManager.CurrentTarget.IsDead)
        {
            ResetStatesFlag();
            aiCharacterManager.CurrentTarget = null;
            return this;
        }

        Debug.Log("Attack 3");

        if(aiCharacterManager.DistanceFromTarget > aiCharacterManager.MaximumAggroRadius)
        {
            ResetStatesFlag();
            return _pursueTargetState;
        }

        Debug.Log("Attack 4");

        if(!_hasPerformedAttack)
        {
            FireAmmo(aiCharacterManager);
        }

        Debug.Log("Attack 5");

        ResetStatesFlag();
        return _rotateTowardsTargetState;
    }
    private void AttackTarget(AICharacterManager aiCharacterManager)
    {
        _currentAttack.PerformAttackAction(aiCharacterManager);
        aiCharacterManager.CurrentRecoveryTime = _currentAttack.RecoveryTime;
        _hasPerformedAttack = true;
    }
    private void AttackTargetWithCombo(AICharacterManager aiCharacterManager)
    {
        _currentAttack.PerformAttackAction(aiCharacterManager);
        _willDoComboOnNextAttack = false;
        aiCharacterManager.CurrentRecoveryTime = _currentAttack.RecoveryTime;
        _currentAttack = null;
    }
    private void RotateTowardsTargetWhilstAttacking(AICharacterManager aiCharacterManager)
    {
        if(aiCharacterManager.CanRotate && aiCharacterManager.IsInteracting)
        {
            Vector3 direction = aiCharacterManager.CurrentTarget.transform.position - transform.position;
            direction.y = 0;
            direction.Normalize();

            if(direction == Vector3.zero)
            {
                direction = transform.forward;
            }

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            aiCharacterManager.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, aiCharacterManager.RotationSpeed / Time.deltaTime);
        }
    }
    private void RollForComboChance(AICharacterManager aiCharacterManager)
    {
        float comboChance = Random.Range(0, 100);

        if(aiCharacterManager.AllowAIToPerformCombos && comboChance <= aiCharacterManager.ComboLikelyHood)
        {
            if(_currentAttack.ActionCanCombo)
            {
                _willDoComboOnNextAttack = true;
            }
            else
            {
                _willDoComboOnNextAttack = false;
                _currentAttack = null;
            }
        }
    }
    private void FireAmmo(AICharacterManager aiCharacterManager)
    {
        if(aiCharacterManager.IsHoldingArrow)
        {
            _hasPerformedAttack = true;
            aiCharacterManager.CharacterInventory.CurrentItemBeingUsed = aiCharacterManager.CharacterInventory.rightHandWeapon;
            aiCharacterManager.CharacterInventory.rightHandWeapon.th_release_RB_Action.PerformAction(aiCharacterManager);
        }
    }
    private void ResetStatesFlag()
    {
        _willDoComboOnNextAttack = false;
        _hasPerformedAttack = false;   
    }
}

