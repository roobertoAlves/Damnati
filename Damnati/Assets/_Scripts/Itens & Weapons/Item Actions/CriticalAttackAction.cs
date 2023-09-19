using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item Actions/Attempt Critical Attack Action")]
public class CriticalAttackAction : ItemActions
{
    public override void PerformAction(PlayerManager player)
    {
        if(player.IsInteracting)
        {
            return;
        }

        player.PlayerCombat.AttemptRiposte();
    }
}
