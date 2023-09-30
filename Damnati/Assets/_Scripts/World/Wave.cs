using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "World/Waves/Enemy Waves")]
public class Wave : ScriptableObject
{
    [SerializeField] private GameObject[] _enemiesInWave;
    [SerializeField] private float _timeBeforeThisWave;
    [SerializeField] private float _numberToSpawn;
    [SerializeField] private bool _isBossWave; // Adicione este campo

    #region GET & SET
    public GameObject[] EnemiesInWave { get { return _enemiesInWave; }}
    public float TimeBeforeThisWave { get { return _timeBeforeThisWave; }}
    public float NumberToSpawn { get { return _numberToSpawn; }}
    public bool IsBossWave { get { return _isBossWave; }} // Getter para IsBossWave
    #endregion
}
