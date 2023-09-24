using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCombatManager : MonoBehaviour
{
    private CharacterManager _character;

    [Header("Attack Type")]
    [Space(15)]
    [SerializeField] private AttackType _currentAttackType;

    #region Attack Animations

    [Header("Attack Animations")]
    [Space(15)]
    [SerializeField] private string oh_light_attack_01 = "OH_Light_Attack_01";
    [SerializeField] private string oh_light_attack_02 = "OH_Light_Attack_02";
    [SerializeField] private string oh_light_attack_03 = "OH_Light_Attack_03";
    
    [SerializeField] private string oh_heavy_attack_01 = "OH_Heavy_Attack_01";
    [SerializeField] private string oh_heavy_attack_02 = "OH_Heavy_Attack_02";
    [SerializeField] private string oh_heavy_attack_03 = "OH_Heavy_Attack_02";

    [SerializeField] private string th_light_attack_01 = "TH_Light_Attack_01";
    [SerializeField] private string th_light_attack_02 = "TH_Light_Attack_02";
    [SerializeField] private string th_light_attack_03 = "TH_Light_Attack_03";

    [SerializeField] private string th_heavy_attack_01 = "TH_Heavy_Attack_01";
    [SerializeField] private string th_heavy_attack_02 = "TH_Heavy_Attack_02";
    [SerializeField] private string th_heavy_attack_03 = "TH_Heavy_Attack_03";

    [SerializeField] private string th_running_attack_01 = "TH_Running_Attack_01";
    
    [SerializeField] private string oh_running_attack_01 = "OH_Running_Attack_01";

    [SerializeField] private string th_jumping_attack_01 = "TH_Jumping_Attack_01";

    [SerializeField] private string oh_jumping_attack_01 = "OH_Jumping_Attack_01";

    
    [SerializeField] private string oh_charge_attack_01 = "OH_Charge_Attack_01";
    [SerializeField] private string oh_charge_attack_02 = "OH_Charge_Attack_02";

    [SerializeField]private string th_charge_attack_01 = "TH_Charge_Attack_01";
    [SerializeField] private string th_charge_attack_02 = "TH_Charge_Attack_02";

    [SerializeField] private string weapon_art = "Weapon_Art";

    #endregion

    private LayerMask _riposteLayer = 1 << 9;

    private string _lastAttack;

    #region GET & SET
    public AttackType CurrentAttackType { get { return _currentAttackType; } set { _currentAttackType = value; }}

    public string LastAttack {get { return _lastAttack; } set { _lastAttack = value; }}
    public string Weapon_Art { get { return weapon_art; }}
    
    public string OH_Light_Attack_01 { get { return oh_light_attack_01; }}
    public string OH_Light_Attack_02 { get { return oh_light_attack_02; }}
    public string OH_Light_Attack_03 { get { return oh_light_attack_03; }}

    public string OH_Heavy_Attack_01 { get { return oh_heavy_attack_01; }}
    public string OH_Heavy_Attack_02 { get { return oh_heavy_attack_02; }}
    public string OH_Heavy_Attack_03 { get { return oh_heavy_attack_03; }}

    public string OH_Running_Attack_01 { get { return oh_running_attack_01; }}
    public string OH_Jumping_Attack_01 { get { return oh_jumping_attack_01; }}

    public string TH_Light_Attack_01 { get { return th_light_attack_01; }}
    public string TH_Light_Attack_02 { get { return th_light_attack_02; }}
    public string TH_Light_Attack_03 { get { return th_light_attack_03; }}
    

    public string TH_Heavy_Attack_01 { get { return th_heavy_attack_01; }}
    public string TH_Heavy_Attack_02 { get { return th_heavy_attack_02; }}
    public string TH_Heavy_Attack_03 { get { return th_heavy_attack_03; }}
    
    public string TH_Running_Attack_01 { get { return th_running_attack_01; }}
    public string TH_Jumping_Attack_01 { get { return th_jumping_attack_01; }}

    public string OH_Charge_Attack_01 { get { return oh_charge_attack_01; }}
    public string OH_Charge_Attack_02 { get { return oh_charge_attack_02; }}

    public string TH_Charge_Attack_01 { get { return th_charge_attack_01; }}
    public string TH_Charge_Attack_02 { get { return th_charge_attack_02; }}
    
    #endregion

    protected virtual void Awake()
    {
        _character = GetComponent<CharacterManager>();
    }

    public virtual void SetBlockingAbsorptionsFromBlockingWeapon()
    {
        if(_character.IsUsingRightHand)
        {
            _character.CharacterStats.BlockingFireDamageAbsorption = _character.CharacterInventory.rightHandWeapon.physicalBlockingDamageAbsorption;
            _character.CharacterStats.BlockingFireDamageAbsorption = _character.CharacterInventory.rightHandWeapon.fireBlockingDamageAbsorption;
            _character.CharacterStats.BlockingStabilityRating = _character.CharacterInventory.rightHandWeapon.stability;
        }
        else if(_character.IsUsingLeftHand)
        {
            _character.CharacterStats.BlockingFireDamageAbsorption = _character.CharacterInventory.leftHandWeapon.physicalBlockingDamageAbsorption;
            _character.CharacterStats.BlockingFireDamageAbsorption = _character.CharacterInventory.leftHandWeapon.fireBlockingDamageAbsorption;
            _character.CharacterStats.BlockingStabilityRating = _character.CharacterInventory.leftHandWeapon.stability;
        }
    }
    public virtual void DrainStaminaBaseOnAttack()
    {

    }
    public virtual void DrainRageBaseOnAttack()
    {
        
    }
    public virtual void AttemptRiposte()
    {
        if(_character.CharacterStats.CurrentStamina <= 0)
        {
            return; 
        }
        
        RaycastHit hit;

        if(Physics.Raycast(_character.CriticalAttackRayCastStartPoint.position, 
        transform.TransformDirection(Vector3.forward), out hit, 0.7f, _riposteLayer))
        {
            //Debug.Log("Step 1");
            CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
            //Debug.Log("Enemy Character Manager: " + enemyCharacterManager.transform.name);
            DamageCollider rightWeapon = _character.CharacterWeaponSlot.RightHandDamageCollider;
            //Debug.Log("Right Weapon Collider: " +  rightWeapon.transform.name);
           
            if(enemyCharacterManager != null && enemyCharacterManager.CanBeRiposted)
            {
                //Debug.Log("Step 2");
                _character.transform.position = enemyCharacterManager.RiposteDamageCollider.CriticalDamagerStandPosition.position;

                Vector3 rotationDirection = _character.transform.eulerAngles;
                rotationDirection = hit.transform.position - _character.transform.position;
                rotationDirection.y = 0;
                rotationDirection.Normalize();
                Quaternion tr = Quaternion.LookRotation(rotationDirection);
                Quaternion targetRotation = Quaternion.Slerp(_character.transform.rotation, tr, 500 * Time.deltaTime);
                _character.transform.rotation = targetRotation;

                int criticalDamage = _character.CharacterInventory.rightHandWeapon.criticalDamageMultiplier * rightWeapon.PhysicalDamage;
                enemyCharacterManager.PendingCriticalDamage = criticalDamage;

                _character.CharacterAnimator.PlayTargetAnimation("Riposte", true);
                enemyCharacterManager.GetComponentInChildren<CharacterAnimatorManager>().PlayTargetAnimation("Riposted", true);
            }
        }
    }
    public virtual void AttemptBlock(DamageCollider attackingWeapon, float physicalDamage, float fireDamage, string blockingAnimation)
    {
        float staminaDamageAbsorption = ((physicalDamage + fireDamage) * attackingWeapon.GuardBreakModifier) 
            * (_character.CharacterStats.BlockingStabilityRating / 100);

        float staminaDamage = ((physicalDamage + fireDamage) * attackingWeapon.GuardBreakModifier) - staminaDamageAbsorption;

        _character.CharacterStats.CurrentStamina = _character.CharacterStats.CurrentStamina - staminaDamage;

        if(_character.CharacterStats.CurrentStamina <= 0)
        {
            _character.IsBlocking = false;
            _character.CharacterAnimator.PlayTargetAnimation("Guard Break 01", true);
        }
        else
        {
            _character.CharacterAnimator.PlayTargetAnimation(blockingAnimation, true);
        }
    }
}
