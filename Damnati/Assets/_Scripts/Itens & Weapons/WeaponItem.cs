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

    public string SS_Heavy_Slash_1;
    public string SS_Heavy_Slash_2;
    public string SS_Light_Slash_1;
    public string SS_Light_Slash_2;
    public string TH_Light_Slash_1;
    public string TH_Light_Slash_2;

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
}
