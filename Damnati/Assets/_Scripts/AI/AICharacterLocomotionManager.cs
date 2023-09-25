using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICharacterLocomotionManager : MonoBehaviour
{
    [Header("Components")]
    [Space(15)]
    private AICharacterManager _aiCharacterManager;
    [SerializeField] private CapsuleCollider _characterCollider;
    [SerializeField] private CapsuleCollider _characterCollisionBlockerCollider;


    [SerializeField] private LayerMask _detectionLayer; 

    #region GET & SET

    public LayerMask DetectionLayer { get { return _detectionLayer; } set { _detectionLayer = value; }}
    #endregion
    private void Awake()
    {
        _aiCharacterManager = GetComponent<AICharacterManager>();
    }
    private void Start() 
    {
        Physics.IgnoreCollision(_characterCollider, _characterCollisionBlockerCollider, true);    
    }   
}
