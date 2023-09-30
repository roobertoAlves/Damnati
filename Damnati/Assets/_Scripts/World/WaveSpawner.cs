using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    private WorldEventManager _worldEventManager;

    [Header("Wave Settings")]
    [SerializeField] private Wave[] _waves;
    [SerializeField] private GameObject _waveParent;
    private int _currentWaveIndex = 0;
    private bool _bossWaveStarted = false;

    [Header("Spawn Settings")]
    [SerializeField] private Transform[] _spawnPoints;
    private float _timeBetweenSpawns = 0f;
    private int _enemiesRemaining;

    private void Awake()
    {
        _worldEventManager = FindObjectOfType<WorldEventManager>();
        _enemiesRemaining = 0;
    }

    private void Update()
    {
        if (_currentWaveIndex >= _waves.Length)
        {
            // Todas as ondas e a onda do chefe foram completadas
            return;
        }

        if (_enemiesRemaining == 0)
        {
            if (_timeBetweenSpawns <= 0f)
            {
                if (_currentWaveIndex == _waves.Length - 1 && !_bossWaveStarted)
                {
                    SpawnBossWave();
                }
                else
                {
                    SpawnWave();
                    _timeBetweenSpawns = _waves[_currentWaveIndex].TimeBeforeThisWave;
                }
            }
            else
            {
                _timeBetweenSpawns -= Time.deltaTime;
            }
        }
    }
    private void SpawnBossWave()
    {
        Wave currentWave = _waves[_currentWaveIndex];

        GameObject waveParent = _waveParent;

        int spawnPointIndex = Random.Range(0, _spawnPoints.Length);

        // Instancie o chefe diretamente da última onda
        GameObject boss = Instantiate(currentWave.EnemiesInWave[currentWave.EnemiesInWave.Length - 1],
            _spawnPoints[spawnPointIndex].position, _spawnPoints[spawnPointIndex].rotation);

        boss.transform.parent = waveParent.transform;

        boss.AddComponent<AICharacterManager>();
        _enemiesRemaining++;

        // Agora, chame o evento e defina a flag _bossWaveStarted como verdadeira
        Debug.Log("Boss Wave Started");
        _worldEventManager.ActivateBossFight();
        _bossWaveStarted = true;
    }
    private void SpawnWave()
    {
        Wave currentWave = _waves[_currentWaveIndex];

        GameObject waveParent = _waveParent;

        for (int i = 0; i < currentWave.NumberToSpawn; i++)
        {
            int enemyIndex = Random.Range(0, currentWave.EnemiesInWave.Length);
            int spawnPointIndex = Random.Range(0, _spawnPoints.Length);

            GameObject enemy = Instantiate(currentWave.EnemiesInWave[enemyIndex],
                _spawnPoints[spawnPointIndex].position, _spawnPoints[spawnPointIndex].rotation);

            enemy.transform.parent = waveParent.transform;

            enemy.AddComponent<AICharacterManager>();
            _enemiesRemaining++;
        }
    }

    public void EnemyDefeated()
    {
        _enemiesRemaining--;

        if (_enemiesRemaining == 0)
        {
            _bossWaveStarted = false;
            _currentWaveIndex++;
            if (_currentWaveIndex < _waves.Length)
            {
                _timeBetweenSpawns = _waves[_currentWaveIndex].TimeBeforeThisWave;
            }
        }
    }
}
