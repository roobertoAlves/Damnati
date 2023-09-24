using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item Actions/Blocking Action")]
public class BlockingAction : ItemActions
{
    public override void PerformAction(CharacterManager character)
    {
        if(character.IsInteracting || character.IsBlocking)
        {
            return;
        }

        character.CharacterCombat.SetBlockingAbsorptionsFromBlockingWeapon();

        character.IsBlocking = true;
    }
}
