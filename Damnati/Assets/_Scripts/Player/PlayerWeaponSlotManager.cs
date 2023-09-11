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
    private Animator _animator;
    private PlayerEffectsManager _playerEffectsManager;
    private PlayerAnimatorManager _playerAnimatorManager;
    private CameraHandler _cameraHandler;

    [Header("Attacking Weapon")]
    [Space(15)]
    public WeaponItem attackingWeapon;
    private void Awake() 
    {   
        _cameraHandler = FindObjectOfType<CameraHandler>();
        _playerManager = GetComponent<PlayerManager>();
        _inputHandler = FindObjectOfType<InputHandler>();
        _playerStatsManager = GetComponent<PlayerStatsManager>();
        _animator = GetComponent<Animator>();
        _playerInventoryManager = GetComponent<PlayerInventoryManager>();
        _playerEffectsManager = GetComponent<PlayerEffectsManager>();
        _playerAnimatorManager = GetComponent<PlayerAnimatorManager>();

        LoadWeaponHolderSlots();
    }
    public void LoadWeaponHolderSlots()
    {
        WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();    
        
        foreach (WeaponHolderSlot weaponSlot in weaponHolderSlots)
        {
            if(weaponSlot.isLeftHandSlot)
            {
                LeftHandSlot = weaponSlot;
            }
            else if(weaponSlot.isRightHandSlot)
            {
                RightHandSlot = weaponSlot;
            }
            else if(weaponSlot.isBackSlot)
            {
                BackSlot = weaponSlot;
            }
        }
    }
    public void LoadBothWeaponsOnSlots()
    {
        LoadWeaponOnSlot(_playerInventoryManager.rightHandWeapon, false);
        LoadWeaponOnSlot(_playerInventoryManager.leftHandWeapon, true);
    }
    public void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
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

    #region Handle Weapon's Damage Collider

    private void LoadLeftWeaponDamageCollider()
    {
        LeftHandDamageCollider = LeftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();

        LeftHandDamageCollider.PhysicalDamage = _playerInventoryManager.leftHandWeapon.PhysicalDamage;

        LeftHandDamageCollider.TeamIDNumber = _playerStatsManager.TeamIDNumber;

        LeftHandDamageCollider.PoiseBreak = _playerInventoryManager.leftHandWeapon.poiseBreak;
        _playerEffectsManager.LeftWeaponFX = LeftHandSlot.currentWeaponModel.GetComponentInChildren<WeaponFX>();
    }
    private void LoadRightWeaponDamageCollider()
    {
        RightHandDamageCollider = RightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
       
        RightHandDamageCollider.PhysicalDamage = _playerInventoryManager.rightHandWeapon.PhysicalDamage;
        
        RightHandDamageCollider.TeamIDNumber = _playerStatsManager.TeamIDNumber;

        RightHandDamageCollider.PoiseBreak = _playerInventoryManager.rightHandWeapon.poiseBreak;
        _playerEffectsManager.RightWeaponFX = RightHandSlot.currentWeaponModel.GetComponentInChildren<WeaponFX>();
    }
    public void OpenDamageCollider()
    {
        if(_playerManager.IsUsingRightHand)
        {
            RightHandDamageCollider.EnableDamageCollider();
        }
        else if(_playerManager.IsUsingLeftHand)
        {
            LeftHandDamageCollider.EnableDamageCollider();
        }
    }

    public void CloseDamageCollider()
    {
        if(RightHandDamageCollider != null)
        {
            RightHandDamageCollider.DisableDamageCollider();
        }
        
        if(LeftHandDamageCollider != null)
        {
            LeftHandDamageCollider.DisableDamageCollider();
        }
    }


    #endregion


    #region Stamina & Rage Drain
    public void DrainsStaminaLightAttack()
    {
        _playerStatsManager.StaminaDrain(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.lightAttackMultiplier));
    }
    public void DrainsStaminaHeavyAttack()
    {
        _playerStatsManager.StaminaDrain(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.heavyAttackMultiplier));
    }


    public void DrainRageLightAttack()
    {
        if(_inputHandler.IsInRage)
        {
            _playerStatsManager.RageDrain(Mathf.RoundToInt(attackingWeapon.baseRage * attackingWeapon.rageLightAttackMultiplier));
        }
    }
    public void DrainRageHeavyAttack()
    {
        if(_inputHandler.IsInRage)
        {
            _playerStatsManager.RageDrain(Mathf.RoundToInt(attackingWeapon.baseRage * attackingWeapon.rageHeavyAttackMultiplier));
        }
    }

    #endregion

    #region Handle Weapon's Poise Bonus

    public void WeaponAttackingPoiseBonus()
    {
        _playerStatsManager.TotalPoiseDefense = _playerStatsManager.TotalPoiseDefense + attackingWeapon.offensivePoiseBonus;
    }
    public void ResetWeaponAttackingPoiseBonus()
    {
        _playerStatsManager.TotalPoiseDefense = _playerStatsManager.ArmorPoiseBonus;
    }

    #endregion
}
