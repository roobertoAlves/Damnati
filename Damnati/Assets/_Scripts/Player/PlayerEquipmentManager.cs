using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipmentManager : MonoBehaviour
{
    private PlayerManager _player;

    [Header("Defensive Items Status")]
    [Space(15)]

    [SerializeField] private float _helmetPhysicalDefense;
    [SerializeField] private float _legsPhysicalDefense;
    [SerializeField] private float _bodyPhysicalDefense;
    [SerializeField] private float _handsPhysicalDefense;
    private void Awake() 
    {
        _player = GetComponent<PlayerManager>();
    }

    private void Start() 
    {
        PhysicalDamageAbsorption();
    }

    private void PhysicalDamageAbsorption()
    {
        _player.PlayerStats.PhysicalDamageAbsorptionHead = _helmetPhysicalDefense;
        //Debug.Log("Head Absorption is: " + _player.PlayerStats.PhysicalDamageAbsorptionHead + "%");

        _player.PlayerStats.PhysicalDamageAbsorptionBody =_bodyPhysicalDefense;
        //Debug.Log("Body Absorption is: " + _player.PlayerStats.PhysicalDamageAbsorptionBody + "%");

        _player.PlayerStats.PhysicalDamageAbsorptionLegs = _legsPhysicalDefense;
        //Debug.Log("Legs Absorption is: " + _player.PlayerStats.PhysicalDamageAbsorptionLegs + "%");

        _player.PlayerStats.PhysicalDamageAbsorptionHands = _handsPhysicalDefense;
        //Debug.Log("Hands Absorption is: " + _player.PlayerStats.PhysicalDamageAbsorptionHands + "%");
    }
}
