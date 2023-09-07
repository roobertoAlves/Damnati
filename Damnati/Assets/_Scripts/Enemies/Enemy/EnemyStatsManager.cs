using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class EnemyStatsManager : CharacterStatsManager
{
    private EnemyAnimatorManager _enemyAnimatorManager;
    private EnemyManager _enemyManager;
    private EnemyBossManager _enemyBossManager;

    [SerializeField] private UIEnemyHealthBar _enemyHealthBar;

    [SerializeField] private bool _isBoss;

    #region  GET & SET
    public bool IsBoss { get { return _isBoss;}}
    #endregion
    private void Awake() 
    {
        _enemyAnimatorManager  = GetComponent<EnemyAnimatorManager>();
        _enemyBossManager = GetComponent<EnemyBossManager>();
        MaxHealth = SetMaxHealthFromHealthLevel();
        _enemyManager = GetComponent<EnemyManager>();
        CurrentHealth = MaxHealth;
    }
    private void Start() 
    {
        if(!_isBoss)
        {
            _enemyHealthBar.SetMaxHealth(MaxHealth);
        }
    }
    public override void HandlePoiseResetTimer()
    {
        if(PoiseResetTimer > 0)
        {
            PoiseResetTimer = PoiseResetTimer - Time.deltaTime;
        }
        else if(PoiseResetTimer <= 0 && !_enemyManager.IsInteracting)
        {
            TotalPoiseDefense = ArmorPoiseBonus;
        }
    }
    private int SetMaxHealthFromHealthLevel()
    {
        MaxHealth = HealthLevel * 10;
        return MaxHealth;
    }

    #region Damage Functions
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

    public void BreakGuard()
    {
        _enemyAnimatorManager.PlayTargetAnimation("Break Guard", true);
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

        _enemyAnimatorManager.PlayTargetAnimation(damageAnimation, true);

        if(CurrentHealth <= 0)
        {
            HandleDeath();
        }
    }
    private void HandleDeath()
    {
        CurrentHealth = 0;
        _enemyAnimatorManager.PlayTargetAnimation("Death_01", true);
        IsDead = true;
    }

    #endregion
}
