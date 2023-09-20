using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class EnemyStatsManager : CharacterStatsManager
{   
    private EnemyManager _enemy;
    [SerializeField] private UIEnemyHealthBar _enemyHealthBar;

    [SerializeField] private bool _isBoss;

    #region  GET & SET
    public bool IsBoss { get { return _isBoss;}}
    #endregion
    protected override void Awake() 
    {
        base.Awake();
        _enemy = GetComponent<EnemyManager>();
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

    #region Damage Functions
    public override void TakeDamageNoAnimation(int physicalDamage, int fireDamage)
    { 
        base.TakeDamageNoAnimation(physicalDamage, fireDamage);

        if(!_isBoss)
        {
            _enemyHealthBar.SetHealth(CurrentHealth);
        }
        else if(_isBoss && _enemy.EnemyBossManager != null)
        {
            _enemy.EnemyBossManager .UpdateBossHealthBar(CurrentHealth, MaxHealth);
        }
    }

    public void BreakGuard()
    {
        _enemy.EnemyAnimatorManager.PlayTargetAnimation("Break Guard", true);
    }
    public override void TakeDamage(int physicalDamage, int fireDamage, string damageAnimation)
    {

        base.TakeDamage(physicalDamage, fireDamage, damageAnimation);

        if(!_isBoss)
        {
            _enemyHealthBar.SetHealth(CurrentHealth);
        }
        else if(_isBoss && _enemy.EnemyBossManager  != null)
        {
            _enemy.EnemyBossManager.UpdateBossHealthBar(CurrentHealth, MaxHealth);
        }

        _enemy.EnemyAnimatorManager.PlayTargetAnimation(damageAnimation, true);

        if(CurrentHealth <= 0)
        {
            HandleDeath();
        }
    }
    private void HandleDeath()
    {
        CurrentHealth = 0;
        _enemy.IsDead = true;
        _enemy.EnemyAnimatorManager.PlayTargetAnimation("Death_01", true);
    }

    #endregion
}
