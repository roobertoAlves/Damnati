using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerCombatManager : MonoBehaviour
{
    private InputHandler _inputHandler;
    private CameraHandler _cameraHandler;
    private PlayerManager _playerManager;
    private PlayerAnimatorManager _playerAnimatorManager;
    private PlayerEquipmentManager _playerEquipmentManager;
    private PlayerStatsManager _playerStatsManager;
    private PlayerWeaponSlotManager _playerWeaponSlotManager;
    private PlayerLocomotionManager _playerLocomotionManger;
    private PlayerInventoryManager _playerInventoryManager;
    private PlayerEffectsManager _playerEffectsManager;

    #region Attack Animations

    [Header("Attack Animations")]
    [Space(15)]
    private string oh_light_attack_01 = "OH_Light_Attack_01";
    private string oh_light_attack_02 = "OH_Light_Attack_02";
    private string oh_light_attack_03 = "OH_Light_Attack_03";
    
    private string oh_heavy_attack_01 = "OH_Heavy_Attack_01";
    private string oh_heavy_attack_02 = "OH_Heavy_Attack_02";
    private string oh_heavy_attack_03 = "OH_Heavy_Attack_02";

    private string th_light_attack_01 = "TH_Light_Attack_01";
    private string th_light_attack_02 = "TH_Light_Attack_02";
    private string th_light_attack_03 = "TH_Light_Attack_03";

    private string th_heavy_attack_01 = "TH_Heavy_Attack_01";
    private string th_heavy_attack_02 = "TH_Heavy_Attack_02";
    private string th_heavy_attack_03 = "TH_Heavy_Attack_03";

    private string weapon_art = "Weapon_Art";

    #endregion

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
        _playerEffectsManager = GetComponent<PlayerEffectsManager>();
    }

    #region Input Actions 

    public void HandleLBAction()
    {
        _playerAnimatorManager.EraseHandIKForWeapon();

        if (_playerInventoryManager.rightHandWeapon.weaponType == WeaponType.StraightSword || _playerInventoryManager.rightHandWeapon.weaponType == WeaponType.Unarmed)
        {
            PerformLBMeleeAction();
        }
    }
    public void HandleRBAction()
    {
        _playerAnimatorManager.EraseHandIKForWeapon();

        if (_playerInventoryManager.rightHandWeapon.weaponType == WeaponType.StraightSword || _playerInventoryManager.rightHandWeapon.weaponType == WeaponType.Unarmed)
        {
            PerformRBMeleeAction();
        }
    }
    public void HandleLTAction()
    {
        if (_playerInventoryManager.leftHandWeapon.weaponType == WeaponType.Shield || _playerInventoryManager.rightHandWeapon.weaponType == WeaponType.Unarmed)
        {
            PerformLTWeaponArt(_inputHandler.THEquipFlag);
        }
        else if (_playerInventoryManager.leftHandWeapon.weaponType == WeaponType.StraightSword)
        {
            //do a light attack
        }
    }
    public void HandleDefenseAction()
    {
        PerfomLBBlockAction();
    }    

    #endregion

    public void HandleWeaponCombo(WeaponItem weapon)
    {
        if(_playerAnimatorManager.Anim.GetBool("IsInteracting") == true && _playerAnimatorManager.Anim.GetBool("CanDoCombo") == false)
        {
            return;
        }

        if(_inputHandler.ComboFlag)
        {
            _playerAnimatorManager.Anim.SetBool("CanDoCombo", false);

            if(_lastAttack == oh_light_attack_01)
            {
                _playerAnimatorManager.PlayTargetAnimation(oh_light_attack_02, true);
                _lastAttack = oh_light_attack_02;
            }
            else if(_lastAttack == oh_light_attack_02)
            {
                _playerAnimatorManager.PlayTargetAnimation(oh_light_attack_03, true);
                _lastAttack = oh_light_attack_03;
            }
            else if(_lastAttack == oh_heavy_attack_01)
            {
                _playerAnimatorManager.PlayTargetAnimation(oh_heavy_attack_02, true);
                _lastAttack = oh_heavy_attack_02;
            }
            else if(_lastAttack == oh_heavy_attack_02)
            {
                _playerAnimatorManager.PlayTargetAnimation(oh_heavy_attack_03, true);
                _lastAttack = oh_heavy_attack_03;
            }
            else if(_lastAttack == th_light_attack_01)
            {
                _playerAnimatorManager.PlayTargetAnimation(th_light_attack_02, true);
                _lastAttack = th_light_attack_02;
            }
            else if(_lastAttack == th_light_attack_02)
            {
                _playerAnimatorManager.PlayTargetAnimation(th_light_attack_03, true);
                _lastAttack = th_light_attack_03;
            }
            else if(_lastAttack == th_heavy_attack_01)
            {
                _playerAnimatorManager.PlayTargetAnimation(th_heavy_attack_02, true);
                _lastAttack = th_heavy_attack_02;
            }
            else if(_lastAttack == th_heavy_attack_02)
            {
                _playerAnimatorManager.PlayTargetAnimation(th_heavy_attack_03, true);
                _lastAttack = th_heavy_attack_03;
            }
        }
    }
    public void HandleLightAttack(WeaponItem weapon)
    {
        _playerWeaponSlotManager.AttackingWeapon = weapon;

        if(_inputHandler.TwoHandFlag)
        {
            _playerAnimatorManager.PlayTargetAnimation(th_light_attack_01, true);
            _lastAttack = th_light_attack_01;
        }
        else
        {
            _playerAnimatorManager.PlayTargetAnimation(oh_light_attack_01, true);
            _lastAttack = oh_light_attack_01;
        }
    }
    public void HandleHeavyAttack(WeaponItem weapon)
    {
        _playerWeaponSlotManager.AttackingWeapon = weapon;

        if(_inputHandler.TwoHandFlag)
        {
            _playerAnimatorManager.PlayTargetAnimation(th_heavy_attack_01, true);
            _lastAttack = th_heavy_attack_01;
        }
        else
        {
            _playerAnimatorManager.PlayTargetAnimation(oh_heavy_attack_01, true);
            _lastAttack = oh_heavy_attack_01;
        }
    }

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
            HandleWeaponCombo(_playerInventoryManager.rightHandWeapon);
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
        
        _playerEffectsManager.PlayWeaponFX(false);
    }
    private void PerformRBMeleeAction()
    {
        if (!_playerAnimatorManager.HasAnimator)
        {
            return;
        }

        if (_playerManager.CanDoCombo)
        {
            _inputHandler.ComboFlag = true;
            HandleWeaponCombo(_playerInventoryManager.rightHandWeapon);
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
        
        _playerEffectsManager.PlayWeaponFX(false);
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
            _playerAnimatorManager.PlayTargetAnimation(weapon_art, true);
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

                int criticalDamage = _playerInventoryManager.rightHandWeapon.criticalDamageMultiplier * rightWeapon.PhysicalDamage;
                enemyCharacterManager.PendingCriticalDamage = criticalDamage;

                _playerAnimatorManager.PlayTargetAnimation("Riposte", true);
                enemyCharacterManager.GetComponentInChildren<CharacterAnimatorManager>().PlayTargetAnimation("Riposted", true);
            }
        }
    }
}
