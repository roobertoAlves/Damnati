using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipmentManager : MonoBehaviour
{
    private InputHandler _inputHandler;
    private PlayerInventoryManager _playerInverntory;
    private PlayerStatsManager _playerStats;

    [SerializeField] private BlockingCollider _blockingCollider;

    private void Awake() 
    {
        _inputHandler = FindObjectOfType<InputHandler>();
        _playerInverntory = GetComponent<PlayerInventoryManager>();  
        _playerStats = GetComponent<PlayerStatsManager>();  
    }

    private void Start() 
    {
        PhysicalDamageAbsorption();
    }

    private void PhysicalDamageAbsorption()
    {
        _playerStats.PhysicalDamageAbsorptionHead = _playerInverntory.HelmetPhysicalDefense;
        Debug.Log("Head Absorption is: " + _playerStats.PhysicalDamageAbsorptionHead + "%");

        _playerStats.PhysicalDamageAbsorptionBody = _playerInverntory.ChesplatePhysicalDefense;
        Debug.Log("Body Absorption is: " + _playerStats.PhysicalDamageAbsorptionBody + "%");

        _playerStats.PhysicalDamageAbsorptionLegs = _playerInverntory.LegsPhysicalDefense;
        Debug.Log("Legs Absorption is: " + _playerStats.PhysicalDamageAbsorptionLegs + "%");

        _playerStats.PhysicalDamageAbsorptionHands = _playerInverntory.GlovesPhysicalDefense;
        Debug.Log("Hands Absorption is: " + _playerStats.PhysicalDamageAbsorptionHands + "%");
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
