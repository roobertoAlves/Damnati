using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    private CharacterManager _characterManager;
    private Collider _damageCollider;
    private bool _enabledDamageColliderOnStartUp = false;

    [Header("Poise")]
    [Space(15)]
    [SerializeField] private float _poiseBreak;
    [SerializeField] private float _offensivePoiseBonus;
    
    [Header("Damage")]
    [Space(15)]
    [SerializeField] private int _currentWeaponDamage = 25;

    #region GET & SET

    public int CurrentWeaponDamage { get { return _currentWeaponDamage; } set { _currentWeaponDamage = value; }}
    public CharacterManager characterManager { get { return _characterManager; } set { _characterManager = value; }}
    public bool EnabledDamageColliderOnStartUp { get { return _enabledDamageColliderOnStartUp; } set { _enabledDamageColliderOnStartUp = value; }}


    public float PoiseBreak { get { return _poiseBreak; } set { _poiseBreak = value; }}
    public float OffensivePoiseBonus { get { return _offensivePoiseBonus; } set { _offensivePoiseBonus = value; }}

    #endregion


    private void Awake() 
    {
        _damageCollider = GetComponent<Collider>();
        _damageCollider.gameObject.SetActive(true);
        _damageCollider.isTrigger = true;
        _damageCollider.enabled = _enabledDamageColliderOnStartUp;
    }

    public void EnableDamageCollider()
    {
        _damageCollider.enabled = true;
    }
    public void DisableDamageCollider()
    {
        _damageCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider collision) 
    {
        if(collision.tag == "Player")
        {
            PlayerStatsManager playerStatsManager = collision.GetComponent<PlayerStatsManager>();
            CharacterManager enemyCharacterManager = collision.GetComponent<CharacterManager>();
            BlockingCollider shield = collision.transform.GetComponentInChildren<BlockingCollider>();
           
            if(enemyCharacterManager != null)
            {
                if(enemyCharacterManager.IsParrying)
                {
                    _characterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Parried", true);
                    return;  
                }
                else if(shield != null && enemyCharacterManager.IsBlocking)
                {
                    float physicalDamageAfterBlock = _currentWeaponDamage - (_currentWeaponDamage * shield.BlockingPhysicalDamageAbsorption)/ 100;

                    if(playerStatsManager != null)
                    {
                        playerStatsManager.TakeDamage(Mathf.RoundToInt(physicalDamageAfterBlock), "Block Idle");
                        return;
                    }
                }
            }
            if (playerStatsManager != null)
            {
                playerStatsManager.PoiseResetTimer = playerStatsManager.TotalPoiseResetTime;
                playerStatsManager.TotalPoiseDefense = playerStatsManager.TotalPoiseDefense - _poiseBreak;
                Debug.Log("Player's Poise is currently " + playerStatsManager.TotalPoiseDefense);
                
                if(playerStatsManager.TotalPoiseDefense > _poiseBreak)
                {
                    playerStatsManager.TakeDamageNoAnimation(_currentWeaponDamage);
                }
                else
                {
                    playerStatsManager.TakeDamage(_currentWeaponDamage);
                }
            }
        }

        if(collision.tag == "Enemy")
        {
            EnemyStatsManager enemyStatsManager = collision.GetComponent<EnemyStatsManager>();
            CharacterManager enemyCharacterManager = collision.GetComponent<CharacterManager>();
            BlockingCollider shield = collision.transform.GetComponentInChildren<BlockingCollider>();
            

            if (enemyCharacterManager != null)
            {
                if (enemyCharacterManager.IsParrying)
                {
                    _characterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Parried", true);
                    return;
                }
                else if (shield != null && enemyCharacterManager.IsBlocking)
                {
                    float physicalDamageAfterBlock =
                    _currentWeaponDamage - (_currentWeaponDamage * shield.BlockingPhysicalDamageAbsorption) / 100;

                    if (enemyStatsManager != null)
                    {
                        enemyStatsManager.TakeDamage(Mathf.RoundToInt(physicalDamageAfterBlock), "Block Idle");
                        return;
                    }
                }
            }

            if (enemyStatsManager != null)
            {
                enemyStatsManager.PoiseResetTimer = enemyStatsManager.TotalPoiseResetTime;
                enemyStatsManager.TotalPoiseDefense = enemyStatsManager.TotalPoiseDefense - _poiseBreak;
                Debug.Log("Enemie's Poise is currently " + enemyStatsManager.TotalPoiseDefense);

                if(enemyStatsManager.IsBoss)
                {
                    if(enemyStatsManager.TotalPoiseDefense > _poiseBreak)
                    {
                        enemyStatsManager.TakeDamageNoAnimation(_currentWeaponDamage);
                    }
                    else
                    {
                        enemyStatsManager.TakeDamageNoAnimation(_currentWeaponDamage);
                        enemyStatsManager.BreakGuard();
                    }
                }
                else
                {
                    if(enemyStatsManager.TotalPoiseDefense > _poiseBreak)
                    {
                        enemyStatsManager.TakeDamageNoAnimation(_currentWeaponDamage);
                    }
                    else
                    {
                        enemyStatsManager.TakeDamage(_currentWeaponDamage);
                    }    
                }
            }
        }        
    }
}
