using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item Actions/Parry Action")]
public class ParryAction : ItemActions
{
    public override void PerformAction(CharacterManager character)
    {
        if(character.IsInteracting)
        {
            return;
        }

        character.CharacterAnimator.EraseHandIKForWeapon();

        WeaponItem parryingWeapon = character.CharacterInventory.CurrentItemBeingUsed as WeaponItem;

        //checando se a arma realizando parry e uma rapida, media ou pesada

        if(parryingWeapon.weaponType == WeaponType.Shield)
        {
            character.CharacterAnimator.PlayTargetAnimation("Parry", true);
        }
    }
}
