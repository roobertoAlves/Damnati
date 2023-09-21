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
    //public string offHandIdleAnimation = "Left Arm Idle 01";
    
    [Header("Weapon Type")]
    [Space(5)]
    public WeaponType weaponType;

    [Header("Damage")]
    [Space(15)]
    public int PhysicalDamage;
    public int FireDamage;
    public int criticalDamageMultiplier = 3;
    public float guardBreakModifier = 1;

    [Header("Damage Modifiers")]
    [Space(15)]
    public float lightAttackDamageModifier;
    public float heavyAttackDamageModifier;
    public float runningAttackDamageModifier;
    public float jumpingAttackDamageModifier;

    [Header("Poise")]
    [Space(15)]
    
    public float poiseBreak;
    public float offensivePoiseBonus;

    [Header("Absorption")]
    [Space(15)]
    public float physicalBlockingDamageAbsorption;
    public float fireBlockingDamageAbsorption;

    [Header("Stability")]
    [Space(15)]
    public int stability = 67;
    
    [Header("Stamina Costs")]
    [Space(15)]

    public int baseStaminaCost;
    public float lightAttackStaminaMultiplier = 1;
    public float heavyAttackStaminaMultiplier = 1;
    public float jumpingAttackStaminaMultiplier = 1;
    public float runningAttackStaminaMultiplier = 1;

    [Header("Rage Costs")]
    [Space(15)]

    public int baseRage;
    public float rageLightAttackMultiplier;
    public float rageHeavyAttackMultiplier;  

    [Header("Item Actions")]
    [Space(15)]
    public ItemActions oh_tap_LB_Action;
    public ItemActions oh_hold_LB_Action;
    public ItemActions oh_tap_RB_Action;
    public ItemActions oh_hold_RB_Action;
    public ItemActions oh_release_RB_Action;
    public ItemActions oh_tap_RT_Action;
    public ItemActions oh_hold_RT_Action;
    public ItemActions oh_hold_LT_Action;
    public ItemActions oh_hold_F_Action;
    public ItemActions oh_tap_Z_Action;
    public ItemActions oh_hold_G_Action;

    
    [Header("TWO Handed Item Actions")]
    [Space(15)]
    public ItemActions th_tap_LB_Action;
    public ItemActions th_hold_LB_Action;
    public ItemActions th_tap_RB_Action;
    public ItemActions th_hold_RB_Action;
    public ItemActions th_release_RB_Action;
    public ItemActions th_tap_RT_Action;
    public ItemActions th_hold_RT_Action;
    public ItemActions th_hold_LT_Action;
    public ItemActions th_hold_F_Action;
    public ItemActions th_tap_Z_Action;
    public ItemActions th_hold_G_Action;


}
