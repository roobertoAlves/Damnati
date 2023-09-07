using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour
{

    private PlayerWeaponSlotManager _playerWeaponSlotManager;

    [Header("Weapons")]
    [Space(15)]
    public WeaponItem rightHandWeapon;
    public WeaponItem leftHandWeapon;

    public WeaponItem[] weaponsInRightHandSlots = new WeaponItem[1];
    public WeaponItem[] weaponsInLeftHandSlots = new WeaponItem[1];

    public int currentRightWeaponIndex = -1;
    public int currentLeftWeaponIndex = -1;

    [SerializeField] private List<WeaponItem> _weaponsInventory;

    
    [Header("Defensive Items Status")]
    [Space(15)]

    [SerializeField] private float _helmetPhysicalDefense;
    [SerializeField] private float _legsPhysicalDefense;
    [SerializeField] private float _bodyPhysicalDefense;
    [SerializeField] private float _handsPhysicalDefense;


    #region GET & SET

    public List<WeaponItem> WeaponsInventory {get { return _weaponsInventory; } set {_weaponsInventory = value; }}


    public float HelmetPhysicalDefense { get { return _helmetPhysicalDefense; }}
    public float LegsPhysicalDefense { get { return _legsPhysicalDefense; }}
    public float ChesplatePhysicalDefense { get { return _bodyPhysicalDefense; }}
    public float GlovesPhysicalDefense { get { return _handsPhysicalDefense; }}
    #endregion

    private void Awake() 
    {
        _playerWeaponSlotManager = GetComponent<PlayerWeaponSlotManager>();    
    }
    private void Start() 
    { 
        rightHandWeapon = weaponsInRightHandSlots[0];
        _playerWeaponSlotManager.LoadWeaponOnSlot(rightHandWeapon, false);

        leftHandWeapon = weaponsInLeftHandSlots[0];
        _playerWeaponSlotManager.LoadWeaponOnSlot(leftHandWeapon, true);
    }


    public void ChangeRightWeapon()
    {
        currentRightWeaponIndex = currentRightWeaponIndex + 1;

        if (currentRightWeaponIndex == 0 && weaponsInRightHandSlots[0] != null)
        {
            rightHandWeapon = weaponsInRightHandSlots[currentRightWeaponIndex];
            _playerWeaponSlotManager.LoadWeaponOnSlot(weaponsInRightHandSlots[currentRightWeaponIndex], false);
        }
        else if (currentRightWeaponIndex == 0 && weaponsInRightHandSlots[0] == null)
        {
            currentRightWeaponIndex = currentRightWeaponIndex + 1;
        }

        else if (currentRightWeaponIndex == 1 && weaponsInRightHandSlots[1] != null)
        {
            rightHandWeapon = weaponsInRightHandSlots[currentRightWeaponIndex];
            _playerWeaponSlotManager.LoadWeaponOnSlot(weaponsInRightHandSlots[currentRightWeaponIndex], false);
        }
        else if (currentRightWeaponIndex == 1 && weaponsInRightHandSlots[1] == null)
        {
            currentRightWeaponIndex = currentRightWeaponIndex + 1;
        }

        if (currentRightWeaponIndex > weaponsInRightHandSlots.Length - 1)
        {
            currentRightWeaponIndex = -1;
            rightHandWeapon = _playerWeaponSlotManager.UnarmedWeapon;
             _playerWeaponSlotManager.LoadWeaponOnSlot(_playerWeaponSlotManager.UnarmedWeapon, false);
        }
    }
    public void ChangeLeftWeapon()
    {
        currentLeftWeaponIndex = currentLeftWeaponIndex + 1;

        if (currentLeftWeaponIndex == 0 && weaponsInLeftHandSlots[0] != null)
        {
            leftHandWeapon = weaponsInLeftHandSlots[currentLeftWeaponIndex];
            _playerWeaponSlotManager.LoadWeaponOnSlot(weaponsInLeftHandSlots[currentLeftWeaponIndex], true);
        }
        else if (currentLeftWeaponIndex == 0 && weaponsInLeftHandSlots[0] == null)
        {
            currentLeftWeaponIndex = currentLeftWeaponIndex + 1;
        }

        else if (currentLeftWeaponIndex == 1 && weaponsInLeftHandSlots[1] != null)
        {
            leftHandWeapon = weaponsInLeftHandSlots[currentLeftWeaponIndex];
            _playerWeaponSlotManager.LoadWeaponOnSlot(weaponsInLeftHandSlots[currentLeftWeaponIndex], true);
        }
        else if (currentLeftWeaponIndex == 1 && weaponsInLeftHandSlots[1] == null)
        {
            currentLeftWeaponIndex = currentLeftWeaponIndex + 1;
        }

        if (currentLeftWeaponIndex > weaponsInLeftHandSlots.Length - 1)
        {
            currentLeftWeaponIndex = -1;
            leftHandWeapon = _playerWeaponSlotManager.UnarmedWeapon;
            _playerWeaponSlotManager.LoadWeaponOnSlot(_playerWeaponSlotManager.UnarmedWeapon, true);
        }
    }
}


