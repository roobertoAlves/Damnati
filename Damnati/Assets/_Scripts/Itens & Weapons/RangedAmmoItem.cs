using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Ammo")]
public class RangedAmmoItem : Item
{
    [Header("Ammo Type")]
    [Space(15)]
    public AmmoType ammoType;

    [Header("Ammo Velocity")]
    [Space(15)]
    public float forwardVelocity = 550;
    public float upwardVelocity = 0;
    public float ammoMass = 0;
    public bool useGravity = false;

    [Header("Ammo Capacity")]
    [Space(15)]
    public int carryLimit = 99;
    public int currentAmount = 99;

    [Header("Ammo Base Damage")]
    [Space(15)]
    public float physicalDamage = 50;
    public float fireDamage = 25;

    [Header("Item Models")]
    [Space(15)]
    public GameObject loadedItemModel;
    public GameObject liveAmmoModel;
    public GameObject penetradedModel;
}
