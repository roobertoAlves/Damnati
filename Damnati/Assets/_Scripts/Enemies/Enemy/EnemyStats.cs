using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class EnemyStats : CharacterStats
{
    private EnemyAnimatorController _enemyAnimatorController;
    private EnemyBossManager _enemyBossManager;

    [SerializeField] private UIEnemyHealthBar _enemyHealthBar;

    [SerializeField] private bool _isBoss;

    #region  GET & SET
    public bool IsBoss { get { return _isBoss;}}
    #endregion
    private void Awake() 
    {
        _enemyAnimatorController  = GetComponent<EnemyAnimatorController>();
        _enemyBossManager = GetComponent<EnemyBossManager>();
        MaxHealth = SetMaxHealthFromHealthLevel();
        CurrentHealth = MaxHealth;
    }
    private void Start() 
    {
        if(!_isBoss)
        {
            _enemyHealthBar.SetMaxHealth(MaxHealth);
        }
    }

    private int SetMaxHealthFromHealthLevel()
    {
        MaxHealth = HealthLevel * 10;
        return MaxHealth;
    }

    public void TakeDamageNoAnimation(int damage)
    { 

        CurrentHealth = CurrentHealth - damage;
        if(!_isBoss)
        {
            _enemyHealthBar.SetHealth(CurrentHealth);
        }
        else if(_isBoss && _enemyBossManager != null)
        {
            _enemyBossManager.UpdateBossHealthBar(CurrentHealth, MaxHealth);
        }


        if(CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            IsDead = true;
        }
    }
    public override void TakeDamage(int damage, string damageAnimation = "Damage_01")
    {
        base.TakeDamage(damage, damageAnimation = "Damage_01");

        if(!_isBoss)
        {
            _enemyHealthBar.SetHealth(CurrentHealth);
        }
        else if(_isBoss && _enemyBossManager != null)
        {
            _enemyBossManager.UpdateBossHealthBar(CurrentHealth, MaxHealth);
        }

        _enemyAnimatorController.PlayTargetAnimation(damageAnimation, true);

        if(CurrentHealth <= 0)
        {
            HandleDeath();
        }
    }
    private void HandleDeath()
    {
        CurrentHealth = 0;
        _enemyAnimatorController.PlayTargetAnimation("Death_01", true);
        IsDead = true;
    }
}
