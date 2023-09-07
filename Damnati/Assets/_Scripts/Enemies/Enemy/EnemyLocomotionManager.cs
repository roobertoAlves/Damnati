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

    private void Awake()
    {
        _enemyManager = GetComponent<EnemyManager>();
        _enemyAnimatorManager = GetComponent<EnemyAnimatorManager>();
    
        Physics.IgnoreCollision(_characterCollider, _characterCollisionBlockerCollider, true);
    }
    
}
