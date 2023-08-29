using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    private EnemyAnimatorController _enemyAnimatorController;
    [SerializeField] private UIEnemyHealthBar _enemyHealthBar;
    private void Awake() 
    {
        _enemyAnimatorController  = GetComponent<EnemyAnimatorController>();
    }
    private void Start() 
    {
        MaxHealth = SetMaxHealthFromHealthLevel();
        CurrentHealth = MaxHealth;
        _enemyHealthBar.SetMaxHealth(MaxHealth);
    }

    private int SetMaxHealthFromHealthLevel()
    {
        MaxHealth = HealthLevel * 10;
        return MaxHealth;
    }

    public void TakeDamageNoAnimation(int damage)
    { 
        CurrentHealth = CurrentHealth - damage;
        
        if(CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            IsDead = true;
        }       
    }
    public void TakeDamage(int damage, string damageAnimation = "Damage_01")
    {
        if(IsDead)
        {
            return;
        }
        CurrentHealth = CurrentHealth - damage;
        _enemyHealthBar.SetHealth(CurrentHealth);

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
