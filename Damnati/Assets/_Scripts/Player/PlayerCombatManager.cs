using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerCombatManager : MonoBehaviour
{
    private InputHandler _inputHandler;
    private PlayerAnimatorManager _playerAnimatorManager;
    private PlayerEquipmentManager _playerEquipmentManager;
    private PlayerManager _playerManager;
    private PlayerStatsManager _playerStatsManager;
    private PlayerWeaponSlotManager _playerWeaponSlotManager;
    private PlayerLocomotionManager _playerLocomotionManger;
    private PlayerInventoryManager _playerInventoryManager;
    private CameraHandler _cameraHandler;

    private LayerMask _riposteLayer = 1 << 9;

    private string _lastAttack;
    public string LastAttack {get { return _lastAttack;} set { _lastAttack = value;}}
    private void Awake() 
    {
        _cameraHandler = FindObjectOfType<CameraHandler>();
        _playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        _playerManager = GetComponent<PlayerManager>();
        _playerLocomotionManger = GetComponent<PlayerLocomotionManager>();
        _playerStatsManager = GetComponent<PlayerStatsManager>();
        _playerWeaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
        _inputHandler = FindObjectOfType<InputHandler>();
        _playerInventoryManager = GetComponent<PlayerInventoryManager>();
        _playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
    }

    public void HandleLightAttack(WeaponItem weapon)
    {
        _playerWeaponSlotManager.attackingWeapon = weapon;
        
        if(_playerManager.TwoHandFlag)
        {
            _playerAnimatorManager.PlayTargetAnimation(weapon.TH_Light_Slash_01, true);
            LastAttack = weapon.TH_Light_Slash_01;
        }
        else
        {
            _playerAnimatorManager.PlayTargetAnimation(weapon.SS_Light_Slash_01, true);
            _lastAttack = weapon.SS_Light_Slash_01;
        }
    }
    public void HandleHeavyAttack(WeaponItem weapon)
    {
        _playerWeaponSlotManager.attackingWeapon = weapon;

        if(_playerManager.TwoHandFlag)
        {
            _playerAnimatorManager.PlayTargetAnimation(weapon.TH_Heavy_Slash_01, true);
            _lastAttack = weapon.TH_Heavy_Slash_01;
        }
        else if(!_playerManager.TwoHandFlag)
        {
            _playerAnimatorManager.PlayTargetAnimation(weapon.SS_Heavy_Slash_01, true);
            _lastAttack = weapon.SS_Heavy_Slash_01;
        }
    }
    public void HandleLightWeaponCombo(WeaponItem weapon)
    {
        if(_playerAnimatorManager.Anim.GetBool("IsInteracting") == true && _playerAnimatorManager.Anim.GetBool("CanCombo") == false)
        {
            return;
        }

        if(_inputHandler.ComboFlag)
        {
            _playerAnimatorManager.Anim.SetBool("CanCombo", false);
            
            #region Sword And Shield One Hand Attack

            if(_lastAttack == weapon.SS_Light_Slash_01)
            {
                _playerAnimatorManager.PlayTargetAnimation(weapon.SS_Light_Slash_02, true);
                _lastAttack = weapon.SS_Light_Slash_02;
            }
            else if(_lastAttack == weapon.SS_Light_Slash_02)
            {
                _playerAnimatorManager.PlayTargetAnimation(weapon.SS_Light_Slash_03, true);
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
                _playerAnimatorManager.PlayTargetAnimation(weapon.TH_Light_Slash_02, true);
                _lastAttack = weapon.TH_Light_Slash_02;
            }
            else if(_lastAttack == weapon.TH_Light_Slash_02)
            {
                _playerAnimatorManager.PlayTargetAnimation(weapon.TH_Light_Slash_03, true);
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
        if(_playerAnimatorManager.Anim.GetBool("IsInteracting") == true && _playerAnimatorManager.Anim.GetBool("CanCombo") == false)
        {
            return;
        }

        if(_inputHandler.ComboFlag)
        {
            _playerAnimatorManager.Anim.SetBool("CanCombo", false);
            
            #region Sword And Shield One Handed Attacks
            
            if(_lastAttack == weapon.SS_Heavy_Slash_01)
            {
                _playerAnimatorManager.PlayTargetAnimation(weapon.SS_Heavy_Slash_02, true);
                _lastAttack = weapon.SS_Heavy_Slash_02;
            }
            else if(_lastAttack == weapon.SS_Heavy_Slash_02)
            {
                _playerAnimatorManager.PlayTargetAnimation(weapon.SS_Heavy_Slash_03, true);
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
                _playerAnimatorManager.PlayTargetAnimation(weapon.TH_Heavy_Slash_02, true);
                _lastAttack = weapon.TH_Heavy_Slash_02;
            }
            else if(_lastAttack == weapon.TH_Heavy_Slash_02)
            {
                _playerAnimatorManager.PlayTargetAnimation(weapon.TH_Heavy_Slash_03, true);
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
        if (_playerInventoryManager.rightHandWeapon.isMeleeWeapon)
        {
            PerformLBMeleeAction();
        }
    }
    public void HandleLBAction()
    {
        if(_playerInventoryManager.rightHandWeapon.isMeleeWeapon)
        {
            PerformLBMeleeAction();
        }
    }

    public void HandleLTAction()
    {
        if (_playerInventoryManager.leftHandWeapon.isShieldWeapon)
        {
            PerformLTWeaponArt(_inputHandler.THEquipFlag);
        }
        else if (_playerInventoryManager.leftHandWeapon.isMeleeWeapon)
        {
            //do a light attack
        }
    }
    public void HandleDefenseAction()
    {
        PerfomLBBlockAction();
    }    

    #endregion

    #region Attack Actions
    private void PerformLBMeleeAction()
    {
        if (!_playerAnimatorManager.HasAnimator)
        {
            return;
        }

        if (_playerManager.CanDoCombo)
        {
            _inputHandler.ComboFlag = true;
            HandleLightWeaponCombo(_playerInventoryManager.rightHandWeapon);
            _inputHandler.ComboFlag = false;
        }
        else
        {
            if (_playerManager.IsInteracting || _playerManager.CanDoCombo)
            {
                return;
            }

            _playerAnimatorManager.Anim.SetBool("IsUsingRightHand", true);
            HandleLightAttack(_playerInventoryManager.rightHandWeapon);
        }
    }
    private void PerfomRBMeleeAction()
    {
        if (!_playerAnimatorManager.HasAnimator)
        {
            return;
        }

        if (_playerManager.CanDoCombo)
        {
            _inputHandler.ComboFlag = true;
            HandleHeavyWeaponCombo(_playerInventoryManager.rightHandWeapon);
            _inputHandler.ComboFlag = false;
        }
        else
        {
            if (_playerManager.IsInteracting || _playerManager.CanDoCombo)
            {
                return;
            }

            _playerAnimatorManager.Anim.SetBool("IsUsingRightHand", true);
            HandleHeavyAttack(_playerInventoryManager.rightHandWeapon); 
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
            _playerAnimatorManager.PlayTargetAnimation(_playerInventoryManager.leftHandWeapon.weapon_Art, true);
        }
    }
    #endregion
    
    #region Defensive Actions 
    private void PerfomLBBlockAction()
    {
        if(_playerManager.IsInteracting || _playerManager.IsBlocking)
        {
            return;
        }

        _playerAnimatorManager.PlayTargetAnimation("Block Start", false, true);
        _playerEquipmentManager.OpenBlockingCollider();
        _playerManager.IsBlocking = true;
    }
    #endregion
    
    public void AttemptRiposte()
    {
        if(_playerStatsManager.CurrentStamina <= 0)
        {
            return; 
        }
        
        RaycastHit hit;

        if(Physics.Raycast(_playerLocomotionManger.CriticalAttackRayCastStartPoint.position, 
        transform.TransformDirection(Vector3.forward), out hit, 0.7f, _riposteLayer))
        {
            //Debug.Log("Step 1");
            CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
            //Debug.Log("Enemy Character Manager: " + enemyCharacterManager.transform.name);
            DamageCollider rightWeapon = _playerWeaponSlotManager.RightHandDamageCollider;
            //Debug.Log("Right Weapon Collider: " +  rightWeapon.transform.name);
           
            if(enemyCharacterManager != null && enemyCharacterManager.CanBeRiposted)
            {
                //Debug.Log("Step 2");
                _playerManager.transform.position = enemyCharacterManager.CriticalDamageCollider.CriticalDamagerStandPosition.position;

                Vector3 rotationDirection = _playerManager.transform.eulerAngles;
                rotationDirection = hit.transform.position - _playerManager.transform.position;
                rotationDirection.y = 0;
                rotationDirection.Normalize();
                Quaternion tr = Quaternion.LookRotation(rotationDirection);
                Quaternion targetRotation = Quaternion.Slerp(_playerManager.transform.rotation, tr, 500 * Time.deltaTime);
                _playerManager.transform.rotation = targetRotation;

                int criticalDamage = _playerInventoryManager.rightHandWeapon.criticalDamageMultiplier * rightWeapon.CurrentWeaponDamage;
                enemyCharacterManager.PendingCriticalDamage = criticalDamage;

                _playerAnimatorManager.PlayTargetAnimation("Riposte", true);
                enemyCharacterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Riposted", true);
            }
        }
    }
}
