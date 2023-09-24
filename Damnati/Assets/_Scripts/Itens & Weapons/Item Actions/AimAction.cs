using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

[CreateAssetMenu(menuName = "Item Actions/Aim Action")]
public class AimAction : ItemActions
{
    public override void PerformAction(CharacterManager character)
    {
        PlayerManager player = character as PlayerManager;

        if(character.IsAiming)
        {
            return;
        }

        if(player != null)
        {
            player.PlayerInput.UIManager.CrossHair.SetActive(true);
        }
        character.IsAiming = true;
    }
}
