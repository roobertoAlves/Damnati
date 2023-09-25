using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCombatManager : MonoBehaviour
{
    private CharacterManager _character;

    [Header("Critical Attack Settings")]
    [Space(15)]
    [SerializeField] private LayerMask _characterLayer;
    [SerializeField] private float _criticalAttackRange = 0.7f;
    [SerializeField] private Transform _riposteReceiverTransform;


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
    [SerializeField] private int _pendingCriticalDamage;
    #endregion


    private string _lastAttack;

    #region GET & SET

    public LayerMask CharacterLayer  { get { return _characterLayer; }}
    public float CriticalAttackRange { get { return _criticalAttackRange; }}
    public Transform RiposteReceiverTransform { get { return _riposteReceiverTransform; } set { _riposteReceiverTransform = value; }}
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
    
    public int PendingCriticalDamage { get { return _pendingCriticalDamage; } set { _pendingCriticalDamage = value; }}
    #endregion

    protected virtual void Awake()
    {
        _character = GetComponent<CharacterManager>();
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
    private IEnumerator ForceMoveCharacterToEnemyRipostePosition(CharacterManager characterPerfomingRiposte)
    {
        for (float timer = 0.05f; timer < 0.5f; timer = timer + 0.05f)
        {
            Quaternion riposteRotation = Quaternion.LookRotation(characterPerfomingRiposte.transform.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, riposteRotation, 1);
            transform.parent = characterPerfomingRiposte.CharacterCombat.RiposteReceiverTransform;
            transform.localPosition = characterPerfomingRiposte.CharacterCombat.RiposteReceiverTransform.localPosition;
            transform.parent = null;
            Debug.Log("Running Coroutine");
            yield return new WaitForSeconds(0.05f);
        } 
    }
    public void GetRiposted(CharacterManager characterPerfomingRiposte)
    {
        _character.IsBeingRiposted = true;

        StartCoroutine(ForceMoveCharacterToEnemyRipostePosition(characterPerfomingRiposte));
        
        _character.CharacterAnimator.PlayTargetAnimation("Riposted", true);
    }
    public void AttemptRiposte()
    {
        if(_character.IsInteracting || _character.CharacterStats.CurrentStamina <= 0)
        {
            return;
        }

        RaycastHit hit;

        if(Physics.Raycast(_character.CriticalAttackRayCastStartPoint.transform.position, _character.transform.TransformDirection(Vector3.forward), out hit, _criticalAttackRange, _characterLayer))
        {
            CharacterManager enemyCharacter = hit.transform.GetComponent<CharacterManager>();
            Vector3 directionFromCharacterToEnemy = transform.position - enemyCharacter.transform.position;
            float dotValue = Vector3.Dot(directionFromCharacterToEnemy, enemyCharacter.transform.forward);

            Debug.Log("Current Dot value is: " + dotValue);
            
            if(enemyCharacter.CanBeRiposted)
            {
                if(dotValue <= 1.2f && dotValue >= 0.6f)
                {
                    Riposte(hit);
                    return;
                }
            }
        }
    }
    private void Riposte(RaycastHit hit)
    {
        CharacterManager enemyCharacter = hit.transform.GetComponent<CharacterManager>();
    
        if(enemyCharacter != null)
        {
            if(!enemyCharacter.IsBeingRiposted)
            {
                EnableIsInvulnerable();
                _character.IsPerformingRiposte = true;
                _character.CharacterAnimator.EraseHandIKForWeapon();

                _character.CharacterAnimator.PlayTargetAnimation("Riposte", true);

                float criticalDamage = (_character.CharacterInventory.rightHandWeapon.criticalDamageMultiplier *
                    (_character.CharacterInventory.rightHandWeapon.PhysicalDamage * 
                    _character.CharacterInventory.rightHandWeapon.FireDamage));
                
                int roundedCriticalDamage = Mathf.RoundToInt(criticalDamage);
                enemyCharacter.CharacterCombat._pendingCriticalDamage = roundedCriticalDamage;
                enemyCharacter.CharacterCombat.GetRiposted(_character);
            }
        }
    }
    private void EnableIsInvulnerable()
    {
        _character.Animator.SetBool("IsInvulnerable", true);
    }
    public void ApllyPendingDamage()
    {
        _character.CharacterStats.TakeDamageNoAnimation(_pendingCriticalDamage, 0);
    }
    public void EnableCanBeParried()
    {
        _character.CanBeParried = true;
    }
    public void DisableCanBeParried()
    {
        _character.CanBeParried = false;
    }
}
