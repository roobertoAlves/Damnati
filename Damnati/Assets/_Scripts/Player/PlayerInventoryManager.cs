using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryManager : CharacterInventoryManager
{
    [SerializeField] private List<WeaponItem> _weaponsInventory;  

    #region GET & SET
    public List<WeaponItem> WeaponsInventory {get { return _weaponsInventory; } set {_weaponsInventory = value; }}
   #endregion

            public void ChangeRightWeapon()
        {
            currentRightWeaponIndex = currentRightWeaponIndex + 1;

            if (currentRightWeaponIndex == 0 && weaponsInRightHandSlots[0] != null)
            {
                rightHandWeapon = weaponsInRightHandSlots[currentRightWeaponIndex];
                characterWeaponSlotManager.LoadWeaponOnSlot(weaponsInRightHandSlots[currentRightWeaponIndex], false);
            }
            else if (currentRightWeaponIndex == 0 && weaponsInRightHandSlots[0] == null)
            {
                currentRightWeaponIndex = currentRightWeaponIndex + 1;
            }

            else if (currentRightWeaponIndex == 1 && weaponsInRightHandSlots[1] != null)
            {
                rightHandWeapon = weaponsInRightHandSlots[currentRightWeaponIndex];
                characterWeaponSlotManager.LoadWeaponOnSlot(weaponsInRightHandSlots[currentRightWeaponIndex], false);
            }
            else if (currentRightWeaponIndex == 1 && weaponsInRightHandSlots[1] == null)
            {
                currentRightWeaponIndex = currentRightWeaponIndex + 1;
            }

            if (currentRightWeaponIndex > weaponsInRightHandSlots.Length - 1)
            {
                currentRightWeaponIndex = -1;
                rightHandWeapon = characterWeaponSlotManager.UnarmedWeapon;
                characterWeaponSlotManager.LoadWeaponOnSlot(characterWeaponSlotManager.UnarmedWeapon, false);
            }
        }

        public void ChangeLeftWeapon()
        {
            currentLeftWeaponIndex = currentLeftWeaponIndex + 1;

            if (currentLeftWeaponIndex == 0 && weaponsInLeftHandSlots[0] != null)
            {
                leftHandWeapon = weaponsInLeftHandSlots[currentLeftWeaponIndex];
                characterWeaponSlotManager.LoadWeaponOnSlot(weaponsInLeftHandSlots[currentLeftWeaponIndex], true);
            }
            else if (currentLeftWeaponIndex == 0 && weaponsInLeftHandSlots[0] == null)
            {
                currentLeftWeaponIndex = currentLeftWeaponIndex + 1;
            }

            else if (currentLeftWeaponIndex == 1 && weaponsInLeftHandSlots[1] != null)
            {
                leftHandWeapon = weaponsInLeftHandSlots[currentLeftWeaponIndex];
                characterWeaponSlotManager.LoadWeaponOnSlot(weaponsInLeftHandSlots[currentLeftWeaponIndex], true);
            }
            else if (currentLeftWeaponIndex == 1 && weaponsInLeftHandSlots[1] == null)
            {
                currentLeftWeaponIndex = currentLeftWeaponIndex + 1;
            }

            if (currentLeftWeaponIndex > weaponsInLeftHandSlots.Length - 1)
            {
                currentLeftWeaponIndex = -1;
                leftHandWeapon = characterWeaponSlotManager.UnarmedWeapon;
                characterWeaponSlotManager.LoadWeaponOnSlot(characterWeaponSlotManager.UnarmedWeapon, true);
            }
        }
}


