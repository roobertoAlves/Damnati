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

    protected bool shieldHasBeenHit;
    protected bool hasBeenParried;
    protected string currentDamageAnimation;

    #region GET & SET
    public int TeamIDNumber { get { return _teamIDNumber; } set { _teamIDNumber = value; }}
    
    
    public int PhysicalDamage { get { return _physicalDamage; } set { _physicalDamage = value; }}
    public int FireDamage { get { return _fireDamage; } set { _fireDamage = value; }}
    
    public CharacterManager characterManager { get { return _characterManager; } set { _characterManager = value; }}
    
    public bool EnabledDamageColliderOnStartUp { get { return _enabledDamageColliderOnStartUp; } set { _enabledDamageColliderOnStartUp = value; }}

    public float PoiseBreak { get { return _poiseBreak; } set { _poiseBreak = value; }}
    public float OffensivePoiseBonus { get { return _offensivePoiseBonus; } set { _offensivePoiseBonus = value; }}

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
        damageCollider.enabled = true;
    }
    public void DisableDamageCollider()
    {
        damageCollider.enabled = false;
    }

    protected virtual void OnTriggerEnter(Collider collision) 
    {
        if(collision.tag == "Character")
        {
            shieldHasBeenHit = false;
            hasBeenParried = false;
             
            CharacterStatsManager enemyStats = collision.GetComponent<CharacterStatsManager>();
            CharacterManager enemyManager = collision.GetComponent<CharacterManager>();
            CharacterEffectsManager enemyEffects = collision.GetComponent<CharacterEffectsManager>();
            BlockingCollider shield = collision.transform.GetComponentInChildren<BlockingCollider>();
           
            if(enemyManager != null)
            {
                if(enemyStats.TeamIDNumber == _teamIDNumber)
                {
                    return;
                }

                CheckForParry(enemyManager);
                CheckForBlock(enemyManager, enemyStats, shield);
            }

            if(enemyStats != null)
            {
                if(enemyStats.TeamIDNumber == _teamIDNumber || hasBeenParried || shieldHasBeenHit)
                {
                    return;
                }

                enemyStats.PoiseResetTimer = enemyStats.TotalPoiseResetTime;
                enemyStats.TotalPoiseDefense = enemyStats.TotalPoiseDefense - _poiseBreak;

                //Detecta onde o colisor da arma fez o primeiro contato

                Vector3 contactPoint = collision.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
                float directionHitFrom = (Vector3.SignedAngle(characterManager.transform.forward, enemyManager.transform.forward, Vector3.up));
                ChooseWhichDirectionDamageCameFrom(directionHitFrom);
                enemyEffects.PlayerBloodSplatterFX(contactPoint);

                if(enemyStats.TotalPoiseDefense > _poiseBreak)
                {
                    enemyStats.TakeDamageNoAnimation(_physicalDamage, 0);
                }
                else
                {
                    enemyStats.TakeDamage(_physicalDamage, 0, currentDamageAnimation);
                }
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
    protected virtual void CheckForBlock(CharacterManager enemyManager, CharacterStatsManager enemyStats, BlockingCollider shield)
    {
        if(shield != null && enemyManager.IsBlocking)
        {
            float physicalDamageAfterBlock = _physicalDamage - (_physicalDamage * shield.BlockingPhysicalDamageAbsorption) / 100;
            float fireDamageAfterBlock = _fireDamage - (_fireDamage * shield.BlockingFireDamageAbsorption) / 100;
        
            if(enemyStats != null)
            {
                enemyStats.TakeDamage(Mathf.RoundToInt(physicalDamageAfterBlock), 0, "Block Idle");
                shieldHasBeenHit = true;
            }
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
