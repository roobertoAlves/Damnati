using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponInventorySlot : MonoBehaviour
{
    private PlayerInventoryManager _playerInventoryManager;
    private PlayerWeaponSlotManager _playerWeaponSlotManager;
    private UIManager _uiManager;
    
    [SerializeField] private Image _icon;
    private WeaponItem _item;

    private void Awake() 
    {
        _playerInventoryManager =  FindObjectOfType<PlayerInventoryManager>();
        _playerWeaponSlotManager =  FindObjectOfType<PlayerWeaponSlotManager>();
        _uiManager = FindObjectOfType<UIManager>();
    }

    public void AddItem(WeaponItem newItem)
    {
        _item = newItem;
        _icon.sprite = _item.itemIcon;
        gameObject.SetActive(true);
        _icon.enabled = true;
    }

    public void ClearInventorySlot()
    {
        _item = null;
        _icon.sprite = null;
        _icon.enabled = false;
        gameObject.SetActive(false);
    }
    public void EquipThisItem()
    {
        if(_uiManager.RightHandSlot01Selected)
        {
            _playerInventoryManager.WeaponsInventory.Add(_playerInventoryManager.weaponsInRightHandSlots[0]);
            _playerInventoryManager.weaponsInRightHandSlots[0] = _item;
            _playerInventoryManager.WeaponsInventory.Remove(_item);
        }
        else if(_uiManager.LeftHandSlot01Selected)
        {
            _playerInventoryManager.WeaponsInventory.Add(_playerInventoryManager.weaponsInLeftHandSlots[0]);
            _playerInventoryManager.weaponsInLeftHandSlots[0] = _item;
            _playerInventoryManager.WeaponsInventory.Remove(_item);
        }
        else
        {
            return;
        }

        _playerInventoryManager.rightHandWeapon = _playerInventoryManager.weaponsInRightHandSlots[_playerInventoryManager.currentRightWeaponIndex];
        _playerInventoryManager.rightHandWeapon = _playerInventoryManager.weaponsInLeftHandSlots[_playerInventoryManager.currentLeftWeaponIndex];

        _playerWeaponSlotManager.LoadWeaponOnSlot(_playerInventoryManager.rightHandWeapon, false);
        _playerWeaponSlotManager.LoadWeaponOnSlot(_playerInventoryManager.leftHandWeapon, true);
    
        _uiManager.EquipmentWindowUI.LoadWeaponsOnEquipmentScreen(_playerInventoryManager);
        _uiManager.ResetSelectedSlots();
    }
}
