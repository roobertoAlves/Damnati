using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item Actions/Draw Arrow Action")]
public class DrawArrowAction : ItemActions
{
    public override void PerformAction(PlayerManager player)
    {
        if(player.IsInteracting || player.IsHoldingArrow)
        {
            return;
        }

        player.PlayerAnimator.Anim.SetBool("IsHoldingArrow", true);
        player.PlayerAnimator.PlayTargetAnimation("Draw Arrow", true);


        GameObject loadedArrow = Instantiate(player.PlayerInventory.currentAmmo.loadedItemModel, player.PlayerWeaponSlot.LeftHandSlot.transform);
        player.PlayerEffects.CurrentRangeFX = loadedArrow;


        Animator bowAnimator = player.PlayerWeaponSlot.RightHandSlot.GetComponentInChildren<Animator>();
        bowAnimator.SetBool("IsDrawn", true);
        bowAnimator.Play("Draw Arrow");
        
    }
}
