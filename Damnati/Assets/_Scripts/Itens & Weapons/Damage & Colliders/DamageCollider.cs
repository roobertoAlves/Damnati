using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    private Collider _damageCollider;

    [SerializeField] private int _currentWeaponDamage = 25;


    private void Awake() 
    {
        _damageCollider = GetComponent<Collider>();
        _damageCollider.gameObject.SetActive(true);
        _damageCollider.isTrigger = true;
        _damageCollider.enabled = false;
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
            PlayerStats playerStats = collision.GetComponent<PlayerStats>();
                
            if (playerStats != null)
            {
                playerStats.TakeDamage(_currentWeaponDamage);
            }
        }

        if(collision.tag == "Enemy")
        {
            EnemyStats enemyStats = collision.GetComponent<EnemyStats>();

            if(enemyStats != null)
            {
                enemyStats.TakeDamage(_currentWeaponDamage);
            }
        }        
    }
}
