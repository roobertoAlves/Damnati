using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [Header("Critical Damage")]
    [Space(15)]
    [SerializeField] private int _pendingCriticalDamage;

    /*[Header("Enemy Lock On")]
    [Space(15)]
    [SerializeField] private Transform _lockOnTransform;
    */

    #region GET & SET
    
    public int PendingCriticalDamage { get { return _pendingCriticalDamage; } set { _pendingCriticalDamage = value; }}
    
    //public Transform LockOnTransform { get { return _lockOnTransform; } set { _lockOnTransform = value; }}
    #endregion
}

