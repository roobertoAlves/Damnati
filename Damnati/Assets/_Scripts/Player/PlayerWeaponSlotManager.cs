using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerWeaponSlotManager : CharacterWeaponSlotManager
{
    private PlayerManager _player;
    protected override void Awake() 
    {   
        base.Awake();
        _player = GetComponent<PlayerManager>();
    }
    
    public override void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
    {
        if(weaponItem != null)
        {
            if(isLeft)
            {
                LeftHandSlot.CurrentWeapon = weaponItem;
                LeftHandSlot.LoadWeaponModel(weaponItem);
                LoadLeftWeaponDamageCollider();
                //_player.PlayerAnimator.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, true);
            }
            else
            {
                if(_player.PlayerInput.TwoHandFlag)
                {
                    BackSlot.LoadWeaponModel(LeftHandSlot.CurrentWeapon);
                    LeftHandSlot.UnloadWeaponAndDestroy();
                    _player.PlayerAnimator.PlayTargetAnimation("Left Arm Empty", false, true);
                }
                else
                {
                    BackSlot.UnloadWeaponAndDestroy();   
                }
                
                RightHandSlot.CurrentWeapon = weaponItem;
                RightHandSlot.LoadWeaponModel(weaponItem);
                LoadRightWeaponDamageCollider();
                _player.Animator.runtimeAnimatorController = weaponItem.weaponController;  
            }
            
        }
        else
        {
            weaponItem = UnarmedWeapon;

            if(isLeft)
            {
                _player.PlayerInventory.leftHandWeapon = weaponItem;
                LeftHandSlot.CurrentWeapon = weaponItem;
                LeftHandSlot.LoadWeaponModel(weaponItem);
                LoadLeftWeaponDamageCollider();
                //_player.PlayerAnimator.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, true);
            }
            else
            {
                _player.PlayerInventory.rightHandWeapon = weaponItem;
                RightHandSlot.CurrentWeapon = weaponItem;
                RightHandSlot.LoadWeaponModel(weaponItem);
                LoadRightWeaponDamageCollider(); 
                _player.Animator.runtimeAnimatorController = weaponItem.weaponController; 
            }
        }
    }

}
