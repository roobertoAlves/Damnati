using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item Actions/Parry Action")]
public class ParryAction : ItemActions
{
    public override void PerformAction(PlayerManager player)
    {
        if(player.IsInteracting)
        {
            return;
        }

        player.PlayerAnimator.EraseHandIKForWeapon();

        WeaponItem parryingWeapon = player.PlayerInventory.CurrentItemBeingUsed as WeaponItem;

        //checando se a arma realizando parry e uma rapida, media ou pesada

        if(parryingWeapon.weaponType == WeaponType.Shield)
        {
            player.PlayerAnimator.PlayTargetAnimation("Parry", true);
        }
    }
}
