using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item Actions/Draw Arrow Action")]
public class DrawArrowAction : ItemActions
{
    public override void PerformAction(CharacterManager character)
    {
        if(character.IsInteracting || character.IsHoldingArrow)
        {
            return;
        }

        character.Animator.SetBool("IsHoldingArrow", true);
        character.CharacterAnimator.PlayTargetAnimation("Draw Arrow", true);

        GameObject loadedArrow = Instantiate(character.CharacterInventory.currentAmmo.loadedItemModel, character.CharacterWeaponSlot.LeftHandSlot.transform);
        character.CharacterEffects.CurrentRangeFX = loadedArrow;

        Animator bowAnimator = character.CharacterWeaponSlot.RightHandSlot.GetComponentInChildren<Animator>();
        bowAnimator.SetBool("IsDrawn", true);
        bowAnimator.Play("Draw Arrow");
        
    }
}
