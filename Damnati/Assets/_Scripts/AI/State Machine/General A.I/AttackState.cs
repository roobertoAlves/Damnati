using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : States
{
    [Header("A.I Scripts Components")]
    [Space(15)]
    [SerializeField] private CombatStanceState _combatStanceState;
    [SerializeField] private PursueTargetState _pursueTargetState;
    [SerializeField] private RotateTowardsTargetState _rotateTowardsTargetState;
    [SerializeField] private AICharacterAttackAction _currentAttack;

    private bool _willDoComboOnNextAttack = false;
    [SerializeField] private bool _hasPerformedAttack = false;


    #region GET & SET
    public AICharacterAttackAction CurrentAttack { get { return _currentAttack; } set { _currentAttack = value; }}
    public bool HasPerformedAttack { get { return _hasPerformedAttack; } set { _hasPerformedAttack = value; }}
    #endregion
    public override States Tick(AICharacterManager aICharacterManager)
    {
       float distanceFromTarget = Vector3.Distance(aICharacterManager.CurrentTarget.transform.position, aICharacterManager.transform.position);

        RotateTowardsTargetWhilstAttacking(aICharacterManager);

        if(distanceFromTarget > aICharacterManager.MaximumAggroRadius)
        {
            return _pursueTargetState;
        }
        
        if(_willDoComboOnNextAttack && aICharacterManager.CanDoCombo)
        {
            AttackTargetWithCombo(aICharacterManager);
        }

        if(!_hasPerformedAttack)
        {
            AttackTarget(aICharacterManager);
            RollForComboChance(aICharacterManager);
        }

        if(_willDoComboOnNextAttack && _hasPerformedAttack)
        {
            return this;
        }

        return _rotateTowardsTargetState;
    }

    private void AttackTarget(AICharacterManager aICharacterManager)
    {
        aICharacterManager.IsUsingRightHand = _currentAttack.IsRightHandedAction;
        aICharacterManager.IsUsingLeftHand = !_currentAttack.IsRightHandedAction;
        aICharacterManager.AICharacterAnimatorManager.PlayTargetAnimation(_currentAttack.ActionAnimation, true);
        aICharacterManager.AICharacterAnimatorManager.PlayWeaponTrailFX();    
        aICharacterManager.CurrentRecoveryTime = _currentAttack.RecoveryTime;
        _hasPerformedAttack = true;
    }
    private void AttackTargetWithCombo(AICharacterManager aICharacterManager)
    {
        aICharacterManager.IsUsingRightHand = _currentAttack.IsRightHandedAction;
        aICharacterManager.IsUsingLeftHand = !_currentAttack.IsRightHandedAction;
        _willDoComboOnNextAttack = false;
        aICharacterManager.AICharacterAnimatorManager.PlayTargetAnimation(_currentAttack.ActionAnimation, true);
        aICharacterManager.AICharacterAnimatorManager.PlayWeaponTrailFX();
        aICharacterManager.CurrentRecoveryTime = _currentAttack.RecoveryTime;
        _currentAttack = null;
    }
    private void RotateTowardsTargetWhilstAttacking(AICharacterManager aICharacterManager)
    {
        if(aICharacterManager.CanRotate && aICharacterManager.IsInteracting)
        {
            Vector3 direction = aICharacterManager.CurrentTarget.transform.position - transform.position;
            direction.y = 0;
            direction.Normalize();

            if(direction == Vector3.zero)
            {
                direction = transform.forward;
            }

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            aICharacterManager.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, aICharacterManager.RotationSpeed / Time.deltaTime);
        }
    }
    private void RollForComboChance(AICharacterManager aICharacterManager)
    {
        float comboChance = Random.Range(0, 100);

        if(aICharacterManager.AllowAIToPerformCombos && comboChance <= aICharacterManager.ComboLikelyHood)
        {
            if(_currentAttack.ComboAction != null)
            {
                _willDoComboOnNextAttack = true;
                _currentAttack = _currentAttack.ComboAction;
            }
            else
            {
                _willDoComboOnNextAttack = false;
                _currentAttack = null;
            }
        }
    }
}
