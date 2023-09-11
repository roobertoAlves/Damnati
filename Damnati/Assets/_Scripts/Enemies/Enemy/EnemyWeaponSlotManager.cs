using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponSlotManager : CharacterWeaponSlotManager
{

    [Header("Enemy Weapon")]
    [Space(15)]
    [SerializeField] private WeaponItem _rightHandWeapon;
    [SerializeField] private WeaponItem _leftHandWeapon;

    private EnemyStatsManager _enemyStatsManager;
    private EnemyEffectsManager _enemyEffectsManager;

    private void Awake() 
    {
        _enemyStatsManager = GetComponent<EnemyStatsManager>();
        _enemyEffectsManager = GetComponent<EnemyEffectsManager>();
        LoadWeaponHolderSlots();
            
    }

    private void Start() 
    {
        LoadWeaponsOnBothHands();
    }
    private void LoadWeaponHolderSlots()
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
        }
    }
    public void LoadWeaponOnSlot(WeaponItem weapon, bool isLeft)
    {
            if(isLeft)
            {
                LeftHandSlot.CurrentWeapon = weapon;
                LeftHandSlot.LoadWeaponModel(weapon);
                LoadWeaponDamageCollider(true);
            }
            else
            {
                RightHandSlot.CurrentWeapon = weapon;
                RightHandSlot.LoadWeaponModel(weapon);
                LoadWeaponDamageCollider(false);
            }
    }
    public void LoadWeaponsOnBothHands()
    {
        if(RightHandSlot != null)
        {
            LoadWeaponOnSlot(_rightHandWeapon, false);
        }
        if(LeftHandSlot != null)
        {
            LoadWeaponOnSlot(_leftHandWeapon, true);
        }
    }
    public void LoadWeaponDamageCollider(bool isLeft)
    {
            if(isLeft)
            {
                LeftHandDamageCollider = LeftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
                LeftHandDamageCollider.characterManager = GetComponent<CharacterManager>();
                
                LeftHandDamageCollider.PhysicalDamage = _leftHandWeapon.PhysicalDamage;

                LeftHandDamageCollider.TeamIDNumber = _enemyStatsManager.TeamIDNumber;

                _enemyEffectsManager.LeftWeaponFX = LeftHandSlot.currentWeaponModel.GetComponentInChildren<WeaponFX>();

            }
            else
            {
                RightHandDamageCollider = RightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
                RightHandDamageCollider.characterManager = GetComponent<CharacterManager>();

                RightHandDamageCollider.PhysicalDamage = _rightHandWeapon.PhysicalDamage;

                RightHandDamageCollider.TeamIDNumber = _enemyStatsManager.TeamIDNumber;
                
                _enemyEffectsManager.RightWeaponFX = RightHandSlot.currentWeaponModel.GetComponentInChildren<WeaponFX>();
            }
    }

    public void OpenDamageCollider()
    {
        RightHandDamageCollider.EnableDamageCollider();
    }

    public void CloseDamageCollider()
    {
        RightHandDamageCollider.DisableDamageCollider();
    }

    #region Combat & Animation Events
    public void DrainStaminaLightAttack()
    {

    }

    public void DrainStaminaHeavyAttack()
    {

    }

    public void EnableCombo()
    {
        //anim.SetBool("canDoCombo", true);
    }

    public void DisableCombo()
    {
        //anim.SetBool("canDoCombo", false);
    }
    #endregion

    #region Handle Weapon's Poise Bonus

    public void GrantWeaponAttackingPoiseBonus()
    {
        _enemyStatsManager.TotalPoiseDefense = _enemyStatsManager.TotalPoiseDefense + _enemyStatsManager.OffensivePoiseBonus;
    }

    public void ResetWeaponAttackingPoiseBonus()
    {
        _enemyStatsManager.TotalPoiseDefense = _enemyStatsManager.ArmorPoiseBonus;
    }

    #endregion
}
