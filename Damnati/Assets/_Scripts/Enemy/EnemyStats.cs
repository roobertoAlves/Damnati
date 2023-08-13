using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    private  Animator _anim;
    private void Awake() 
    {
        _anim  = GetComponent<Animator>();
    }
    private void Start() 
    {
        MaxHealth = SetMaxHealthFromHealthLevel();
        CurrentHealth = MaxHealth;
    }

    private int SetMaxHealthFromHealthLevel()
    {
        MaxHealth = HealthLevel * 10;
        return MaxHealth;
    }

    public void TakeDamage(int damage)
    {
        if(IsDead)
        {
            return;
        }
        CurrentHealth = CurrentHealth - damage;

        _anim.Play("Damage_01");

        if(CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            _anim.Play("Dead_01");
            IsDead = true;
        }
    }
}
