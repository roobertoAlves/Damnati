using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponSlotManager : MonoBehaviour
{

    private WeaponHolderSlot _rightHandSlot;
    private WeaponHolderSlot _leftHandSlot;

    
    [SerializeField] private WeaponItem _rightHandWeapon;
    [SerializeField] private WeaponItem _leftHandWeapon;

    private DamageCollider _leftHandDamageCollider;
    private DamageCollider _rightHandDamageCollider;

    private EnemyStatsManager _enemyStatsManager;

    private void Awake() 
    {
        _enemyStatsManager = GetComponent<EnemyStatsManager>();
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
                _leftHandSlot = weaponSlot;
            }
            else if(weaponSlot.isRightHandSlot)
            {
                _rightHandSlot = weaponSlot;
            }
        }
    }
    public void LoadWeaponOnSlot(WeaponItem weapon, bool isLeft)
    {
            if(isLeft)
            {
                _leftHandSlot.CurrentWeapon = weapon;
                _leftHandSlot.LoadWeaponModel(weapon);
                LoadWeaponDamageCollider(true);
            }
            else
            {
                _rightHandSlot.CurrentWeapon = weapon;
                _rightHandSlot.LoadWeaponModel(weapon);
                LoadWeaponDamageCollider(false);
            }
    }

    public void LoadWeaponsOnBothHands()
    {
        if(_rightHandWeapon != null)
        {
            LoadWeaponOnSlot(_rightHandWeapon, false);
        }
        if(_leftHandWeapon != null)
        {
            LoadWeaponOnSlot(_leftHandWeapon, true);
        }
    }
    public void LoadWeaponDamageCollider(bool isLeft)
    {
            if(isLeft)
            {
                _leftHandDamageCollider = _leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
                _leftHandDamageCollider.characterManager = GetComponent<CharacterManager>();
            }
            else
            {
                _rightHandDamageCollider = _rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
                _rightHandDamageCollider.characterManager = GetComponent<CharacterManager>();
            }
    }

    public void OpenDamageCollider()
    {
        _rightHandDamageCollider.EnableDamageCollider();
    }

    public void CloseDamageCollider()
    {
        _rightHandDamageCollider.DisableDamageCollider();
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
