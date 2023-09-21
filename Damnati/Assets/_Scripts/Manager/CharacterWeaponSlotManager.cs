using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterWeaponSlotManager : MonoBehaviour
{
    protected CharacterManager character;

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


    [Header("Hand IK Targets")]
    [Space(15)]
    
    private RightHandIKTarget _rightHandIKTarget;
    private LeftHandIKTarget _leftHandIKTarget;

    #region GET & SET
    public RightHandIKTarget RightHandIKTarget { get { return _rightHandIKTarget; }}
    public LeftHandIKTarget LeftHandIKTarget { get { return _leftHandIKTarget; }}

    #endregion
    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
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
        LoadWeaponOnSlot(character.CharacterInventory.rightHandWeapon, false);
        LoadWeaponOnSlot(character.CharacterInventory.leftHandWeapon, true);
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
                //character.CharacterAnimator.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, true);
            }
            else
            {
                if(character.IsTwoHandingWeapon)
                {
                    BackSlot.LoadWeaponModel(LeftHandSlot.CurrentWeapon);
                    LeftHandSlot.UnloadWeaponAndDestroy();
                    character.CharacterAnimator.PlayTargetAnimation("Left Arm Empty", false, true);
                }
                else
                {
                    BackSlot.UnloadWeaponAndDestroy();   
                }
                
                RightHandSlot.CurrentWeapon = weaponItem;
                RightHandSlot.LoadWeaponModel(weaponItem);
                LoadRightWeaponDamageCollider();
                LoadTwoHandIKTargets(character.IsTwoHandingWeapon);
                character.Animator.runtimeAnimatorController = weaponItem.weaponController;  
            }
            
        }
        else
        {
            weaponItem = UnarmedWeapon;

            if(isLeft)
            {
                character.CharacterInventory.leftHandWeapon = weaponItem;
                LeftHandSlot.CurrentWeapon = weaponItem;
                LeftHandSlot.LoadWeaponModel(weaponItem);
                LoadLeftWeaponDamageCollider();
                //character.CharacterAnimator.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, true);
            }
            else
            {
                character.CharacterInventory.rightHandWeapon = weaponItem;
                RightHandSlot.CurrentWeapon = weaponItem;
                RightHandSlot.LoadWeaponModel(weaponItem);
                LoadRightWeaponDamageCollider(); 
                character.Animator.runtimeAnimatorController = weaponItem.weaponController; 
            }
        }
    }

    protected virtual void LoadLeftWeaponDamageCollider()
    {
        LeftHandDamageCollider = LeftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();

        LeftHandDamageCollider.PhysicalDamage = character.CharacterInventory.leftHandWeapon.PhysicalDamage;
        LeftHandDamageCollider.FireDamage = character.CharacterInventory.leftHandWeapon.FireDamage;

        LeftHandDamageCollider.characterManager = character;
        LeftHandDamageCollider.TeamIDNumber = character.CharacterStats.TeamIDNumber;

        LeftHandDamageCollider.PoiseBreak = character.CharacterInventory.leftHandWeapon.poiseBreak;
        character.CharacterEffects.LeftWeaponFX = LeftHandSlot.currentWeaponModel.GetComponentInChildren<WeaponFX>();

    }
    protected virtual void LoadRightWeaponDamageCollider()
    {
        RightHandDamageCollider = RightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
       
        RightHandDamageCollider.PhysicalDamage = character.CharacterInventory.rightHandWeapon.PhysicalDamage;
        RightHandDamageCollider.FireDamage = character.CharacterInventory.rightHandWeapon.FireDamage;

        RightHandDamageCollider.characterManager = character;
        RightHandDamageCollider.TeamIDNumber = character.CharacterStats.TeamIDNumber;

        RightHandDamageCollider.PoiseBreak = character.CharacterInventory.rightHandWeapon.poiseBreak;
        character.CharacterEffects.RightWeaponFX = RightHandSlot.currentWeaponModel.GetComponentInChildren<WeaponFX>();

    }
    public virtual void LoadTwoHandIKTargets(bool isTwoHandingWeapon)
    {
        _leftHandIKTarget = RightHandSlot.currentWeaponModel.GetComponentInChildren<LeftHandIKTarget>();
        _rightHandIKTarget = RightHandSlot.currentWeaponModel.GetComponentInChildren<RightHandIKTarget>();
        character.CharacterAnimator.SetHandIKForWeapon(_rightHandIKTarget, _leftHandIKTarget, isTwoHandingWeapon);
    }
    public virtual void OpenDamageCollider()
    {
        if(character.IsUsingRightHand)
        {
            RightHandDamageCollider.EnableDamageCollider();
        }
        else if(character.IsUsingLeftHand)
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
        WeaponItem currentWeaponBeingUsed = character.CharacterInventory.CurrentItemBeingUsed as WeaponItem;
        character.CharacterStats.TotalPoiseDefense = character.CharacterStats.TotalPoiseDefense + currentWeaponBeingUsed.offensivePoiseBonus;
    }
    public virtual void ResetWeaponAttackingPoiseBonus()
    {
        character.CharacterStats.TotalPoiseDefense = character.CharacterStats.ArmorPoiseBonus;
    }
}
