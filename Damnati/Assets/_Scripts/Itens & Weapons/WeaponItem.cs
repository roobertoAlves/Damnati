using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapon Item")]
public class WeaponItem : Item
{   
    public GameObject modelPrefab;
    public bool isUnarmed;

    [Header("Animator Replacer")]
    [Space(15)]
    public AnimatorOverrideController weaponController;
    public string offHandIdleAnimation = "Left Arm Idle 01";
    
    [Header("Weapon Type")]
    [Space(5)]
    public WeaponType weaponType;

    [Header("Damage")]
    [Space(15)]
    public int PhysicalDamage;
    public int FireDamage;
    public int criticalDamageMultiplier = 3;

    [Header("Poise")]
    [Space(15)]
    
    public float poiseBreak;
    public float offensivePoiseBonus;

    [Header("Absorption")]
    [Space(15)]
    public float physicalDamageAbsorption;
    
    [Header("Stamina Costs")]
    [Space(15)]

    public int baseStamina;
    public float lightAttackMultiplier;
    public float heavyAttackMultiplier;
    public float thLightAttackMutiplier;
    public float thHeavyAttackMultiplier;

    [Header("Rage Costs")]
    [Space(15)]

    public int baseRage;
    public float rageLightAttackMultiplier;
    public float rageHeavyAttackMultiplier;  

}
