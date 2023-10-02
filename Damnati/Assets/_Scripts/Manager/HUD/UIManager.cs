using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    [Header("Components")]
    [Space(15)]
    private PlayerManager _playerManager;
    [SerializeField] private EquipmentWindowUI _equipmentWindowUI;

    [Header("HUD")]
    [Space(15)]
    [SerializeField] private GameObject _crossHair;

    [Header("UI Window")]
    [Space(15)]
    [SerializeField] private GameObject _hudWindow;
    [SerializeField] private GameObject _inventoryWindow;
    [SerializeField] private GameObject _weaponInventoryWindow;
    [SerializeField] private GameObject _equipmentScreenWindow;


    [Header("Weapon Inventory")]
    [Space(15)]
    [SerializeField] GameObject _weaponInventorySlotPrefab;
    [SerializeField] private Transform _weaponInventorySlotsParent;
    WeaponInventorySlot[] _weaponsInventorySlots;

    [Header("Equipment Window Slot Selected")]
    [Space(15)]
    [SerializeField] private bool _rightHandSlot01Selected;
    [SerializeField] private bool _leftHandSlot01Selected;


    #region GET & SET

    public GameObject HudWindow {get { return _hudWindow; } set { _hudWindow = value; }}
    public EquipmentWindowUI EquipmentWindowUI {get { return _equipmentWindowUI; } set { _equipmentWindowUI = value; }}
    public bool RightHandSlot01Selected {get { return _rightHandSlot01Selected; } set { _rightHandSlot01Selected = value; }}
    public bool LeftHandSlot01Selected {get { return _leftHandSlot01Selected; } set { _leftHandSlot01Selected = value; }}
    public GameObject CrossHair { get { return _crossHair; } set { _crossHair = value; }}
    #endregion

    private void Awake() 
    {
        _playerManager = FindObjectOfType<PlayerManager>();
        _equipmentWindowUI = FindObjectOfType<EquipmentWindowUI>();    
    }
    private void Start() 
    {
        _weaponsInventorySlots = _weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
    }
    public void UpdateUI()
    {
        #region Weapon Inventory

        for(int i = 0; i < _weaponsInventorySlots.Length; i++)
        {
            if(i < _playerManager.PlayerInventory.WeaponsInventory.Count)
            {
                if(_weaponsInventorySlots.Length < _playerManager.PlayerInventory.WeaponsInventory.Count)
                {
                    Instantiate(_weaponInventorySlotPrefab, _weaponInventorySlotsParent);
                    _weaponsInventorySlots = _weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
                }
                _weaponsInventorySlots[i].AddItem(_playerManager.PlayerInventory.WeaponsInventory[i]);
            }
            else
            {
                _weaponsInventorySlots[i].ClearInventorySlot();
            }
        }

        #endregion
    }
    public void OpenPauseWindow()
    {
        _inventoryWindow.SetActive(true);
    }
    public void ClosePauseWindow()
    {
        _inventoryWindow.SetActive(false);
    }

    public void CloseAllInventoryWindows()
    {
        ResetSelectedSlots();
        _weaponInventoryWindow.SetActive(false);
        _equipmentScreenWindow.SetActive(false);
    }
    public void ResetSelectedSlots()
    {
        _rightHandSlot01Selected = false;
        _leftHandSlot01Selected = false;
    }
}
