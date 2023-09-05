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
    [SerializeField] private bool _canBeRiposted;
    private bool _canBeParried;
    private bool _isParrying;
    private bool _isBlocking;
    private bool _isInvulnerable;
    private bool _isParried;

    [Header("Movement Flags")]
    [Space(15)]
    [SerializeField] private bool _isRotatingWithRootMotion;
    [SerializeField] private bool _canRotate;


    #region GET & SET
    
    public int PendingCriticalDamage { get { return _pendingCriticalDamage; } set { _pendingCriticalDamage = value; }}
    
    public Transform LockOnTransform { get { return _lockOnTransform; } set { _lockOnTransform = value; }}
    
    public CriticalDamageCollider CriticalDamageCollider { get { return _riposteDamageCollider; } set { _riposteDamageCollider = value; }}
  
    public bool IsParrying { get { return _isParrying; } set { _isParrying = value; }}
    public bool IsParried { get { return _isParried; } set { _isParried = value; }}
    public bool CanBeParried { get { return _canBeParried; } set { _canBeParried = value; }}
    public bool CanBeRiposted { get { return _canBeRiposted; } set { _canBeRiposted = value; }}
    public bool IsBlocking { get { return _isBlocking; } set { _isBlocking = value; }}
    public bool IsRotatingWithRootMotion { get { return _isRotatingWithRootMotion; } set { _isRotatingWithRootMotion = value; }}
    public bool CanRotate { get { return _canRotate; } set { _canRotate = value; }}
    public bool IsInvulnerable {get { return _isInvulnerable; } set { _isInvulnerable = value; }}
    #endregion
}

