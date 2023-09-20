using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerCombatManager : MonoBehaviour
{
    private PlayerManager _player;

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

    
    private string oh_charge_attack_01 = "OH_Charge_Attack_01";
    private string oh_charge_attack_02 = "OH_Charge_Attack_02";

    private string th_charge_attack_01 = "TH_Charge_Attack_01";
    private string th_charge_attack_02 = "TH_Charge_Attack_02";

    private string weapon_art = "Weapon_Art";

    #endregion

    private LayerMask _riposteLayer = 1 << 9;

    private string _lastAttack;

    #region GET & SET
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

    private void Awake() 
    {
        _player = GetComponent<PlayerManager>();
    }
    
    public void AttemptRiposte()
    {
        if(_player.PlayerStats.CurrentStamina <= 0)
        {
            return; 
        }
        
        RaycastHit hit;

        if(Physics.Raycast(_player.PlayerLocomotion.CriticalAttackRayCastStartPoint.position, 
        transform.TransformDirection(Vector3.forward), out hit, 0.7f, _riposteLayer))
        {
            //Debug.Log("Step 1");
            CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
            //Debug.Log("Enemy Character Manager: " + enemyCharacterManager.transform.name);
            DamageCollider rightWeapon = _player.PlayerWeaponSlot.RightHandDamageCollider;
            //Debug.Log("Right Weapon Collider: " +  rightWeapon.transform.name);
           
            if(enemyCharacterManager != null && enemyCharacterManager.CanBeRiposted)
            {
                //Debug.Log("Step 2");
                _player.transform.position = enemyCharacterManager.RiposteDamageCollider.CriticalDamagerStandPosition.position;

                Vector3 rotationDirection = _player.transform.eulerAngles;
                rotationDirection = hit.transform.position - _player.transform.position;
                rotationDirection.y = 0;
                rotationDirection.Normalize();
                Quaternion tr = Quaternion.LookRotation(rotationDirection);
                Quaternion targetRotation = Quaternion.Slerp(_player.transform.rotation, tr, 500 * Time.deltaTime);
                _player.transform.rotation = targetRotation;

                int criticalDamage = _player.PlayerInventory.rightHandWeapon.criticalDamageMultiplier * rightWeapon.PhysicalDamage;
                enemyCharacterManager.PendingCriticalDamage = criticalDamage;

                _player.PlayerAnimator.PlayTargetAnimation("Riposte", true);
                enemyCharacterManager.GetComponentInChildren<CharacterAnimatorManager>().PlayTargetAnimation("Riposted", true);
            }
        }
    }
}
