using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerCombatManager : CharacterCombatManager
{
    private PlayerManager _player;


    private LayerMask _riposteLayer = 1 << 9;
   
    protected override void Awake() 
    {
        base.Awake();
        _player = GetComponent<PlayerManager>();
    }
    
    public override void AttemptRiposte()
    {
        base.AttemptRiposte();
        RaycastHit hit;

        if(Physics.Raycast(_player.CriticalAttackRayCastStartPoint.position, 
        transform.TransformDirection(Vector3.forward), out hit, 0.7f, _riposteLayer))
        {
            //Debug.Log("Step 1");
            CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
            //Debug.Log("Enemy Character Manager: " + enemyCharacterManager.transform.name);
            DamageCollider rightWeapon = _player.PlayerWeaponSlot.RightHandDamageCollider;
            //Debug.Log("Right Weapon Collider: " +  rightWeapon.transform.name);
           
            if(enemyCharacterManager != null && enemyCharacterManager.CanBeRiposted)
            {
                //Debug.Log("Step 2");
                _player.transform.position = enemyCharacterManager.RiposteDamageCollider.CriticalDamagerStandPosition.position;

                Vector3 rotationDirection = _player.transform.eulerAngles;
                rotationDirection = hit.transform.position - _player.transform.position;
                rotationDirection.y = 0;
                rotationDirection.Normalize();
                Quaternion tr = Quaternion.LookRotation(rotationDirection);
                Quaternion targetRotation = Quaternion.Slerp(_player.transform.rotation, tr, 500 * Time.deltaTime);
                _player.transform.rotation = targetRotation;

                int criticalDamage = _player.PlayerInventory.rightHandWeapon.criticalDamageMultiplier * rightWeapon.PhysicalDamage;
                enemyCharacterManager.PendingCriticalDamage = criticalDamage;

                _player.PlayerAnimator.PlayTargetAnimation("Riposte", true);
                enemyCharacterManager.GetComponentInChildren<CharacterAnimatorManager>().PlayTargetAnimation("Riposted", true);
            }
        }
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
