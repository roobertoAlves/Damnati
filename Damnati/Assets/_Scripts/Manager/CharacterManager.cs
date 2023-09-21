using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    private Animator _animator;
    private CharacterAnimatorManager _characterAnimatorManager;
    private CharacterWeaponSlotManager _characterWeaponSlotManager;
    private CharacterStatsManager _characterStatsManager;
    private CharacterInventoryManager _characterInventoryManager;
    private CharacterEffectsManager _characterEffectsManager;
    private CharacterCombatManager _characterCombatManager;

    [Header("Lock On Transform")]
    [Space(15)]
    [SerializeField] private Transform _lockOnTransform;

    [Header("Critical Damage")]
    [Space(15)]
    [SerializeField] private int _pendingCriticalDamage;

    [Header("Combat Colliders")]
    [Space(15)]
    [SerializeField] private CriticalDamageCollider _riposteDamageCollider;


    [Header("Interaction")]
    [Space(15)]
    private bool _isInteracting;
    
    [Header("Status")]
    [Space(15)]
    private bool _isDead;

    [Header("Combat Flags")]
    [Space(15)]
    [SerializeField] private bool _canBeRiposted;
    private bool _canBeParried;
    private bool _canDoCombo;
    private bool _isParrying;
    private bool _isBlocking;
    private bool _isInvulnerable;
    private bool _isUsingRightHand;
    private bool _isUsingLeftHand;
    private bool _isTwoHandingWeapon;
    private bool _isAiming;
    private bool _isHoldingArrow;
    private bool _isPerformingFullyChargedAttack;

    [Header("Movement Flags")]
    [Space(15)]
    [SerializeField] private bool _isRotatingWithRootMotion;
    [SerializeField] private bool _canRotate;
    private bool _isSprinting;
    private bool _isInAir;
    private bool _isGrounded;



    #region GET & SET
    public Animator Animator { get { return _animator; } set { _animator = value; }}
    public CharacterAnimatorManager CharacterAnimator { get { return _characterAnimatorManager; }}
    public CharacterWeaponSlotManager CharacterWeaponSlot { get { return _characterWeaponSlotManager; }}
    public CharacterStatsManager CharacterStats { get { return _characterStatsManager; }}
    public CharacterInventoryManager CharacterInventory { get { return _characterInventoryManager;}}
    public CharacterEffectsManager CharacterEffects { get { return _characterEffectsManager; }}
    public CharacterCombatManager CharacterCombat { get { return _characterCombatManager; } set { _characterCombatManager = value;}}
    public int PendingCriticalDamage { get { return _pendingCriticalDamage; } set { _pendingCriticalDamage = value; }}
    
    public Transform LockOnTransform { get { return _lockOnTransform; } set { _lockOnTransform = value; }}
    
    public CriticalDamageCollider RiposteDamageCollider { get { return _riposteDamageCollider; } set { _riposteDamageCollider = value; }}

    public bool IsSprinting { get { return _isSprinting; } set { _isSprinting = value; }}
    public bool IsInAir { get { return _isInAir; } set { _isInAir = value; }}
    public bool IsGrounded { get { return _isGrounded; } set { _isGrounded = value; }}
    public bool IsInteracting { get { return _isInteracting; } set { _isInteracting = value; }}
    public bool IsParrying { get { return _isParrying; } set { _isParrying = value; }}
    public bool CanBeParried { get { return _canBeParried; } set { _canBeParried = value; }}
    public bool CanBeRiposted { get { return _canBeRiposted; } set { _canBeRiposted = value; }}
    public bool IsBlocking { get { return _isBlocking; } set { _isBlocking = value; }}
    public bool IsRotatingWithRootMotion { get { return _isRotatingWithRootMotion; } set { _isRotatingWithRootMotion = value; }}
    public bool CanRotate { get { return _canRotate; } set { _canRotate = value; }}
    public bool IsInvulnerable {get { return _isInvulnerable; } set { _isInvulnerable = value; }}
    public bool CanDoCombo { get { return _canDoCombo; } set { _canDoCombo = value; }}
    public bool IsUsingRightHand { get { return _isUsingRightHand; } set { _isUsingRightHand = value; }}
    public bool IsUsingLeftHand { get { return _isUsingLeftHand; } set { _isUsingLeftHand = value; }}
    public bool IsTwoHandingWeapon { get { return _isTwoHandingWeapon; } set { _isTwoHandingWeapon = value; }}
    public bool IsHoldingArrow { get { return _isHoldingArrow; } set { _isHoldingArrow = value; }}
    public bool IsAiming { get { return _isAiming; } set { _isAiming = value; }}
    public bool IsDead { get { return _isDead; } set { _isDead = value; }}
    public bool IsPerformingFullyChargedAttack { get { return _isPerformingFullyChargedAttack; } set { _isPerformingFullyChargedAttack = value; }}
    #endregion

    protected virtual void Awake()
    {
        _animator = GetComponent<Animator>();
        _characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
        _characterWeaponSlotManager = GetComponent<CharacterWeaponSlotManager>();
        _characterStatsManager = GetComponent<CharacterStatsManager>();
        _characterInventoryManager = GetComponent<CharacterInventoryManager>();
        _characterEffectsManager = GetComponent<CharacterEffectsManager>();
        _characterCombatManager = GetComponent<CharacterCombatManager>();
    }
    protected virtual void FixedUpdate()
    {
        _characterAnimatorManager.CheckHandIKWeight(_characterWeaponSlotManager.RightHandIKTarget, _characterWeaponSlotManager.LeftHandIKTarget, _isTwoHandingWeapon);
    }
    public virtual void UpdateWhichHandCharacterIsUsing(bool usingRightHand)
    {
        if(usingRightHand)
        {
            _isUsingRightHand = true;
            _isUsingLeftHand = false;
        }
        else
        {
            _isUsingLeftHand = true;
            _isUsingRightHand = false;
        }
    }
}

