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
    public WeaponItem[] weaponsInLeftHandSlot = new WeaponItem[1];

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
        //rightHandWeapon = weaponsInRightHandSlots[0];
        _playerWeaponSlotManager.LoadWeaponOnSlot(rightHandWeapon, false);

        //leftHandWeapon = weaponsInLeftHandSlot[0];
        _playerWeaponSlotManager.LoadWeaponOnSlot(leftHandWeapon, true);
    }

    public void ChangeRightWeapon()
    {
        currentRightWeaponIndex = currentRightWeaponIndex + 1;

        if (currentRightWeaponIndex > weaponsInRightHandSlots.Length - 1)
        {
            currentRightWeaponIndex = -1;
            rightHandWeapon = _playerWeaponSlotManager.unarmedWeapon;
            _playerWeaponSlotManager.LoadWeaponOnSlot(_playerWeaponSlotManager.unarmedWeapon, false);
        }
        else if (weaponsInRightHandSlots[currentRightWeaponIndex] != null)
        {
            rightHandWeapon = weaponsInRightHandSlots[currentRightWeaponIndex];
            _playerWeaponSlotManager.LoadWeaponOnSlot(weaponsInRightHandSlots[currentRightWeaponIndex], false);
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
            leftHandWeapon = _playerWeaponSlotManager.unarmedWeapon;
            _playerWeaponSlotManager.LoadWeaponOnSlot(_playerWeaponSlotManager.unarmedWeapon, false);
        }
        else if (weaponsInLeftHandSlot[currentLeftWeaponIndex] != null)
        {
            leftHandWeapon = weaponsInLeftHandSlot[currentLeftWeaponIndex];
            _playerWeaponSlotManager.LoadWeaponOnSlot(weaponsInLeftHandSlot[currentLeftWeaponIndex], false);
        }
        else
        {
            currentLeftWeaponIndex = currentLeftWeaponIndex + 1;
        }
    }
}

