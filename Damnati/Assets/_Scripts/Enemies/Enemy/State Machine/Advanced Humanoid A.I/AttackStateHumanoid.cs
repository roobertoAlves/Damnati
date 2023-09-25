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
    public override States Tick(EnemyManager enemy)
    {
        if(enemy.CombatStyle == AICombatStyle.SwordAndShield)
        {
            return ProcessSwordAndShieldCombatySyle(enemy);
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

    private States ProcessSwordAndShieldCombatySyle(EnemyManager enemy)
    {       
        RotateTowardsTargetWhilstAttacking(enemy);

        if(enemy.DistanceFromTarget > enemy.MaximumAggroRadius)
        {
            return _pursueTargetState;
        }
        
        if(_willDoComboOnNextAttack && enemy.CanDoCombo)
        {
            AttackTargetWithCombo(enemy);
        }

        if(!_hasPerformedAttack)
        {
            AttackTarget(enemy);
            RollForComboChance(enemy);
        }

        if(_willDoComboOnNextAttack && _hasPerformedAttack)
        {
            return this;
        }

        ResetStatesFlag(); 
        return _rotateTowardsTargetState;
    }
    private States ProcessArcherCombatStyle(EnemyManager enemy)
    {
        Debug.Log("Attack 1");
        RotateTowardsTargetWhilstAttacking(enemy);

        if(enemy.IsInteracting)
        {
            return this;
        }

        if(!enemy.IsHoldingArrow)
        {
            ResetStatesFlag();
            return _combatStanceState;
        }

        Debug.Log("Attack 2");

        if(enemy.CurrentTarget.IsDead)
        {
            ResetStatesFlag();
            enemy.CurrentTarget = null;
            return this;
        }

        Debug.Log("Attack 3");

        if(enemy.DistanceFromTarget > enemy.MaximumAggroRadius)
        {
            ResetStatesFlag();
            return _pursueTargetState;
        }

        Debug.Log("Attack 4");

        if(!_hasPerformedAttack)
        {
            FireAmmo(enemy);
        }

        Debug.Log("Attack 5");

        ResetStatesFlag();
        return _rotateTowardsTargetState;
    }
    private void AttackTarget(EnemyManager enemy)
    {
        _currentAttack.PerformAttackAction(enemy);
        enemy.CurrentRecoveryTime = _currentAttack.RecoveryTime;
        _hasPerformedAttack = true;
    }
    private void AttackTargetWithCombo(EnemyManager enemy)
    {
        _currentAttack.PerformAttackAction(enemy);
        _willDoComboOnNextAttack = false;
        enemy.CurrentRecoveryTime = _currentAttack.RecoveryTime;
        _currentAttack = null;
    }
    private void RotateTowardsTargetWhilstAttacking(EnemyManager enemy)
    {
        if(enemy.CanRotate && enemy.IsInteracting)
        {
            Vector3 direction = enemy.CurrentTarget.transform.position - transform.position;
            direction.y = 0;
            direction.Normalize();

            if(direction == Vector3.zero)
            {
                direction = transform.forward;
            }

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            enemy.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, enemy.RotationSpeed / Time.deltaTime);
        }
    }
    private void RollForComboChance(EnemyManager enemy)
    {
        float comboChance = Random.Range(0, 100);

        if(enemy.AllowAIToPerformCombos && comboChance <= enemy.ComboLikelyHood)
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
    private void FireAmmo(EnemyManager enemy)
    {
        if(enemy.IsHoldingArrow)
        {
            _hasPerformedAttack = true;
            enemy.CharacterInventory.CurrentItemBeingUsed = enemy.CharacterInventory.rightHandWeapon;
            enemy.CharacterInventory.rightHandWeapon.th_release_RB_Action.PerformAction(enemy);
        }
    }
    private void ResetStatesFlag()
    {
        _willDoComboOnNextAttack = false;
        _hasPerformedAttack = false;   
    }
}

