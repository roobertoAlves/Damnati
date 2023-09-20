using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentWindowUI : MonoBehaviour
{
    private bool  _rightHandSlot01Selected;
    private bool _leftHandSlot01Selected;

    #region GET & SET
    public bool RightHandSlot01Selected { get { return _rightHandSlot01Selected; }}
    public bool LeftHandSlot01Selected { get { return _leftHandSlot01Selected; }}
    #endregion

    [SerializeField] private HandEquipmentSlotsUI[] _handEquipmentSlots;
    
    public void LoadWeaponsOnEquipmentScreen(PlayerInventoryManager playerInventoryManager)
    {
        for(int i = 0; i < _handEquipmentSlots.Length; i++)
        {
            if(_handEquipmentSlots[i].RightHandSlot01)
            {
                _handEquipmentSlots[i].AddItem(playerInventoryManager.weaponsInRightHandSlots[0]);
            }
            else if(_handEquipmentSlots[i].LeftHandSlot01)
            {
                _handEquipmentSlots[i].AddItem(playerInventoryManager.weaponsInLeftHandSlots[0]);
            }
        }
    }

    public void SelectRightHandSlot01()
    {
        _rightHandSlot01Selected = true;
    }  
    public void SelectLeftHandSlot01()
    {
        _leftHandSlot01Selected = true;
    }  
}
