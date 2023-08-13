using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLocomotion : MonoBehaviour
{
    [Header("Components")]
    [Space(15)]
    private EnemyManager _enemyManager;
    private EnemyAnimatorController _enemyAnimatorController;

    [SerializeField] private CapsuleCollider _characterCollider;
    [SerializeField] private CapsuleCollider _characterCollisionBlockerCollider;

    private void Awake()
    {
        _enemyManager = GetComponent<EnemyManager>();
        _enemyAnimatorController = GetComponent<EnemyAnimatorController>();
    
        Physics.IgnoreCollision(_characterCollider, _characterCollisionBlockerCollider, true);
    }
    
}
