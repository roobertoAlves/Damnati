using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterWeaponSlotManager : MonoBehaviour
{
    protected CharacterManager characterManager;
    protected CharacterStatsManager characterStatsManager;
    protected CharacterEffectsManager characterEffectsManager;
    protected CharacterInventoryManager characterInventoryManager;
    protected CharacterAnimatorManager characterAnimatorManager;

    [Header("Unarmed Weapon")]
    [Space(15)]
    public WeaponItem UnarmedWeapon;

    [Header("Weapon Slots")]
    [Space(15)]
    public WeaponHolderSlot LeftHandSlot;
    public WeaponHolderSlot RightHandSlot;
    public WeaponHolderSlot BackSlot;

    [Header("Damage Colliders")]
    [Space(15)]
    public DamageCollider LeftHandDamageCollider;
    public DamageCollider RightHandDamageCollider;

    [Header("Attacking Weapon")]
    [Space(15)]
    public WeaponItem AttackingWeapon;

    [Header("Hand IK Targets")]
    [Space(15)]
    
    private RightHandIKTarget _rightHandIKTarget;
    private LeftHandIKTarget _leftHandIKTarget;

    protected virtual void Awake()
    {
        characterManager = GetComponent<CharacterManager>();
        characterStatsManager = GetComponent<CharacterStatsManager>();
        characterEffectsManager = GetComponent<CharacterEffectsManager>();
        characterInventoryManager = GetComponent<CharacterInventoryManager>();
        characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
        LoadWeaponHolderSlots();
    }
    private void Start()
    {

    }

    protected virtual void LoadWeaponHolderSlots()
    {
        WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();    
        
        foreach (WeaponHolderSlot weaponSlot in weaponHolderSlots)
        {
            if(weaponSlot.isLeftHandSlot)
            {
                LeftHandSlot = weaponSlot;
            }
            else if(weaponSlot.isRightHandSlot)
            {
                RightHandSlot = weaponSlot;
            }
            else if(weaponSlot.isBackSlot)
            {
                BackSlot = weaponSlot;
            }
        }
    }

    public virtual void LoadBothWeaponsOnSlots()
    {
        LoadWeaponOnSlot(characterInventoryManager.rightHandWeapon, false);
        LoadWeaponOnSlot(characterInventoryManager.leftHandWeapon, true);
    }
    public virtual void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
    {
        if(weaponItem != null)
        {
            if(isLeft)
            {
                LeftHandSlot.CurrentWeapon = weaponItem;
                LeftHandSlot.LoadWeaponModel(weaponItem);
                LoadLeftWeaponDamageCollider();
                characterAnimatorManager.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, true);
            }
            else
            {
                if(characterManager.IsTwoHandingWeapon)
                {
                    BackSlot.LoadWeaponModel(LeftHandSlot.CurrentWeapon);
                    LeftHandSlot.UnloadWeaponAndDestroy();
                    characterAnimatorManager.PlayTargetAnimation("Left Arm Empty", false, true);
                }
                else
                {
                    BackSlot.UnloadWeaponAndDestroy();   
                }
                
                RightHandSlot.CurrentWeapon = weaponItem;
                RightHandSlot.LoadWeaponModel(weaponItem);
                LoadRightWeaponDamageCollider();
                characterAnimatorManager.Anim.runtimeAnimatorController = weaponItem.weaponController;  
            }
            
        }
        else
        {
            weaponItem = UnarmedWeapon;

            if(isLeft)
            {
                characterInventoryManager.leftHandWeapon = weaponItem;
                LeftHandSlot.CurrentWeapon = weaponItem;
                LeftHandSlot.LoadWeaponModel(weaponItem);
                LoadLeftWeaponDamageCollider();
                characterAnimatorManager.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, true);
            }
            else
            {
                characterInventoryManager.rightHandWeapon = weaponItem;
                RightHandSlot.CurrentWeapon = weaponItem;
                RightHandSlot.LoadWeaponModel(weaponItem);
                LoadRightWeaponDamageCollider(); 
                characterAnimatorManager.Anim.runtimeAnimatorController = weaponItem.weaponController; 
            }
        }
    }

    protected virtual void LoadLeftWeaponDamageCollider()
    {
        LeftHandDamageCollider = LeftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();

        LeftHandDamageCollider.PhysicalDamage = characterInventoryManager.leftHandWeapon.PhysicalDamage;

        LeftHandDamageCollider.TeamIDNumber = characterStatsManager.TeamIDNumber;

        LeftHandDamageCollider.PoiseBreak = characterInventoryManager.leftHandWeapon.poiseBreak;
        characterEffectsManager.LeftWeaponFX = LeftHandSlot.currentWeaponModel.GetComponentInChildren<WeaponFX>();

    }
    protected virtual void LoadRightWeaponDamageCollider()
    {
        RightHandDamageCollider = RightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
       
        RightHandDamageCollider.PhysicalDamage = characterInventoryManager.rightHandWeapon.PhysicalDamage;
        
        RightHandDamageCollider.TeamIDNumber = characterStatsManager.TeamIDNumber;

        RightHandDamageCollider.PoiseBreak = characterInventoryManager.rightHandWeapon.poiseBreak;
        characterEffectsManager.RightWeaponFX = RightHandSlot.currentWeaponModel.GetComponentInChildren<WeaponFX>();

    }
    public virtual void LoadTwoHandIKTargets(bool isTwoHandingWeapon)
    {
        _leftHandIKTarget = RightHandSlot.currentWeaponModel.GetComponentInChildren<LeftHandIKTarget>();
        _rightHandIKTarget = RightHandSlot.currentWeaponModel.GetComponentInChildren<RightHandIKTarget>();
        characterAnimatorManager.SetHandIKForWeapon(_rightHandIKTarget, _leftHandIKTarget, isTwoHandingWeapon);
    }
    public virtual void OpenDamageCollider()
    {
        if(characterManager.IsUsingRightHand)
        {
            RightHandDamageCollider.EnableDamageCollider();
        }
        else if(characterManager.IsUsingLeftHand)
        {
            LeftHandDamageCollider.EnableDamageCollider();
        }
    }
    public virtual void CloseDamageCollider()
    {
        if(RightHandDamageCollider != null)
        {
            RightHandDamageCollider.DisableDamageCollider();
        }
        
        if(LeftHandDamageCollider != null)
        {
            LeftHandDamageCollider.DisableDamageCollider();
        }
    }
    
    public virtual void GrantWeaponAttackingPoiseBonus()
    {
        characterStatsManager.TotalPoiseDefense = characterStatsManager.TotalPoiseDefense + AttackingWeapon.offensivePoiseBonus;
    }
    public virtual void ResetWeaponAttackingPoiseBonus()
    {
        characterStatsManager.TotalPoiseDefense = characterStatsManager.ArmorPoiseBonus;
    }
}
