using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class EnemyStatsManager : CharacterStatsManager
{
    private EnemyAnimatorManager _enemyAnimatorManager;
    private EnemyBossManager _enemyBossManager;

    [SerializeField] private UIEnemyHealthBar _enemyHealthBar;

    [SerializeField] private bool _isBoss;

    #region  GET & SET
    public bool IsBoss { get { return _isBoss;}}
    #endregion
    protected override void Awake() 
    {
        base.Awake();
        _enemyAnimatorManager  = GetComponent<EnemyAnimatorManager>();
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

    #region Damage Functions
    public override void TakeDamageNoAnimation(int physicalDamage, int fireDamage)
    { 
        base.TakeDamageNoAnimation(physicalDamage, fireDamage);

        if(!_isBoss)
        {
            _enemyHealthBar.SetHealth(CurrentHealth);
        }
        else if(_isBoss && _enemyBossManager != null)
        {
            _enemyBossManager.UpdateBossHealthBar(CurrentHealth, MaxHealth);
        }
    }

    public void BreakGuard()
    {
        _enemyAnimatorManager.PlayTargetAnimation("Break Guard", true);
    }
    public override void TakeDamage(int physicalDamage, int fireDamage, string damageAnimation)
    {

        base.TakeDamage(physicalDamage, fireDamage, damageAnimation);

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
