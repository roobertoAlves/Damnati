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
        PlayerInventoryManager playerInventoryManager;
        PlayerLocomotionManager playerLocomotionManager;
        PlayerAnimatorManager playerAnimatorManager;

        playerInventoryManager = playerManager.GetComponent<PlayerInventoryManager>();
        playerLocomotionManager = playerManager.GetComponent<PlayerLocomotionManager>();
        playerAnimatorManager = playerManager.GetComponent<PlayerAnimatorManager>();

        //playerController.NewDirection = Vector3.zero; 
        playerAnimatorManager.PlayTargetAnimation("Picking Up", true);
        playerInventoryManager.WeaponsInventory.Add(weapon);
        playerManager.ItemInteractableGameObject.GetComponentInChildren<TextMeshProUGUI>().text = weapon.itemName;
        playerManager.ItemInteractableGameObject.GetComponentInChildren<RawImage>().texture = weapon.itemIcon.texture;
        playerManager.ItemInteractableGameObject.SetActive(true);
        Destroy(gameObject);
    }
}
