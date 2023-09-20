using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponSlotManager : CharacterWeaponSlotManager
{
    public override void GrantWeaponAttackingPoiseBonus()
    {
        character.CharacterStats.TotalPoiseDefense = character.CharacterStats.TotalPoiseDefense - character.CharacterStats.OffensivePoiseBonus;
    }
    public override void ResetWeaponAttackingPoiseBonus()
    {
        character.CharacterStats.TotalPoiseDefense = character.CharacterStats.ArmorPoiseBonus;
    }
}
