using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectsManager : CharacterEffectsManager
{
    private PlayerManager _player;

    [SerializeField] private GameObject _currentParticleFX;
    [SerializeField] private GameObject _instantiatedFXModel;

    protected override void Awake()
    {
        base.Awake();
        _player = GetComponent<PlayerManager>();
    }
}
