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
    public override States Tick(EnemyManager enemyManager, EnemyStatsManager enemyStatsManager, EnemyAnimatorManager enemyAnimatorManager)
    {
       float distanceFromTarget = Vector3.Distance(enemyManager.CurrentTarget.transform.position, enemyManager.transform.position);

        RotateTowardsTargetWhilstAttacking(enemyManager);

        if(distanceFromTarget > enemyManager.MaximumAggroRadius)
        {
            return _pursueTargetState;
        }
        
        if(_willDoComboOnNextAttack && enemyManager.CanDoCombo)
        {
            AttackTargetWithCombo(enemyAnimatorManager, enemyManager);
        }

        if(!_hasPerformedAttack)
        {
            AttackTarget(enemyAnimatorManager, enemyManager);
            RollForComboChance(enemyManager);
        }

        if(_willDoComboOnNextAttack && _hasPerformedAttack)
        {
            return this;
        }

        return _rotateTowardsTargetState;
    }

    private void AttackTarget(EnemyAnimatorManager enemyAnimatorManager, EnemyManager enemyManager)
    {
        enemyAnimatorManager.Anim.SetBool("IsUsingRightHand", CurrentAttack.IsRightHandedAction);
        enemyAnimatorManager.Anim.SetBool("IsUsingLeftHand", !CurrentAttack.IsRightHandedAction);
        enemyAnimatorManager.PlayTargetAnimation(_currentAttack.ActionAnimation, true);
        enemyAnimatorManager.PlayWeaponTrailFX();    
        enemyManager.CurrentRecoveryTime = _currentAttack.RecoveryTime;
        _hasPerformedAttack = true;
    }
    private void AttackTargetWithCombo(EnemyAnimatorManager enemyAnimatorManager, EnemyManager enemyManager)
    {
        enemyAnimatorManager.Anim.SetBool("IsUsingRightHand", CurrentAttack.IsRightHandedAction);
        enemyAnimatorManager.Anim.SetBool("IsUsingLeftHand", !CurrentAttack.IsRightHandedAction);
        _willDoComboOnNextAttack = false;
        enemyAnimatorManager.PlayTargetAnimation(_currentAttack.ActionAnimation, true);
        enemyAnimatorManager.PlayWeaponTrailFX();
        enemyManager.CurrentRecoveryTime = _currentAttack.RecoveryTime;
        _currentAttack = null;
    }
    private void RotateTowardsTargetWhilstAttacking(EnemyManager enemyManager)
    {
        if(enemyManager.CanRotate && enemyManager.IsInteracting)
        {
            Vector3 direction = enemyManager.CurrentTarget.transform.position - transform.position;
            direction.y = 0;
            direction.Normalize();

            if(direction == Vector3.zero)
            {
                direction = transform.forward;
            }

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            enemyManager.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, enemyManager.RotationSpeed / Time.deltaTime);
        }
    }
    private void RollForComboChance(EnemyManager enemyManager)
    {
        float comboChance = Random.Range(0, 100);

        if(enemyManager.AllowAIToPerformCombos && comboChance <= enemyManager.ComboLikelyHood)
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
