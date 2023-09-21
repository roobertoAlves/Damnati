using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item Actions/Blocking Action")]
public class BlockingAction : ItemActions
{
    public override void PerformAction(PlayerManager player)
    {
        if(player.IsInteracting || player.IsBlocking)
        {
            return;
        }

        player.PlayerCombat.SetBlockingAbsorptionsFromBlockingWeapon();

        player.IsBlocking = true;
    }
}
