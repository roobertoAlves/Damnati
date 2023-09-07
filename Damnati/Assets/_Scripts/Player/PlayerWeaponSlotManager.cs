using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponSlotManager : MonoBehaviour
{
    private PlayerStatsManager _playerStatsManager;
    private PlayerManager _playerManager;
    private PlayerInventoryManager _playerInventoryManager;
    private Animator _anim;
    
    [Header("Weapon Slots")]
    [Space(15)]
    private WeaponHolderSlot _leftHandSlot;
    private WeaponHolderSlot _rightHandSlot;
    private WeaponHolderSlot _backSlot;

    [Header("Damage Colliders")]
    [Space(15)]
    private DamageCollider _leftHandDamageCollider;
    private DamageCollider _rightHandDamageCollider;

    [Header("Attacking Weapon")]
    [Space(15)]
    public WeaponItem attackingWeapon;

    [Header("Unarmed Weapon")]
    [Space(15)]
    public WeaponItem unarmedWeapon;


    #region GET & SET

    public DamageCollider LeftHandDamageCollider { get { return _leftHandDamageCollider; } set { _leftHandDamageCollider = value; }}
    public DamageCollider RightHandDamageCollider { get { return _rightHandDamageCollider; } set { _rightHandDamageCollider = value; }}
    #endregion
    private void Awake() 
    {   
        _playerManager = GetComponent<PlayerManager>();
        _playerStatsManager = GetComponent<PlayerStatsManager>();
        _anim = GetComponent<Animator>();
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
                _leftHandSlot = weaponSlot;
            }
            else if(weaponSlot.isRightHandSlot)
            {
                _rightHandSlot = weaponSlot;
            }
            else if(weaponSlot.isBackSlot)
            {
                _backSlot = weaponSlot;
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
                _leftHandSlot.CurrentWeapon = weaponItem;
                _anim.CrossFade(weaponItem.left_Hand_Idle, 0.2f);
            }
            else
            {
                if(_playerManager.TwoHandFlag)
                {
                    _backSlot.LoadWeaponModel(_leftHandSlot.CurrentWeapon);
                    _leftHandSlot.UnloadWeaponAndDestroy();
                    _anim.CrossFade(weaponItem.th_idle, 0.2f);
                }
                else
                {
                    _anim.CrossFade("Both Arms Empty", 0.2f);
                    _backSlot.UnloadWeaponAndDestroy();
                    _anim.CrossFade(weaponItem.right_Hand_Idle, 0.2f);
                }
            }
                
            _rightHandSlot.CurrentWeapon = weaponItem;
            _rightHandSlot.LoadWeaponModel(weaponItem);
            LoadRightWeaponDamageCollider();  
        }
        else
        {
            weaponItem = unarmedWeapon;

            if(isLeft)
            {
                _anim.CrossFade("Left Arm Empty", 0.2f);
                _playerInventoryManager.leftHandWeapon = weaponItem;
                _leftHandSlot.CurrentWeapon = weaponItem;
                _leftHandSlot.LoadWeaponModel(weaponItem);
                LoadLeftWeaponDamageCollider();
            }
            else
            {
                _anim.CrossFade("Right Arm Empty", 0.2f);
                _playerInventoryManager.rightHandWeapon = weaponItem;
                _rightHandSlot.CurrentWeapon = weaponItem;
                _rightHandSlot.LoadWeaponModel(weaponItem);
                LoadRightWeaponDamageCollider();  
            }
        }
    }

    #region Handle Weapon's Damage Collider

    private void LoadLeftWeaponDamageCollider()
    {
        _leftHandDamageCollider = _leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        _leftHandDamageCollider.CurrentWeaponDamage = _playerInventoryManager.leftHandWeapon.baseDamage;
        _leftHandDamageCollider.PoiseBreak = _playerInventoryManager.leftHandWeapon.poiseBreak;
    }
    private void LoadRightWeaponDamageCollider()
    {
        _rightHandDamageCollider = _rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        _rightHandDamageCollider.CurrentWeaponDamage = _playerInventoryManager.rightHandWeapon.baseDamage;
        _rightHandDamageCollider.PoiseBreak = _playerInventoryManager.rightHandWeapon.poiseBreak;
    }
    public void OpenDamageCollider()
    {
        if(_playerManager.IsUsingRightHand)
        {
            _rightHandDamageCollider.EnableDamageCollider();
        }
        else if(_playerManager.IsUsingLeftHand)
        {
            _leftHandDamageCollider.EnableDamageCollider();
        }
    }

    public void CloseDamageCollider()
    {
        if(_rightHandDamageCollider != null)
        {
            _rightHandDamageCollider.DisableDamageCollider();
        }
        
        if(_leftHandDamageCollider != null)
        {
            _leftHandDamageCollider.DisableDamageCollider();
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
        if(_playerManager.IsInRage)
        {
            _playerStatsManager.RageDrain(Mathf.RoundToInt(attackingWeapon.baseRage * attackingWeapon.rageLightAttackMultiplier));
        }
    }
    public void DrainRageHeavyAttack()
    {
        if(_playerManager.IsInRage)
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
