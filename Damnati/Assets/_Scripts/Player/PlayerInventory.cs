using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{

    [SerializeField] private WeaponSlotManager _weaponSlot;

    [Header("Weapons")]
    [Space(15)]
    public WeaponItem rightHandWeapon;
    public WeaponItem leftHandWeapon;

    public WeaponItem unarmedWeapon;

    public WeaponItem[] weaponsInRightHandSlots = new WeaponItem[1];
    public WeaponItem[] weaponsInLeftHandSlot = new WeaponItem[1];

    public int currentRightWeaponIndex = -1;
    public int currentLeftWeaponIndex = -1;

    [SerializeField] private List<WeaponItem> _weaponsInventory;


    #region GET & SET

    public List<WeaponItem> WeaponsInventory {get { return _weaponsInventory; } set {_weaponsInventory = value; }}

    #endregion

    private void Start() 
    { 
        rightHandWeapon = weaponsInRightHandSlots[0];
        _weaponSlot.LoadWeaponOnSlot(rightHandWeapon, false);

        leftHandWeapon = weaponsInLeftHandSlot[0];
        _weaponSlot.LoadWeaponOnSlot(leftHandWeapon, true);
    }

    public void ChangeRightWeapon()
    {
        currentRightWeaponIndex = currentRightWeaponIndex + 1;

        if (currentRightWeaponIndex > weaponsInRightHandSlots.Length - 1)
        {
            currentRightWeaponIndex = -1;
            rightHandWeapon = unarmedWeapon;
            _weaponSlot.LoadWeaponOnSlot(unarmedWeapon, false);
        }
        else if (weaponsInRightHandSlots[currentRightWeaponIndex] != null)
        {
            rightHandWeapon = weaponsInRightHandSlots[currentRightWeaponIndex];
            _weaponSlot.LoadWeaponOnSlot(weaponsInRightHandSlots[currentRightWeaponIndex], false);
        }
        else
        {
            currentRightWeaponIndex = currentRightWeaponIndex + 1;
        }
    }

    public void ChangeLeftWeapon()
    {
        currentLeftWeaponIndex = currentLeftWeaponIndex + 1;

        if (currentLeftWeaponIndex > weaponsInLeftHandSlot.Length - 1)
        {
            currentLeftWeaponIndex = -1;
            leftHandWeapon = unarmedWeapon;
            _weaponSlot.LoadWeaponOnSlot(unarmedWeapon, false);
        }
        else if (weaponsInLeftHandSlot[currentLeftWeaponIndex] != null)
        {
            leftHandWeapon = weaponsInLeftHandSlot[currentLeftWeaponIndex];
            _weaponSlot.LoadWeaponOnSlot(weaponsInLeftHandSlot[currentLeftWeaponIndex], false);
        }
        else
        {
            currentLeftWeaponIndex = currentLeftWeaponIndex + 1;
        }
    }
}

