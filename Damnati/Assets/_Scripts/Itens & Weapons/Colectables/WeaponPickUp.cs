using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class WeaponPickUp : Interactable
{
    public WeaponItem weapon;

    public override void Interact(PlayerManager playerManager)
    {
        base.Interact(playerManager);

        PickUpItem(playerManager);
    }

    private void PickUpItem(PlayerManager playerManager)
    {
        PlayerInventory playerInventory;
        PlayerLocomotion playerController;
        PlayerAnimatorController animatorController;

        playerInventory = playerManager.GetComponent<PlayerInventory>();
        playerController = playerManager.GetComponent<PlayerLocomotion>();
        animatorController = playerManager.GetComponent<PlayerAnimatorController>();

        //playerController.NewDirection = Vector3.zero; 
        animatorController.PlayTargetAnimation("Picking Up", true);
        playerInventory.WeaponsInventory.Add(weapon);
        playerManager.ItemInteractableGameObject.GetComponentInChildren<TextMeshProUGUI>().text = weapon.itemName;
        playerManager.ItemInteractableGameObject.GetComponentInChildren<RawImage>().texture = weapon.itemIcon.texture;
        playerManager.ItemInteractableGameObject.SetActive(true);
        Destroy(gameObject);
    }
}
