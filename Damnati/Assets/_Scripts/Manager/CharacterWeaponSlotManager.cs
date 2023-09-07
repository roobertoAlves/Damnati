using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterWeaponSlotManager : MonoBehaviour
{
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



}
