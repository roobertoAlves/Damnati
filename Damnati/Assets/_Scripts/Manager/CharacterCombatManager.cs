using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCombatManager : MonoBehaviour
{
    private CharacterManager _character;

    [Header("Attack Type")]
    [Space(15)]
    [SerializeField] private AttackType _currentAttackType;

    #region GET & SET
    public AttackType CurrentAttackType { get { return _currentAttackType; } set { _currentAttackType = value; }}

    #endregion

    protected virtual void Awake()
    {
        _character = GetComponent<CharacterManager>();
    }

    public virtual void SetBlockingAbsorptionsFromBlockingWeapon()
    {
        if(_character.IsUsingRightHand)
        {
            _character.CharacterStats.BlockingFireDamageAbsorption = _character.CharacterInventory.rightHandWeapon.physicalBlockingDamageAbsorption;
            _character.CharacterStats.BlockingFireDamageAbsorption = _character.CharacterInventory.rightHandWeapon.fireBlockingDamageAbsorption;
            _character.CharacterStats.BlockingStabilityRating = _character.CharacterInventory.rightHandWeapon.stability;
        }
        else if(_character.IsUsingLeftHand)
        {
            _character.CharacterStats.BlockingFireDamageAbsorption = _character.CharacterInventory.leftHandWeapon.physicalBlockingDamageAbsorption;
            _character.CharacterStats.BlockingFireDamageAbsorption = _character.CharacterInventory.leftHandWeapon.fireBlockingDamageAbsorption;
            _character.CharacterStats.BlockingStabilityRating = _character.CharacterInventory.leftHandWeapon.stability;
        }
    }
    public virtual void DrainStaminaBaseOnAttack()
    {

    }
    public virtual void DrainRageBaseOnAttack()
    {
        
    }
    public virtual void AttemptBlock(DamageCollider attackingWeapon, float physicalDamage, float fireDamage, string blockingAnimation)
    {
        float staminaDamageAbsorption = ((physicalDamage + fireDamage) * attackingWeapon.GuardBreakModifier) 
            * (_character.CharacterStats.BlockingStabilityRating / 100);

        float staminaDamage = ((physicalDamage + fireDamage) * attackingWeapon.GuardBreakModifier) - staminaDamageAbsorption;

        _character.CharacterStats.CurrentStamina = _character.CharacterStats.CurrentStamina - staminaDamage;

        if(_character.CharacterStats.CurrentStamina <= 0)
        {
            _character.IsBlocking = false;
            _character.CharacterAnimator.PlayTargetAnimation("Guard Break 01", true);
        }
        else
        {
            _character.CharacterAnimator.PlayTargetAnimation(blockingAnimation, true);
        }
    }
}
