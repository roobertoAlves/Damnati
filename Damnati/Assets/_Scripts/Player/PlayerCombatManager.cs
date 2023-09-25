using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerCombatManager : CharacterCombatManager
{
    private PlayerManager _player;
   
    protected override void Awake() 
    {
        base.Awake();
        _player = GetComponent<PlayerManager>();
    }
    
    public override void AttemptBlock(DamageCollider attackingWeapon, float physicalDamage, float fireDamage, string blockingAnimation)
    {
        base.AttemptBlock(attackingWeapon, physicalDamage, fireDamage, blockingAnimation);
        _player.PlayerStats.StaminaBar.SetCurrentStamina(Mathf.RoundToInt(_player.PlayerStats.CurrentStamina));

    }
    public override void DrainStaminaBaseOnAttack()
    {
        if(_player.IsUsingRightHand)
        {
            if(CurrentAttackType == AttackType.LightAttack)
            {
                _player.PlayerStats.DeductStamina(_player.PlayerInventory.rightHandWeapon.baseStaminaCost * _player.PlayerInventory.rightHandWeapon.lightAttackStaminaMultiplier);
            }
            else if(CurrentAttackType == AttackType.HeavyAttack)
            {
                _player.PlayerStats.DeductStamina(_player.PlayerInventory.rightHandWeapon.baseStaminaCost * _player.PlayerInventory.rightHandWeapon.heavyAttackStaminaMultiplier);
            }
            else if(CurrentAttackType == AttackType.RunningLightAttack)
            {
                _player.PlayerStats.DeductStamina(_player.PlayerInventory.rightHandWeapon.baseStaminaCost * _player.PlayerInventory.rightHandWeapon.runningAttackStaminaMultiplier);
            }
            else if(CurrentAttackType == AttackType.JumpingHeavyAttack)
            {
                _player.PlayerStats.DeductStamina(_player.PlayerInventory.rightHandWeapon.baseStaminaCost * _player.PlayerInventory.rightHandWeapon.jumpingAttackStaminaMultiplier);
            }
        }
        else if(_player.IsUsingLeftHand)
        {
            if(CurrentAttackType == AttackType.LightAttack)
            {
                _player.PlayerStats.DeductStamina(_player.PlayerInventory.leftHandWeapon.baseStaminaCost * _player.PlayerInventory.leftHandWeapon.lightAttackStaminaMultiplier);
            }
            else if(CurrentAttackType == AttackType.HeavyAttack)
            {
                _player.PlayerStats.DeductStamina(_player.PlayerInventory.leftHandWeapon.baseStaminaCost * _player.PlayerInventory.leftHandWeapon.heavyAttackStaminaMultiplier);
            }
            else if(CurrentAttackType == AttackType.RunningLightAttack)
            {
                _player.PlayerStats.DeductStamina(_player.PlayerInventory.leftHandWeapon.baseStaminaCost * _player.PlayerInventory.leftHandWeapon.runningAttackStaminaMultiplier);
            }
            else if(CurrentAttackType == AttackType.JumpingHeavyAttack)
            {
                _player.PlayerStats.DeductStamina(_player.PlayerInventory.leftHandWeapon.baseStaminaCost * _player.PlayerInventory.leftHandWeapon.jumpingAttackStaminaMultiplier);
            }
        }
    }
    public override void DrainRageBaseOnAttack()
    {
        if(_player.IsUsingRightHand)
        {
            if(CurrentAttackType == AttackType.LightAttack)
            {
                //_player.PlayerStats
            }
            else if(CurrentAttackType == AttackType.HeavyAttack)
            {

            }
            else if(CurrentAttackType == AttackType.RunningLightAttack)
            {

            }
            else if(CurrentAttackType == AttackType.JumpingHeavyAttack)
            {

            }
        }
        else if(_player.IsUsingLeftHand)
        {
            if(CurrentAttackType == AttackType.LightAttack)
            {

            }
            else if(CurrentAttackType == AttackType.HeavyAttack)
            {

            }
            else if(CurrentAttackType == AttackType.RunningLightAttack)
            {

            }
            else if(CurrentAttackType == AttackType.JumpingHeavyAttack)
            {
                
            }
        }
    }
}
