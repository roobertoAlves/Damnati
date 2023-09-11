using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    private CharacterManager _characterManager;
    protected Collider damageCollider;
    private bool _enabledDamageColliderOnStartUp = false;

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

    #region GET & SET
    public int TeamIDNumber { get { return _teamIDNumber; } set { _teamIDNumber = value; }}
    public int PhysicalDamage { get { return _physicalDamage; } set { _physicalDamage = value; }}
    public CharacterManager characterManager { get { return _characterManager; } set { _characterManager = value; }}
    public bool EnabledDamageColliderOnStartUp { get { return _enabledDamageColliderOnStartUp; } set { _enabledDamageColliderOnStartUp = value; }}


    public float PoiseBreak { get { return _poiseBreak; } set { _poiseBreak = value; }}
    public float OffensivePoiseBonus { get { return _offensivePoiseBonus; } set { _offensivePoiseBonus = value; }}

    #endregion


    private void Awake() 
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

    private void OnTriggerEnter(Collider collision) 
    {
        if(collision.tag == "Character")
        {
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

                if(enemyManager.IsParrying)
                {
                    characterManager.GetComponentInChildren<CharacterAnimatorManager>().PlayTargetAnimation("Parried", true);
                    return;
                }
                else if(shield != null && enemyManager.IsBlocking)
                {
                    float physicalDamageAfterBlock = _physicalDamage - (_physicalDamage * shield.BlockingPhysicalDamageAbsorption) / 100;

                    if(enemyStats != null)
                    {
                        enemyStats.TakeDamage(Mathf.RoundToInt(physicalDamageAfterBlock), 0 , "Block Idle");
                    }
                }
            }
            if(enemyStats != null)
            {
                if(enemyStats.TeamIDNumber == _teamIDNumber)
                {
                    return;
                }

                enemyStats.PoiseResetTimer = enemyStats.TotalPoiseResetTime;
                enemyStats.TotalPoiseDefense = enemyStats.TotalPoiseDefense - _poiseBreak;

                //DETECTS WHERE ON THE COLLIDER OUR WEAPON FIRST MAKES CONTACT
                Vector3 contactPoint = collision.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
                enemyEffects.PlayerBloodSplatterFX(contactPoint);

                if (enemyStats.TotalPoiseDefense > _poiseBreak)
                {
                    enemyStats.TakeDamageNoAnimation(_physicalDamage, 0);
                }
                else
                {
                    enemyStats.TakeDamage(_physicalDamage, 0);
                }
            }
        }
    }
}
