using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandEquipmentSlotsUI : MonoBehaviour
{
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private Image _icon;
    private WeaponItem _weapon;

    [SerializeField] private bool _rightHandSlot01;
    [SerializeField] private bool _leftHandSlot01;


    #region GET & SET

    public bool RightHandSlot01 { get{ return _rightHandSlot01; } set{ _rightHandSlot01 = value; }}
    public bool LeftHandSlot01 { get{ return _leftHandSlot01; } set{ _leftHandSlot01 = value; }}

    #endregion
    public void AddItem(WeaponItem newWeapon)
    {
        _weapon = newWeapon;
        _icon.sprite = _weapon.itemIcon;
        _icon.enabled = true;
        gameObject.SetActive(true);
    }
    public void ClearItem()
    {
        _weapon = null;
        _icon.sprite = null;
        _icon.enabled = false;
        gameObject.SetActive(false);
    }

    public void SelectThisSlot()
    {
        if(_rightHandSlot01)
        {
            _uiManager.RightHandSlot01Selected = true;
        }
        else if(_leftHandSlot01)
        {
            _uiManager.LeftHandSlot01Selected = true;
        }
    }
}
