using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    public int damage = 25;
    private void OnTriggerEnter(Collider other) 
    {
        PlayerStatsManager _playerStatsManager = other.GetComponent<PlayerStatsManager>();

        if(_playerStatsManager != null)
        {
            _playerStatsManager.TakeDamage(damage);
        }
    }
}
