using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [Header("Lock On Camera")]
    [Space(15)]
    [SerializeField] private Transform _lockOnTransform;

    [Header("Critical Damage")]
    [Space(15)]
    [SerializeField] private int _pendingCriticalDamage;

    [Header("Combat Colliders")]
    [Space(15)]
    [SerializeField] private BoxCollider _riposteBoxCollider;
    private CriticalDamageCollider _criticalDamageCollider;

    [Header("Combat Flags")]
    [Space(15)]
    private bool _canBeRiposted;
    private bool _canbeParried;
    private bool _isParrying;

    /*[Header("Enemy Lock On")]
    [Space(15)]
    [SerializeField] private Transform _lockOnTransform;
    */

    #region GET & SET
    
    public int PendingCriticalDamage { get { return _pendingCriticalDamage; } set { _pendingCriticalDamage = value; }}
    
    //public Transform LockOnTransform { get { return _lockOnTransform; } set { _lockOnTransform = value; }}
    
    public CriticalDamageCollider CriticalDamageCollider { get { return _criticalDamageCollider; } set { _criticalDamageCollider = value; }}
    public BoxCollider RiposteBoxCollider { get { return _riposteBoxCollider; } set { _riposteBoxCollider = value; }}
    
    public Transform LockOnTransform { get { return _lockOnTransform; } set { _lockOnTransform = value; }}
    
    public bool IsParrying { get { return _isParrying; } set { _isParrying = value; }}
    public bool CanBeParried { get { return _canbeParried; } set { _canbeParried = value; }}
    public bool CanBeRiposted { get { return _canBeRiposted; } set { _canBeRiposted = value; }}
    #endregion
}

