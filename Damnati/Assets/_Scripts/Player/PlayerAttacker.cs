using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    private InputHandler _inputHandler;
    private PlayerAnimatorController _animator;
    private PlayerManager _playerManager;
    private string _lastAttack;
    private PlayerStats _playerStats;
    private WeaponSlotManager _weaponSlotManager;
    private PlayerLocomotion _playerLocomotion;

    public string LastAttack {get { return _lastAttack;} set { _lastAttack = value;}}
    private void Awake() 
    {
        _animator = GetComponent<PlayerAnimatorController>();
        _playerManager = GetComponent<PlayerManager>();
        _playerLocomotion = GetComponent<PlayerLocomotion>();
        _playerStats = GetComponent<PlayerStats>();
        _weaponSlotManager = GetComponent<WeaponSlotManager>();
        _inputHandler = FindObjectOfType<InputHandler>();
    }

    public void HandleLightAttack(WeaponItem weapon)
    {
        _weaponSlotManager.attackingWeapon = weapon;
        
        if(_playerManager.TwoHandFlag)
        {
            _animator.PlayTargetAnimation(weapon.TH_Light_Slash_01, true);
            LastAttack = weapon.TH_Light_Slash_01;
        }
        else
        {
            _animator.PlayTargetAnimation(weapon.SS_Light_Slash_01, true);
            _lastAttack = weapon.SS_Light_Slash_01;
        }
    }
    public void HandleHeavyAttack(WeaponItem weapon)
    {

        _weaponSlotManager.attackingWeapon = weapon;
        if(_playerManager.TwoHandFlag)
        {
            _animator.PlayTargetAnimation(weapon.TH_Heavy_Slash_01, true);
            _lastAttack = weapon.TH_Heavy_Slash_01;
        }
        else if(!_playerManager.TwoHandFlag)
        {
            _animator.PlayTargetAnimation(weapon.SS_Heavy_Slash_01, true);
            _lastAttack = weapon.SS_Heavy_Slash_01;
        }
    }
    public void HandleLightWeaponCombo(WeaponItem weapon)
    {
        if(_animator.Anim.GetBool("IsInteracting") == true && _animator.Anim.GetBool("CanCombo") == false)
        {
            return;
        }

        if(_inputHandler.ComboFlag)
        {
            _animator.Anim.SetBool("CanCombo", false);
            
            #region Sword And Shield One Hand Attack

            if(_lastAttack == weapon.SS_Light_Slash_01)
            {
                _animator.PlayTargetAnimation(weapon.SS_Light_Slash_02, true);
                _lastAttack = weapon.SS_Light_Slash_02;
            }
            else if(_lastAttack == weapon.SS_Light_Slash_02)
            {
                _animator.PlayTargetAnimation(weapon.SS_Light_Slash_03, true);
                _lastAttack = weapon.SS_Light_Slash_03;
            }
            else if(_lastAttack == weapon.SS_Light_Slash_03 && !_playerManager.IsInteracting)
            {
                _playerManager.CanDoCombo = false;
                _lastAttack = "";
            }

            #endregion

            #region Sword And Shield Two Handed Attacks

            else if(_lastAttack == weapon.TH_Light_Slash_01)
            {
                _animator.PlayTargetAnimation(weapon.TH_Light_Slash_02, true);
                _lastAttack = weapon.TH_Light_Slash_02;
            }
            else if(_lastAttack == weapon.TH_Light_Slash_02)
            {
                _animator.PlayTargetAnimation(weapon.TH_Light_Slash_03, true);
                _lastAttack = weapon.TH_Light_Slash_03;
            }
            else if(_lastAttack == weapon.TH_Light_Slash_03 && !_playerManager.IsInteracting)
            {
                _playerManager.CanDoCombo = false;
                _lastAttack = "";
            }

            #endregion
        }
    }

    public void HandleHeavyWeaponCombo(WeaponItem weapon)
    {
        if(_animator.Anim.GetBool("IsInteracting") == true && _animator.Anim.GetBool("CanCombo") == false)
        {
            return;
        }

        if(_inputHandler.ComboFlag)
        {
            _animator.Anim.SetBool("CanCombo", false);
            
            #region Sword And Shield One Handed Attacks
            
            if(_lastAttack == weapon.SS_Heavy_Slash_01)
            {
                _animator.PlayTargetAnimation(weapon.SS_Heavy_Slash_02, true);
                _lastAttack = weapon.SS_Heavy_Slash_02;
            }
            else if(_lastAttack == weapon.SS_Heavy_Slash_02)
            {
                _animator.PlayTargetAnimation(weapon.SS_Heavy_Slash_03, true);
                _lastAttack = weapon.SS_Heavy_Slash_03;
            }
            else if(_lastAttack == weapon.SS_Heavy_Slash_03 && !_playerManager.IsInteracting)
            {
                _playerManager.CanDoCombo = false;
                _lastAttack = "";
            }

            #endregion

            #region Sword And Shield Two Handed Attacks

            else if(_lastAttack == weapon.TH_Heavy_Slash_01)
            {
                _animator.PlayTargetAnimation(weapon.TH_Heavy_Slash_02, true);
                _lastAttack = weapon.TH_Heavy_Slash_02;
            }
            else if(_lastAttack == weapon.TH_Heavy_Slash_02)
            {
                _animator.PlayTargetAnimation(weapon.TH_Heavy_Slash_03, true);
                _lastAttack = weapon.TH_Heavy_Slash_03;
            }
            else if(_lastAttack == weapon.TH_Heavy_Slash_03 && !_playerManager.IsInteracting)
            {
                _playerManager.CanDoCombo = false;
                _lastAttack = "";
            }

            #endregion
        }
    }
}
