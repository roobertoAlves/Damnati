using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    private InputHandler _inputHandler;
    private AnimatorHandler _animator;
    private PlayerManager _playerManager;
    private string _lastAttack;
    private WeaponSlotManager _weaponSlotManager;

    public string LastAttack {get { return _lastAttack;} set { _lastAttack = value;}}
    private void Awake() 
    {
        _inputHandler = FindObjectOfType<InputHandler>();
        _animator = GetComponent<AnimatorHandler>();
        _playerManager = GetComponent<PlayerManager>();
        _weaponSlotManager = FindObjectOfType<WeaponSlotManager>();
    }

    public void HandleLightAttack(WeaponItem weapon)
    {
        _weaponSlotManager.attackingWeapon = weapon;
        if(_playerManager.TwoHandFlag)
        {
            _animator.PlayTargetAnimation(weapon.TH_Light_Slash_1, true);
            LastAttack = weapon.TH_Light_Slash_1;
        }
        else
        {
            _animator.PlayTargetAnimation(weapon.SS_Light_Slash_1, true);
            _lastAttack = weapon.SS_Light_Slash_1;
        }
    }
    public void HandleHeavyAttack(WeaponItem weapon)
    {
        _weaponSlotManager.attackingWeapon = weapon;
        if(_playerManager.TwoHandFlag)
        {

        }
        else
        {
            _animator.PlayTargetAnimation(weapon.SS_Heavy_Slash_1, true);
            _lastAttack = weapon.SS_Heavy_Slash_1;
        }
    }

    public void HandleWeaponCombo(WeaponItem weapon)
    {
        if(_lastAttack == weapon.SS_Light_Slash_1)
        {
            _animator.PlayTargetAnimation(weapon.SS_Light_Slash_2, true);
        }
        else if(_lastAttack == weapon.TH_Light_Slash_1)
        {
            _animator.PlayTargetAnimation(weapon.TH_Light_Slash_2, true);
        }
    }
}
