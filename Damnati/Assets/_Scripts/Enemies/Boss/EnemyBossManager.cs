using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossManager : MonoBehaviour
{
    [SerializeField] private string _bossName;

    private UIBossHealthBar _bossHealthBar;
    private EnemyStats _enemyStats;


    #region GET & SET
    public string BossName { get { return _bossName; } set { _bossName = value; }}
    #endregion
    private void Awake() 
    {
        _bossHealthBar = FindObjectOfType<UIBossHealthBar>();
        _enemyStats = GetComponent<EnemyStats>();   
    }

    private void Start() 
    {
        _bossHealthBar.SetBossName(_bossName);
        _bossHealthBar.SetBossMaxHealth(_enemyStats.MaxHealth);    
    }

    public void UpdateBossHealthBar(int currentHealth)
    {
        _bossHealthBar.SetBossCurrentHealth(currentHealth);
    }
}
