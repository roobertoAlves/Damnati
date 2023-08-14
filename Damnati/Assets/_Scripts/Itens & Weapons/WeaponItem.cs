using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapon Item")]
public class WeaponItem : Item
{   
    public GameObject modelPrefab;
    public bool isUnarmed;

    [Header("Idle Animations")]
    [Space(15)]

    public string right_Hand_Idle;
    public string left_Hand_Idle;
    public string th_idle;


    [Header("Attack Animations")]
    [Space(15)]

    public string SS_Heavy_Slash_01;
    public string SS_Heavy_Slash_02;
    public string SS_Heavy_Slash_03;

    public string SS_Light_Slash_01;
    public string SS_Light_Slash_02;
    public string SS_Light_Slash_03;

    public string TH_Light_Slash_01;
    public string TH_Light_Slash_02;
    public string TH_Light_Slash_03;

    public string TH_Heavy_Slash_01;
    public string TH_Heavy_Slash_02;
    public string TH_Heavy_Slash_03;
    
    [Header("Stamina Costs")]
    [Space(15)]

    public int baseStamina;
    public float lightAttackMultiplier;
    public float heavyAttackMultiplier;

    [Header("Rage Costs")]
    [Space(15)]

    public int baseRage;
    public float rageLightAttackMultiplier;
    public float rageHeavyAttackMultiplier;  

    [Header("Weapon Type")]
    public bool isMeleeWeapon;
}
