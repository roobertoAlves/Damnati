using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInventoryManager : MonoBehaviour
{
    protected CharacterWeaponSlotManager characterWeaponSlotManager;

    [Header("Current Item Being Used")]
    [Space(15)]
    [SerializeField] private Item _currentItemBeingUsed;

    [Header("Current Equipment")]
    [Space(15)]
    public WeaponItem rightHandWeapon;
    public WeaponItem leftHandWeapon;
    public RangedAmmoItem currentAmmo;

    public WeaponItem[] weaponsInRightHandSlots = new WeaponItem[1];
    public WeaponItem[] weaponsInLeftHandSlots = new WeaponItem[1];

    public int currentRightWeaponIndex = 0;
    public int currentLeftWeaponIndex = 0;

    #region GET & SET
    public Item CurrentItemBeingUsed { get { return _currentItemBeingUsed; } set { _currentItemBeingUsed = value; }}
    #endregion
    private void Awake() 
    {
        characterWeaponSlotManager = GetComponent<CharacterWeaponSlotManager>();    
    }
    private void Start()
    {
        characterWeaponSlotManager.LoadBothWeaponsOnSlots();
    }
}
