using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInventoryManager : MonoBehaviour
{
    protected CharacterWeaponSlotManager characterWeaponSlotManager;
    public WeaponItem rightHandWeapon;
    public WeaponItem leftHandWeapon;

    [Header("Current Equipment")]
    [Space(15)]

    public WeaponItem[] weaponsInRightHandSlots = new WeaponItem[1];
    public WeaponItem[] weaponsInLeftHandSlots = new WeaponItem[1];

    public int currentRightWeaponIndex = -1;
    public int currentLeftWeaponIndex = -1;

    private void Awake() 
    {
        characterWeaponSlotManager = GetComponent<CharacterWeaponSlotManager>();    
    }
    private void Start()
    {
        characterWeaponSlotManager.LoadBothWeaponsOnSlots();
    }
}
