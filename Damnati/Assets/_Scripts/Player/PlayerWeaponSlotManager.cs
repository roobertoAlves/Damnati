using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerWeaponSlotManager : CharacterWeaponSlotManager
{
    private PlayerStatsManager _playerStatsManager;
    private PlayerManager _playerManager;
    private PlayerInventoryManager _playerInventoryManager;
    private InputHandler _inputHandler;
    private PlayerEffectsManager _playerEffectsManager;
    private PlayerAnimatorManager _playerAnimatorManager;
    private CameraHandler _cameraHandler;
    protected override void Awake() 
    {   
        base.Awake();
        _cameraHandler = FindObjectOfType<CameraHandler>();
        _playerManager = GetComponent<PlayerManager>();
        _inputHandler = FindObjectOfType<InputHandler>();
        _playerStatsManager = GetComponent<PlayerStatsManager>();
        _playerInventoryManager = GetComponent<PlayerInventoryManager>();
        _playerEffectsManager = GetComponent<PlayerEffectsManager>();
        _playerAnimatorManager = GetComponent<PlayerAnimatorManager>();

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
                _playerAnimatorManager.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, true);
            }
            else
            {
                if(_inputHandler.TwoHandFlag)
                {
                    BackSlot.LoadWeaponModel(LeftHandSlot.CurrentWeapon);
                    LeftHandSlot.UnloadWeaponAndDestroy();
                    _playerAnimatorManager.PlayTargetAnimation("Left Arm Empty", false, true);
                }
                else
                {
                    BackSlot.UnloadWeaponAndDestroy();   
                }
                
                RightHandSlot.CurrentWeapon = weaponItem;
                RightHandSlot.LoadWeaponModel(weaponItem);
                LoadRightWeaponDamageCollider();
                _playerAnimatorManager.Anim.runtimeAnimatorController = weaponItem.weaponController;  
            }
            
        }
        else
        {
            weaponItem = UnarmedWeapon;

            if(isLeft)
            {
                _playerInventoryManager.leftHandWeapon = weaponItem;
                LeftHandSlot.CurrentWeapon = weaponItem;
                LeftHandSlot.LoadWeaponModel(weaponItem);
                LoadLeftWeaponDamageCollider();
                _playerAnimatorManager.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, true);
            }
            else
            {
                _playerInventoryManager.rightHandWeapon = weaponItem;
                RightHandSlot.CurrentWeapon = weaponItem;
                RightHandSlot.LoadWeaponModel(weaponItem);
                LoadRightWeaponDamageCollider(); 
                _playerAnimatorManager.Anim.runtimeAnimatorController = weaponItem.weaponController; 
            }
        }
    }

}
