using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    private CharacterManager _characterManager;
    protected Collider damageCollider;
    [SerializeField] private bool _enabledDamageColliderOnStartUp = false;

    [Header("Team I.D")]
    [Space(15)]
    [SerializeField] private int _teamIDNumber = 0;

    [Header("Poise")]
    [Space(15)]
    [SerializeField] private float _poiseBreak;
    [SerializeField] private float _offensivePoiseBonus;
    
    [Header("Damage")]
    [Space(15)]
    [SerializeField] private int _physicalDamage;
    [SerializeField] private int _fireDamage;

    [Header("Guard Break Multiplier")]
    [Space(15)]
    [SerializeField] private float _guardBreakModifier = 1;

    protected bool shieldHasBeenHit;
    protected bool hasBeenParried;
    protected string currentDamageAnimation;

    private List<CharacterManager> _characterDamagedDuringThisCalculation = new List<CharacterManager>();

    #region GET & SET
    public int TeamIDNumber { get { return _teamIDNumber; } set { _teamIDNumber = value; }}
    
    
    public int PhysicalDamage { get { return _physicalDamage; } set { _physicalDamage = value; }}
    public int FireDamage { get { return _fireDamage; } set { _fireDamage = value; }}
    
    public CharacterManager characterManager { get { return _characterManager; } set { _characterManager = value; }}
    
    public bool EnabledDamageColliderOnStartUp { get { return _enabledDamageColliderOnStartUp; } set { _enabledDamageColliderOnStartUp = value; }}

    public float PoiseBreak { get { return _poiseBreak; } set { _poiseBreak = value; }}
    public float OffensivePoiseBonus { get { return _offensivePoiseBonus; } set { _offensivePoiseBonus = value; }}
    
    public float GuardBreakModifier { get { return _guardBreakModifier; } set { _guardBreakModifier = value; }}
    #endregion


    protected virtual void Awake() 
    {
        damageCollider = GetComponent<Collider>();
        damageCollider.gameObject.SetActive(true);
        damageCollider.isTrigger = true;
        damageCollider.enabled = _enabledDamageColliderOnStartUp;
    }
    public void EnableDamageCollider()
    {
        _characterDamagedDuringThisCalculation = new List<CharacterManager>();
        damageCollider.enabled = true;
    }
    public void DisableDamageCollider()
    {
        if(_characterDamagedDuringThisCalculation.Count > 0)
        {
            _characterDamagedDuringThisCalculation.Clear();
        }
        damageCollider.enabled = false;
    }
    protected virtual void OnTriggerEnter(Collider collision) 
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Damageble Character"))
        {
            shieldHasBeenHit = false;
            hasBeenParried = false;
             
            CharacterManager enemyManager = collision.GetComponentInParent<CharacterManager>();

            if(enemyManager != null)
            {
                if(_characterDamagedDuringThisCalculation.Contains(enemyManager))
                {
                    return;
                }

                _characterDamagedDuringThisCalculation.Add(enemyManager);

                if(enemyManager.CharacterStats.TeamIDNumber == _teamIDNumber)
                {
                    return;
                }

                CheckForParry(enemyManager);
                CheckForBlock(enemyManager);
            }

            if(enemyManager.CharacterStats != null)
            {
                if(enemyManager.CharacterStats.TeamIDNumber == _teamIDNumber || hasBeenParried || shieldHasBeenHit)
                {
                    return;
                }

                enemyManager.CharacterStats.PoiseResetTimer = enemyManager.CharacterStats.TotalPoiseResetTime;
                enemyManager.CharacterStats.TotalPoiseDefense = enemyManager.CharacterStats.TotalPoiseDefense - _poiseBreak;

                //Detecta onde o colisor da arma fez o primeiro contato

                Vector3 contactPoint = collision.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
                float directionHitFrom = (Vector3.SignedAngle(characterManager.transform.forward, enemyManager.transform.forward, Vector3.up));
                ChooseWhichDirectionDamageCameFrom(directionHitFrom);
                enemyManager.CharacterEffects.PlayerBloodSplatterFX(contactPoint);
                enemyManager.CharacterEffects.InterruptEffect();

                
                DealDamage(enemyManager.CharacterStats);
            }
        }
    }
    protected virtual void CheckForParry(CharacterManager enemyManager)
    {
        if(enemyManager.IsParrying)
        {
            characterManager.GetComponentInChildren<CharacterAnimatorManager>().PlayTargetAnimation("Parried", true);
            hasBeenParried = true;
        }
    }
    protected virtual void CheckForBlock(CharacterManager enemyManager)
    {
        CharacterStatsManager enemyShield = enemyManager.CharacterStats;
        Vector3 directionFromPlayerToEnemy = (_characterManager.transform.position - enemyManager.transform.position);
        float dotValueFromPlayerToEnemy = Vector3.Dot(directionFromPlayerToEnemy, enemyManager.transform.forward);

        if(enemyManager.IsBlocking && dotValueFromPlayerToEnemy > 0.3f)
        {
            shieldHasBeenHit = true;
            float physicalDamageAfterBlock = _physicalDamage - (_physicalDamage * enemyShield.BlockingPhysicalDamageAbsorption) / 100;
            float fireDamageAfterBlock = _fireDamage - (_fireDamage * enemyShield.BlockingFireDamageAbsorption) / 100;

            enemyManager.CharacterCombat.AttemptBlock(this, _physicalDamage, _fireDamage, "Block_01");
            enemyShield.TakeDamageAfterBlock(Mathf.RoundToInt(physicalDamageAfterBlock), Mathf.RoundToInt(fireDamageAfterBlock) , _characterManager);
        }
    }
    protected virtual void DealDamage(CharacterStatsManager enemyStats)
    {
        float finalPhysicalDamage = PhysicalDamage;

        if(characterManager.IsUsingRightHand)
        {
            if(characterManager.CharacterCombat.CurrentAttackType == AttackType.LightAttack)
            {
                finalPhysicalDamage = characterManager.CharacterInventory.rightHandWeapon.lightAttackDamageModifier;
            }
            else if(characterManager.CharacterCombat.CurrentAttackType == AttackType.HeavyAttack)
            {
                finalPhysicalDamage = characterManager.CharacterInventory.rightHandWeapon.heavyAttackDamageModifier;
            }   
            else if(characterManager.CharacterCombat.CurrentAttackType == AttackType.JumpingHeavyAttack)
            {
                finalPhysicalDamage = characterManager.CharacterInventory.rightHandWeapon.jumpingAttackDamageModifier;
            }
            else if(characterManager.CharacterCombat.CurrentAttackType == AttackType.RunningLightAttack)
            {
                finalPhysicalDamage = characterManager.CharacterInventory.rightHandWeapon.runningAttackDamageModifier;
            }
        }
        else if(characterManager.IsUsingLeftHand)
        {
            if(characterManager.CharacterCombat.CurrentAttackType == AttackType.LightAttack)
            {
                finalPhysicalDamage = characterManager.CharacterInventory.leftHandWeapon.lightAttackDamageModifier;
            }
            else if(characterManager.CharacterCombat.CurrentAttackType == AttackType.HeavyAttack)
            {
                finalPhysicalDamage = characterManager.CharacterInventory.leftHandWeapon.heavyAttackDamageModifier;
            }   
            else if(characterManager.CharacterCombat.CurrentAttackType == AttackType.JumpingHeavyAttack)
            {
                finalPhysicalDamage = characterManager.CharacterInventory.leftHandWeapon.jumpingAttackDamageModifier;
            }
            else if(characterManager.CharacterCombat.CurrentAttackType == AttackType.RunningLightAttack)
            {
                finalPhysicalDamage = characterManager.CharacterInventory.leftHandWeapon.runningAttackDamageModifier;
            }
        }

        if(enemyStats.TotalPoiseDefense > _poiseBreak)
        {
            enemyStats.TakeDamageNoAnimation(Mathf.RoundToInt(finalPhysicalDamage), 0);
        }
        else
        {
           enemyStats.TakeDamage(Mathf.RoundToInt(finalPhysicalDamage), 0, currentDamageAnimation, _characterManager);
        }
    }
    protected virtual void ChooseWhichDirectionDamageCameFrom(float direction)
    {
        Debug.Log(direction);

        if(direction >= 145 && direction <= 180)
        {
            currentDamageAnimation = "Damage Forward";
        }
        else if(direction <= -145 && direction >= -180)
        {
            currentDamageAnimation = "Damage Forward";
        }
        else if(direction >= -45 && direction <= 45)
        {
            currentDamageAnimation = "Damage Back";
        }
        else if(direction >= -144 && direction <= -45)
        {
            currentDamageAnimation = "Damage Left";
        }
        else if(direction >= 45 && direction <= 144)
        {
            currentDamageAnimation = "Damage Right";
        }
    }
}
