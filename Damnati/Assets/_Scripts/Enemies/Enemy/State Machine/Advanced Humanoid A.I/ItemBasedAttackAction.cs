using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBasedAttackAction : MonoBehaviour
{
    [Header("Attack Type")]
    [Space(15)]
    [SerializeField] private AIAttackActionType _actionAttackType = AIAttackActionType.MeleeAttackAction;
    [SerializeField] private AttackType _attackType = AttackType.LightAttack;

    [Header("Action Combo Settings")]
    [Space(15)]
    [SerializeField] private bool _actionCanCombo = false;
    
    [Header("Right Hand Or Left Hand Action")]
    [Space(15)]
    [SerializeField] private bool _isRightHandedAction = true;

    [Header("Action Settings")]
    [Space(15)]
    [SerializeField] private int _attackScore = 3;
    [SerializeField] private float _recoveryTime = 2;

    [SerializeField] private float _maximumAttackAngle = 35;
    [SerializeField] private float _minimumAttackAngle = -35;

    [SerializeField] private float _minimumDistanceNeededToAttack = 0;
    [SerializeField] private float _maximumDistanceNeededToAttack = 3;

    public void PerformAttackAction(EnemyManager enemy)
    {
        if(_isRightHandedAction)
        {
            enemy.UpdateWhichHandCharacterIsUsing(true);
            PerformRightHandItemActionBasedOnAttackType(enemy);
        }
        else
        {
            enemy.UpdateWhichHandCharacterIsUsing(false); 
            PerformLeftHandItemActionBasedOnAttackType(enemy);
        }
    }

    private void PerformRightHandItemActionBasedOnAttackType(EnemyManager enemy)
    {
        if(_actionAttackType == AIAttackActionType.MeleeAttackAction)
        {

        }
        else if(_actionAttackType == AIAttackActionType.RangedAttackAction)
        {

        }
    }
    private void PerformLeftHandItemActionBasedOnAttackType(EnemyManager enemy)
    {
        if(_actionAttackType == AIAttackActionType.MeleeAttackAction)
        {

        }
        else if(_actionAttackType == AIAttackActionType.RangedAttackAction)
        {

        }
    }

    #region  Right Hand Actions
    private void PerformRightHandMeleeAction(EnemyManager enemy)
    {
        if(enemy.IsTwoHandingWeapon)
        {
            if(_attackType == AttackType.LightAttack)
            {
                enemy.CharacterInventory.rightHandWeapon.th_tap_LB_Action.PerformAction(enemy);
            }
            else if(_attackType == AttackType.HeavyAttack)
            {
                enemy.CharacterInventory.rightHandWeapon.th_tap_RB_Action.PerformAction(enemy);
            }
        }
        else
        {
            if(_attackType == AttackType.LightAttack)
            {
                enemy.CharacterInventory.rightHandWeapon.oh_tap_LB_Action.PerformAction(enemy);
            }
            else if(_attackType == AttackType.HeavyAttack)
            {
                enemy.CharacterInventory.rightHandWeapon.oh_tap_RB_Action.PerformAction(enemy);
            }
        }
    }
    #endregion

    #region  Left Hand Actions

    #endregion

    #region GET & SET

    public AIAttackActionType ActionAttackType { get { return _actionAttackType; } set { _actionAttackType = value; }}
    public AttackType AttackType { get { return _attackType; } set { _attackType = value; }}

    public bool ActionCanCombo { get { return _actionCanCombo; } set { _actionCanCombo = value; }}
    
    public bool IsRightHandedAction { get { return _isRightHandedAction; } set { _isRightHandedAction = value; }}
    
    public int AttackScore { get { return _attackScore; } set { _attackScore = value; }}
    public float RecoveryTime { get { return _recoveryTime; } set { _recoveryTime = value; }}
    
    public float MaximumAttackAngle { get { return _maximumAttackAngle; } set { _maximumAttackAngle = value; }}
    public float MinimumAttackAngle { get { return _minimumAttackAngle; } set { _minimumAttackAngle = value; }} 
    
    public float MinimumDistanceNeededToAttack { get { return _minimumDistanceNeededToAttack; } set { _minimumDistanceNeededToAttack = value; }}
    public float MaximumDistanceNeededToAttack { get { return _maximumDistanceNeededToAttack; } set { _maximumDistanceNeededToAttack = value; }}
    
    #endregion
}
