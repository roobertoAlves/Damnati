using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "A.I/Humanoid Actions/Item Based Attack Action")]
public class ItemBasedAttackAction : ScriptableObject
{
    [Header("Attack Type")]
    [Space(15)]
    public AIAttackActionType actionAttackType = AIAttackActionType.MeleeAttackAction;
    public AttackType attackType = AttackType.LightAttack;

    [Header("Action Combo Settings")]
    [Space(15)]
    [SerializeField] private bool _actionCanCombo = false;
    
    [Header("Right Hand Or Left Hand Action")]
    [Space(15)]
    private bool _isRightHandedAction = true;

    [Header("Action Settings")]
    [Space(15)]
    [SerializeField] private int _attackScore = 3;
    [SerializeField] private float _recoveryTime = 2;

    [SerializeField] private float _maximumAttackAngle = 35;
    [SerializeField] private float _minimumAttackAngle = -35;

    [SerializeField] private float _minimumDistanceNeededToAttack = 0;
    [SerializeField] private float _maximumDistanceNeededToAttack = 3;

    #region GET & SET
    public bool ActionCanCombo { get { return _actionCanCombo; } set { _actionCanCombo = value; }}
    
    public bool IsRightHandedAction { get { return _isRightHandedAction; } set { _isRightHandedAction = value; }}
    
    public int AttackScore { get { return _attackScore; } set { _attackScore = value; }}
    public float RecoveryTime { get { return _recoveryTime; } set { _recoveryTime = value; }}
    
    public float MaximumAttackAngle { get { return _maximumAttackAngle; } set { _maximumAttackAngle = value; }}
    public float MinimumAttackAngle { get { return _minimumAttackAngle; } set { _minimumAttackAngle = value; }} 
    
    public float MinimumDistanceNeededToAttack { get { return _minimumDistanceNeededToAttack; } set { _minimumDistanceNeededToAttack = value; }}
    public float MaximumDistanceNeededToAttack { get { return _maximumDistanceNeededToAttack; } set { _maximumDistanceNeededToAttack = value; }}
    
    #endregion

    public void PerformAttackAction(AICharacterManager aiCharacterManager)
    {
        if(_isRightHandedAction)
        {
            aiCharacterManager.UpdateWhichHandCharacterIsUsing(true);
            PerformRightHandItemActionBasedOnAttackType(aiCharacterManager);
        }
        else
        {
            aiCharacterManager.UpdateWhichHandCharacterIsUsing(false); 
            PerformLeftHandItemActionBasedOnAttackType(aiCharacterManager);
        }
    }

    private void PerformRightHandItemActionBasedOnAttackType(AICharacterManager aiCharacterManager)
    {
        if(actionAttackType == AIAttackActionType.MeleeAttackAction)
        {
        }
        else if(actionAttackType == AIAttackActionType.RangedAttackAction)
        {

        }
    }
    private void PerformLeftHandItemActionBasedOnAttackType(AICharacterManager aiCharacterManager)
    {
        if(actionAttackType == AIAttackActionType.MeleeAttackAction)
        {
        }
        else if(actionAttackType == AIAttackActionType.RangedAttackAction)
        {

        }
    }

    #region  Right Hand Actions
    private void PerformRightHandMeleeAction(AICharacterManager aiCharacterManager)
    {
        if(aiCharacterManager.IsTwoHandingWeapon)
        {
            if(attackType == AttackType.LightAttack)
            {
                aiCharacterManager.CharacterInventory.rightHandWeapon.th_tap_LB_Action.PerformAction(aiCharacterManager);
            }
            else if(attackType == AttackType.HeavyAttack)
            {
                aiCharacterManager.CharacterInventory.rightHandWeapon.th_tap_RB_Action.PerformAction(aiCharacterManager);
            }
        }
        else
        {
            if(attackType == AttackType.LightAttack)
            {
                aiCharacterManager.CharacterInventory.rightHandWeapon.oh_tap_LB_Action.PerformAction(aiCharacterManager);
            }
            else if(attackType == AttackType.HeavyAttack)
            {
                aiCharacterManager.CharacterInventory.rightHandWeapon.oh_tap_RB_Action.PerformAction(aiCharacterManager);
            }
        }
    }
    #endregion

    #region  Left Hand Actions

    #endregion

}
