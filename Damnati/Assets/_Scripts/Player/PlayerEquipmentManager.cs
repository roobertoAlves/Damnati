using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipmentManager : MonoBehaviour
{
    private InputHandler _inputHandler;
    private PlayerInventory _playerInverntory;

    [SerializeField] private BlockingCollider _blockingCollider;

    private void Awake() 
    {
        _inputHandler = FindObjectOfType<InputHandler>();
        _playerInverntory = GetComponent<PlayerInventory>();    
    }

    public void OpenBlockingCollider()
    {
        if(_inputHandler.THEquipFlag)
        {
            _blockingCollider.SetColliderDamageAbsorption(_playerInverntory.rightHandWeapon);
        }
        else
        {
            _blockingCollider.SetColliderDamageAbsorption(_playerInverntory.leftHandWeapon);
        }
        _blockingCollider.EnableBlockingCollider();
    }

    public void CloseBlockingCollider()
    {
        _blockingCollider.DisableBlockingCollider();
    }
}
