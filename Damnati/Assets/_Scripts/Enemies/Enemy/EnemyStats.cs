using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    private EnemyAnimatorController _enemyAnimatorController;
    private EnemyBossManager _eenemyBossManager;
    [SerializeField] private UIEnemyHealthBar _enemyHealthBar;

    [SerializeField] private bool _isBoss;

    #region  GET & SET
    public bool IsBoss { get { return _isBoss;}}
    #endregion
    private void Awake() 
    {
        _enemyAnimatorController  = GetComponent<EnemyAnimatorController>();
        _eenemyBossManager = GetComponent<EnemyBossManager>();
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
        _enemyHealthBar.SetHealth(CurrentHealth);
        
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
        else if(_isBoss && _eenemyBossManager != null)
        {
            _eenemyBossManager.UpdateBossHealthBar(CurrentHealth);
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
