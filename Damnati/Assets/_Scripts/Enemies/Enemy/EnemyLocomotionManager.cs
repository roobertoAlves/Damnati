using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLocomotionManager : MonoBehaviour
{
    [Header("Components")]
    [Space(15)]
    private EnemyManager _enemyManager;
    private EnemyAnimatorManager _enemyAnimatorManager;

    [SerializeField] private CapsuleCollider _characterCollider;
    [SerializeField] private CapsuleCollider _characterCollisionBlockerCollider;


    [SerializeField] private LayerMask _detectionLayer; 

    #region GET & SET

    public LayerMask DetectionLayer { get { return _detectionLayer; } set { _detectionLayer = value; }}
    #endregion
    private void Awake()
    {
        _enemyManager = GetComponent<EnemyManager>();
        _enemyAnimatorManager = GetComponent<EnemyAnimatorManager>();
    }
    private void Start() 
    {
        Physics.IgnoreCollision(_characterCollider, _characterCollisionBlockerCollider, true);    
    }   
}
