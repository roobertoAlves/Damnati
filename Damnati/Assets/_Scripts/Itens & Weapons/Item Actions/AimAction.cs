using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item Actions/Aim Action")]
public class AimAction : ItemActions
{
    public override void PerformAction(PlayerManager player)
    {
        if(player.IsAiming)
        {
            return;
        }

        player.PlayerInput.UIManager.CrossHair.SetActive(true);
        player.IsAiming = true;
    }
}
