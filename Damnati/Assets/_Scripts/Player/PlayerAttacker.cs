using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    private InputHandler _inputHandler;
    private PlayerAnimatorController _animator;
    private PlayerManager _playerManager;
    private PlayerStats _playerStats;
    private WeaponSlotManager _weaponSlotManager;
    private PlayerLocomotion _playerLocomotion;
    private PlayerInventory _playerInventory;

    private LayerMask _riposteLayer = 1 << 9;

    private string _lastAttack;
    public string LastAttack {get { return _lastAttack;} set { _lastAttack = value;}}
    private void Awake() 
    {
        _animator = GetComponent<PlayerAnimatorController>();
        _playerManager = GetComponent<PlayerManager>();
        _playerLocomotion = GetComponent<PlayerLocomotion>();
        _playerStats = GetComponent<PlayerStats>();
        _weaponSlotManager = GetComponent<WeaponSlotManager>();
        _inputHandler = FindObjectOfType<InputHandler>();
        _playerInventory = GetComponent<PlayerInventory>();
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

    #region Input Actions 

    public void HandleRBAction()
    {
        if (_playerInventory.rightHandWeapon.isMeleeWeapon)
        {
            PerformLBMeleeAction();
        }
    }
    public void HandleLBAction()
    {
        if(_playerInventory.rightHandWeapon.isMeleeWeapon)
        {
            PerformLBMeleeAction();
        }
    }

    public void HandleLTAction()
    {
        if (_playerInventory.leftHandWeapon.isShieldWeapon)
        {
            PerformLTWeaponArt(_inputHandler.THEquipFlag);
        }
        else if (_playerInventory.leftHandWeapon.isMeleeWeapon)
        {
            //do a light attack
        }
    }    

    #endregion

    #region Attack Actions
    private void PerformLBMeleeAction()
    {
        if (!_animator.HasAnimator)
        {
            return;
        }

        if (_playerManager.CanDoCombo)
        {
            _inputHandler.ComboFlag = true;
            HandleLightWeaponCombo(_playerInventory.rightHandWeapon);
            _inputHandler.ComboFlag = false;
        }
        else
        {
            if (_playerManager.IsInteracting || _playerManager.CanDoCombo)
            {
                return;
            }

            _animator.Anim.SetBool("IsUsingRightHand", true);
            HandleLightAttack(_playerInventory.rightHandWeapon);
        }
    }
    private void PerfomRBMeleeAction()
    {
        if (!_animator.HasAnimator)
        {
            return;
        }

        if (_playerManager.CanDoCombo)
        {
            _inputHandler.ComboFlag = true;
            HandleHeavyWeaponCombo(_playerInventory.rightHandWeapon);
            _inputHandler.ComboFlag = false;
        }
        else
        {
            if (_playerManager.IsInteracting || _playerManager.CanDoCombo)
            {
                return;
            }

            _animator.Anim.SetBool("IsUsingRightHand", true);
            HandleHeavyAttack(_playerInventory.rightHandWeapon); 
        }
    }
    private void PerformLTWeaponArt(bool isTwoHanding)
    {
        if(_playerManager.IsInteracting)
        {
            return;
        }

        if(isTwoHanding)
        {

        }
        else
        {
            _animator.PlayTargetAnimation(_playerInventory.leftHandWeapon.weapon_Art, true);
        }
    }
    public void AttemptRiposte()
    {
        if(_playerStats.CurrentStamina <= 0)
        {
            return; 
        }
        
        RaycastHit hit;

        if(Physics.Raycast(_playerLocomotion.CriticalAttackRayCastStartPoint.position, 
        transform.TransformDirection(Vector3.forward), out hit, 0.7f, _riposteLayer))
        {
            CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponent<CharacterManager>();
            DamageCollider rightWeapon = _weaponSlotManager.RightHandDamageCollider;
            _playerManager.transform.position = enemyCharacterManager.CriticalDamageCollider.CriticalDamagerStandPosition.position;

            Vector3 rotationDirection = _playerManager.transform.root.eulerAngles;
            rotationDirection = hit.transform.position - _playerManager.transform.position;
            rotationDirection.y = 0;
            rotationDirection.Normalize();
            Quaternion tr = Quaternion.LookRotation(rotationDirection);
            Quaternion targetRotation = Quaternion.Slerp(_playerManager.transform.rotation, tr, 500 * Time.deltaTime);
            _playerManager.transform.rotation = targetRotation;

            int criticalDamage = _playerInventory.rightHandWeapon.criticalDamageMultiplier * rightWeapon.CurrentWeaponDamage;
            enemyCharacterManager.PendingCriticalDamage = criticalDamage;

            _animator.PlayTargetAnimation("Riposte", true);
            enemyCharacterManager.GetComponent<AnimatorManager>().PlayTargetAnimation("Riposted", true);
        }
    }
    #endregion
}
