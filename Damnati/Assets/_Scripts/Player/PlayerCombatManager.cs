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

    private string th_running_attack_01 = "TH_Running_Attack_01";
    
    private string oh_running_attack_01 = "OH_Running_Attack_01";

    private string th_jumping_attack_01 = "TH_Jumping_Attack_01";

    private string oh_jumping_attack_01 = "OH_Jumping_Attack_01";

    private string weapon_art = "Weapon_Art";

    #endregion

    private LayerMask _riposteLayer = 1 << 9;

    private string _lastAttack;

    #region GET & SET
    public string LastAttack {get { return _lastAttack;} set { _lastAttack = value;}}
   
    #endregion

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
    public void HandleArrowActions()
    {
        if(_playerManager.IsTwoHandingWeapon)
        {
            PerformRangedAction();
        }
    }
    public void HandleLBAction()
    {
        _playerAnimatorManager.EraseHandIKForWeapon();

        if (_playerInventoryManager.rightHandWeapon.weaponType == WeaponType.StraightSword 
            || _playerInventoryManager.rightHandWeapon.weaponType == WeaponType.Unarmed)
        {
            PerformLBMeleeAction();
        }
    }
    public void HandleRBAction()
    {
        _playerAnimatorManager.EraseHandIKForWeapon();

        if (_playerInventoryManager.rightHandWeapon.weaponType == WeaponType.StraightSword 
            || _playerInventoryManager.rightHandWeapon.weaponType == WeaponType.Unarmed)
        {
            PerformRBMeleeAction();
        }
    }
    public void HandleLTAction()
    {
        if (_playerInventoryManager.leftHandWeapon.weaponType == WeaponType.Shield 
            || _playerInventoryManager.rightHandWeapon.weaponType == WeaponType.Unarmed)
        {
            PerformLTWeaponArt(_inputHandler.THEquipFlag);
        }
        else if (_playerInventoryManager.leftHandWeapon.weaponType == WeaponType.StraightSword)
        {
            //do a light attack
        }
    }
    public void HandleRTAction()
    {
        if(_playerManager.IsTwoHandingWeapon)
        {
            if(_playerInventoryManager.rightHandWeapon.weaponType == WeaponType.Bow)
            {
                PerformRBAimingAction();
            }
            else
            {
                if(_playerInventoryManager.leftHandWeapon.weaponType == WeaponType.Shield ||
                    _playerInventoryManager.leftHandWeapon.weaponType == WeaponType.StraightSword)
                    {
                        PerfomLBBlockAction();
                    }
            }
        }
    }    

    #endregion

    public void HandleLightWeaponCombo(WeaponItem weapon)
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
        }
    }
    public void HandleHeavyWeaponCombo(WeaponItem weapon)
    {
        if(_playerAnimatorManager.Anim.GetBool("IsInteracting") == true && _playerAnimatorManager.Anim.GetBool("CanDoCombo") == false)
        {
            return;
        }

        if(_inputHandler.ComboFlag)
        {
            _playerAnimatorManager.Anim.SetBool("CanDoCombo", false);

            if(_lastAttack == oh_heavy_attack_01)
            {
                _playerAnimatorManager.PlayTargetAnimation(oh_heavy_attack_02, true);
                _lastAttack = oh_heavy_attack_02;
            }
            else if(_lastAttack == oh_heavy_attack_02)
            {
                _playerAnimatorManager.PlayTargetAnimation(oh_heavy_attack_03, true);
                _lastAttack = oh_heavy_attack_03;
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
    private void HandleRunningAttack(WeaponItem weapon)
    {
        _playerWeaponSlotManager.AttackingWeapon = weapon;

        if(_inputHandler.TwoHandFlag)
        {
            _playerAnimatorManager.PlayTargetAnimation(th_running_attack_01, true);
            _lastAttack = th_running_attack_01;
        }
        else
        {
            _playerAnimatorManager.PlayTargetAnimation(oh_running_attack_01, true);
            _lastAttack = oh_running_attack_01;
        }
    }
    private void HandleJumpingAttack(WeaponItem weapon)
    {
        _playerWeaponSlotManager.AttackingWeapon = weapon;

        if(_inputHandler.TwoHandFlag)
        {
            _playerAnimatorManager.PlayTargetAnimation(th_jumping_attack_01, true);
            _lastAttack = th_jumping_attack_01;
        }
        else
        {
            _playerAnimatorManager.PlayTargetAnimation(oh_jumping_attack_01, true);
            _lastAttack = oh_jumping_attack_01;
        }
    }
    #region Attack Actions
    private void PerformLBMeleeAction()
    {
        if (!_playerAnimatorManager.HasAnimator)
        {
            return;
        }
        
        _playerAnimatorManager.Anim.SetBool("IsUsingRightHand", true);

        if(_playerManager.IsSprinting)
        {
            HandleRunningAttack(_playerInventoryManager.rightHandWeapon);
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

        if(_playerManager.IsSprinting)
        {
            HandleJumpingAttack(_playerInventoryManager.rightHandWeapon);
            return;
        }

        _playerAnimatorManager.Anim.SetBool("IsUsingRightHand", true);

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
    
    #region Bow and Arrow Actions

    private void DrawArrowAction()
    {
        _playerAnimatorManager.PlayTargetAnimation("Draw Arrow", true);
        _playerAnimatorManager.Anim.SetBool("IsHoldingArrow", true);
        GameObject loadedArrow = Instantiate(_playerInventoryManager.currentAmmo.loadedItemModel, _playerWeaponSlotManager.LeftHandSlot.transform);
        Animator bowAnimator = _playerWeaponSlotManager.RightHandSlot.GetComponentInChildren<Animator>();
        bowAnimator.SetBool("IsDrawn", true);
        bowAnimator.Play("Draw Arrow");
        _playerEffectsManager.CurrentRangeFX = loadedArrow;
    }
    public void FireArrowAction()
    {
        ArrowInstantiationLocation arrowInstantiationLocation;
        arrowInstantiationLocation = _playerWeaponSlotManager.RightHandSlot.GetComponentInChildren<ArrowInstantiationLocation>();
        Debug.Log(arrowInstantiationLocation.transform.parent.name);

        Animator bowAnimator = _playerWeaponSlotManager.RightHandSlot.GetComponentInChildren<Animator>();
        bowAnimator.SetBool("IsDrawn", false);
        bowAnimator.Play("Bow Fire");
        Destroy(_playerEffectsManager.CurrentRangeFX);

        _playerAnimatorManager.PlayTargetAnimation("Bow Fire", true);
        _playerAnimatorManager.Anim.SetBool("IsHoldingArrow", false);

        GameObject liveArrow = Instantiate(_playerInventoryManager.currentAmmo.liveAmmoModel, arrowInstantiationLocation.transform.position, _cameraHandler.CameraPivotTransform.rotation);
        Rigidbody rb = liveArrow.GetComponentInChildren<Rigidbody>();
        RangedProjectileDamageCollider damageCollider = liveArrow.GetComponentInChildren<RangedProjectileDamageCollider>();
        Debug.Log(damageCollider.transform.parent.name);
        
        if(_playerManager.IsAiming)
        {
            Ray ray = _cameraHandler.CameraObject.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hitPoint;

            if(Physics.Raycast(ray, out hitPoint, 100.0f))
            {
                liveArrow.transform.LookAt(hitPoint.point);
                Debug.Log(hitPoint.transform.name);
            }
            else
            {
                liveArrow.transform.rotation = Quaternion.Euler(_cameraHandler.CameraTransform.localEulerAngles.x, _playerManager.LockOnTransform.eulerAngles.y, 0);
            }
        }
        else
        {
            if(_cameraHandler.CurrentLockOnTarget != null)
            {
                Quaternion arrowRotation = Quaternion.LookRotation(_cameraHandler.CurrentLockOnTarget.LockOnTransform.position - liveArrow.gameObject.transform.position);
                liveArrow.transform.rotation = arrowRotation;
            }
            else
            {
                liveArrow.transform.rotation = Quaternion.Euler(_cameraHandler.CameraPivotTransform.eulerAngles.x, _playerManager.LockOnTransform.eulerAngles.y, 0);
            }
        }
            rb.AddForce(liveArrow.transform.forward * _playerInventoryManager.currentAmmo.forwardVelocity);
            rb.AddForce(liveArrow.transform.up * _playerInventoryManager.currentAmmo.upwardVelocity);
            rb.useGravity = _playerInventoryManager.currentAmmo.useGravity;
            rb.mass = _playerInventoryManager.currentAmmo.ammoMass;
            liveArrow.transform.parent = null;

            //SET LIVE ARROW DAMAGE
            damageCollider.characterManager = _playerManager;
            damageCollider.AmmoItem = _playerInventoryManager.currentAmmo;
            damageCollider.PhysicalDamage = _playerInventoryManager.currentAmmo.physicalDamage;
    
    }
    private void PerformRangedAction()
    {
        _playerAnimatorManager.EraseHandIKForWeapon();
        _playerAnimatorManager.Anim.SetBool("IsUsingRightHand", true);

        if(!_playerManager.IsHoldingArrow)
        {
            if(_playerInventoryManager.currentAmmo != null)
            {
                DrawArrowAction();
            }
            else
            {
                _playerAnimatorManager.PlayTargetAnimation("Shrug", true);
            }
        }
    }
    private void PerformRBAimingAction()
    {
        if(_playerManager.IsAiming)
        {
            return;
        }

        _inputHandler.UIManager.CrossHair.SetActive(true);
        _playerManager.IsAiming = true;
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
