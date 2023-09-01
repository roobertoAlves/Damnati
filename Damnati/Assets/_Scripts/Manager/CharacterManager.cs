using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [Header("Lock On Transform")]
    [Space(15)]
    [SerializeField] private Transform _lockOnTransform;

    [Header("Critical Damage")]
    [Space(15)]
    [SerializeField] private int _pendingCriticalDamage;

    [Header("Combat Colliders")]
    [Space(15)]
    [SerializeField] private CriticalDamageCollider _riposteDamageCollider;

    [Header("Combat Flags")]
    [Space(15)]
    private bool _canBeRiposted;
    private bool _canbeParried;
    private bool _isParrying;
    private bool _isBlocking;

    [Header("Movement Flags")]
    [Space(15)]
    [SerializeField] private bool _isRotatingWithRootMotion;
    [SerializeField] private bool _canRotate;


    #region GET & SET
    
    public int PendingCriticalDamage { get { return _pendingCriticalDamage; } set { _pendingCriticalDamage = value; }}
    
    public Transform LockOnTransform { get { return _lockOnTransform; } set { _lockOnTransform = value; }}
    
    public CriticalDamageCollider CriticalDamageCollider { get { return _riposteDamageCollider; } set { _riposteDamageCollider = value; }}
  
    public bool IsParrying { get { return _isParrying; } set { _isParrying = value; }}
    public bool CanBeParried { get { return _canbeParried; } set { _canbeParried = value; }}
    public bool CanBeRiposted { get { return _canBeRiposted; } set { _canBeRiposted = value; }}
    public bool IsBlocking { get { return _isBlocking; } set { _isBlocking = value; }}
    public bool IsRotatingWithRootMotion { get { return _isRotatingWithRootMotion; } set { _isRotatingWithRootMotion = value; }}
    public bool CanRotate { get { return _canRotate; } set { _canRotate = value; }}
    #endregion
}

