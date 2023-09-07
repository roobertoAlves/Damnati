using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponSlotManager : CharacterWeaponSlotManager
{
    private PlayerStatsManager _playerStatsManager;
    private PlayerManager _playerManager;
    private PlayerInventoryManager _playerInventoryManager;
    private InputHandler _inputHandler;
    private Animator _animator;

    [Header("Attacking Weapon")]
    [Space(15)]
    public WeaponItem attackingWeapon;
    private void Awake() 
    {   
        _playerManager = GetComponent<PlayerManager>();
        _inputHandler = FindObjectOfType<InputHandler>();
        _playerStatsManager = GetComponent<PlayerStatsManager>();
        _animator = GetComponent<Animator>();
        _playerInventoryManager = GetComponent<PlayerInventoryManager>();

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
                _animator.CrossFade(weaponItem.left_Hand_Idle, 0.2f);
            }
            else
            {
                if(_inputHandler.TwoHandFlag)
                {
                    BackSlot.LoadWeaponModel(LeftHandSlot.CurrentWeapon);
                    LeftHandSlot.UnloadWeaponAndDestroy();
                    _animator.CrossFade(weaponItem.th_idle, 0.2f);
                }
                else
                {
                    _animator.CrossFade("Both Arms Empty", 0.2f);
                    BackSlot.UnloadWeaponAndDestroy();
                    _animator.CrossFade(weaponItem.right_Hand_Idle, 0.2f);
                }
                
                RightHandSlot.CurrentWeapon = weaponItem;
                RightHandSlot.LoadWeaponModel(weaponItem);
                LoadRightWeaponDamageCollider();  
            }
            
        }
        else
        {
            weaponItem = UnarmedWeapon;

            if(isLeft)
            {
                _animator.CrossFade("Left Arm Empty", 0.2f);
                _playerInventoryManager.leftHandWeapon = weaponItem;
                LeftHandSlot.CurrentWeapon = weaponItem;
                LeftHandSlot.LoadWeaponModel(weaponItem);
                LoadLeftWeaponDamageCollider();
            }
            else
            {
                _animator.CrossFade("Right Arm Empty", 0.2f);
                _playerInventoryManager.rightHandWeapon = weaponItem;
                RightHandSlot.CurrentWeapon = weaponItem;
                RightHandSlot.LoadWeaponModel(weaponItem);
                LoadRightWeaponDamageCollider();  
            }
        }
    }

    #region Handle Weapon's Damage Collider

    private void LoadLeftWeaponDamageCollider()
    {
        LeftHandDamageCollider = LeftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        LeftHandDamageCollider.CurrentWeaponDamage = _playerInventoryManager.leftHandWeapon.baseDamage;
        LeftHandDamageCollider.PoiseBreak = _playerInventoryManager.leftHandWeapon.poiseBreak;
    }
    private void LoadRightWeaponDamageCollider()
    {
        RightHandDamageCollider = RightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        RightHandDamageCollider.CurrentWeaponDamage = _playerInventoryManager.rightHandWeapon.baseDamage;
        RightHandDamageCollider.PoiseBreak = _playerInventoryManager.rightHandWeapon.poiseBreak;
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
