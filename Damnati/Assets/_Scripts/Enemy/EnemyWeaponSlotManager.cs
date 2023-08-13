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

    private void Awake() 
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

    private void Start() 
    {
        LoadWeaponsOnBothHands();
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
            }
            else
            {
                _rightHandDamageCollider = _rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
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
}
