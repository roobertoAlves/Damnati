using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponInventorySlot : MonoBehaviour
{
    [SerializeField] private PlayerInventory _playerInventory;
    [SerializeField] private WeaponSlotManager _weaponSlotManager;
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private Image _icon;
    private WeaponItem _item;

    private void Awake() 
    {
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
            _playerInventory.WeaponsInventory.Add(_playerInventory.weaponsInRightHandSlots[0]);
            _playerInventory.weaponsInRightHandSlots[0] = _item;
            _playerInventory.WeaponsInventory.Remove(_item);
        }
        else if(_uiManager.LeftHandSlot01Selected)
        {
            _playerInventory.WeaponsInventory.Add(_playerInventory.weaponsInLeftHandSlot[0]);
            _playerInventory.weaponsInLeftHandSlot[0] = _item;
            _playerInventory.WeaponsInventory.Remove(_item);
        }
        else
        {
            return;
        }

        _playerInventory.rightHandWeapon = _playerInventory.weaponsInRightHandSlots[_playerInventory.currentRightWeaponIndex];
        _playerInventory.rightHandWeapon = _playerInventory.weaponsInLeftHandSlot[_playerInventory.currentLeftWeaponIndex];

        _weaponSlotManager.LoadWeaponOnSlot(_playerInventory.rightHandWeapon, false);
        _weaponSlotManager.LoadWeaponOnSlot(_playerInventory.leftHandWeapon, true);
    
        _uiManager.EquipmentWindowUI.LoadWeaponsOnEquipmentScreen(_playerInventory);
        _uiManager.ResetSelectedSlots();
    }
}
