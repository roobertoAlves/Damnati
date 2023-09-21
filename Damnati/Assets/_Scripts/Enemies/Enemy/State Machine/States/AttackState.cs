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
    [SerializeField] private EnemyAttackAction _currentAttack;

    private bool _willDoComboOnNextAttack = false;
    [SerializeField] private bool _hasPerformedAttack = false;


    #region GET & SET
    public EnemyAttackAction CurrentAttack { get { return _currentAttack; } set { _currentAttack = value; }}
    public bool HasPerformedAttack { get { return _hasPerformedAttack; } set { _hasPerformedAttack = value; }}
    #endregion
    public override States Tick(EnemyManager enemy)
    {
       float distanceFromTarget = Vector3.Distance(enemy.CurrentTarget.transform.position, enemy.transform.position);

        RotateTowardsTargetWhilstAttacking(enemy);

        if(distanceFromTarget > enemy.MaximumAggroRadius)
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

        return _rotateTowardsTargetState;
    }

    private void AttackTarget(EnemyManager enemy)
    {
        enemy.IsUsingRightHand = _currentAttack.IsRightHandedAction;
        enemy.IsUsingLeftHand = !_currentAttack.IsRightHandedAction;
        enemy.EnemyAnimatorManager.PlayTargetAnimation(_currentAttack.ActionAnimation, true);
        enemy.EnemyAnimatorManager.PlayWeaponTrailFX();    
        enemy.CurrentRecoveryTime = _currentAttack.RecoveryTime;
        _hasPerformedAttack = true;
    }
    private void AttackTargetWithCombo(EnemyManager enemy)
    {
        enemy.IsUsingRightHand = _currentAttack.IsRightHandedAction;
        enemy.IsUsingLeftHand = !_currentAttack.IsRightHandedAction;
        _willDoComboOnNextAttack = false;
        enemy.EnemyAnimatorManager.PlayTargetAnimation(_currentAttack.ActionAnimation, true);
        enemy.EnemyAnimatorManager.PlayWeaponTrailFX();
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
