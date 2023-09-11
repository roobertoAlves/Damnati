using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponSlotManager : CharacterWeaponSlotManager
{
    public override void GrantWeaponAttackingPoiseBonus()
    {
        characterStatsManager.TotalPoiseDefense = characterStatsManager.TotalPoiseDefense - characterStatsManager.OffensivePoiseBonus;
    }
    public override void ResetWeaponAttackingPoiseBonus()
    {
        characterStatsManager.TotalPoiseDefense = characterStatsManager.ArmorPoiseBonus;
    }
}
