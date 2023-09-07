using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : CharacterManager
{
    private InputHandler _inputHandler;
    private Animator _animator;
    private CameraHandler _cameraHandler;
    private PlayerLocomotionManager _playerLocomotionManager;
    private PlayerStatsManager _playerStatsManager;
    private PlayerAnimatorManager _playerAnimatorManager;

    [Header("Item Collect Components")]
    [Space(15)] 
    private InteractableUI _interactableUI;
    [SerializeField] private GameObject _interactableUIGameObject;
    [SerializeField] private GameObject _itemInteractableGameObject;


    #region Player Flags

    private bool _isInteracting;
    private bool _isInAir;
    private bool _isGrounded;
    private bool _canDoCombo;
    private bool _isRollingOrSteppingBack;
    private bool _isAttacking;
    private bool _twoHandFlag;
    private bool _isUsingLeftHand;
    private bool _isUsingRightHand;
    private bool _isSprinting;
    private bool _isHitEnemy;
    private bool _isInRage;

    #endregion

    #region GET & SET

    public GameObject ItemInteractableGameObject {get { return _itemInteractableGameObject; } set { _itemInteractableGameObject = value; }}

    public bool IsGrounded { get { return _isGrounded; } set { _isGrounded = value; }}
    public bool IsInteracting { get { return _isInteracting; } set { _isInteracting = value; }}
    public bool IsInAir { get { return _isInAir; } set { _isInAir = value; }}
    public bool IsSprinting { get { return _isSprinting; } set { _isSprinting = value; }}
    public bool CanDoCombo { get { return _canDoCombo; } set { _canDoCombo = value; }}
    public bool IsDodge { get { return _isRollingOrSteppingBack; } set { _isRollingOrSteppingBack = value; }}
    public bool IsAttacking { get { return _isAttacking; } set { _isAttacking = value; }}
    public bool TwoHandFlag { get { return _twoHandFlag; } set { _twoHandFlag = value; }}
    public bool IsUsingLeftHand { get { return _isUsingLeftHand; } set { _isUsingLeftHand = value; }}
    public bool IsUsingRightHand { get { return _isUsingRightHand; } set { _isUsingRightHand = value; }}
    public bool IsHitEnemy { get { return _isHitEnemy; } set { _isHitEnemy = value; }}
    public bool IsInRage { get { return _isInRage; } set { _isInRage = value; }}
    

    #endregion  

    private void Awake() 
    {
        _cameraHandler = FindObjectOfType<CameraHandler>();
        _inputHandler = FindObjectOfType<InputHandler>();
        _animator = GetComponent<Animator>();
        _playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        _interactableUI = FindObjectOfType<InteractableUI>();
        _playerStatsManager = GetComponent<PlayerStatsManager>();
        _playerAnimatorManager = GetComponent<PlayerAnimatorManager>();

    }
    private void Update()
    {
        float delta = Time.deltaTime;

        _isInteracting = _animator.GetBool("IsInteracting");
        _canDoCombo = _animator.GetBool("CanCombo");
        _isUsingRightHand = _animator.GetBool("IsUsingRightHand");
        _isUsingLeftHand = _animator.GetBool("IsUsingLeftHand");
        IsInvulnerable = _animator.GetBool("IsInvulnerable");
        
        _animator.SetBool("IsInAir", _isInAir);
        _animator.SetBool("IsDead", _playerStatsManager.IsDead);
        _animator.SetFloat("InAirTimer", _playerLocomotionManager.InAirTimer);
        _animator.SetBool("IsGrounded", _isGrounded);
        _animator.SetBool("IsBlocking", IsBlocking);

        _inputHandler.TickInput(delta);
        _playerAnimatorManager.canRotate = _animator.GetBool("CanRotate");
        _playerLocomotionManager.HandleDodge(delta);
        _playerStatsManager.RegenerateStamina();

        CheckForInteractableObject();

        if(_isHitEnemy && !_isInRage)
        {
            _playerStatsManager.RegenerateRage();
        }
    }
    private void FixedUpdate()
    {
        float delta = Time.fixedDeltaTime;

        _playerLocomotionManager.HandleGravity(delta, _playerLocomotionManager.MoveDirection);
        _playerLocomotionManager.HandleMovement(delta);
        _playerLocomotionManager.HandleRotation(delta);
    }
    
    private void LateUpdate() 
    {
        _inputHandler.SBFlag = false;

        HandleSprinting();

        float delta = Time.deltaTime;
        
        if (_cameraHandler != null)
        {
            _cameraHandler.FollowTarget(delta);
            _cameraHandler.HandleCameraRotation(delta, _inputHandler.HorizontalCameraMovement, _inputHandler.VerticalCameraMovement);
        }   
        if(_isInAir)
        {
            _playerLocomotionManager.InAirTimer = _playerLocomotionManager.InAirTimer + Time.deltaTime;
        } 
    }
    
    public void CheckForInteractableObject()
    {
        RaycastHit hit;

        if(Physics.SphereCast(transform.position, 0.3f, transform.forward, out hit, 1f, _playerLocomotionManager.IgnoreForGroundCheck))
        {
            if(hit.collider.tag == "Interactable")
            {
                Interactable interactableObject = hit.collider.GetComponent<Interactable>();

                if(interactableObject != null)
                {
                    string interactableText = interactableObject.InteractableText;
                    _interactableUI.InteractableText.text = interactableText;
                    _interactableUIGameObject.SetActive(true);
                    
                    if(_inputHandler.InteractInput)
                    {
                        hit.collider.GetComponent<Interactable>().Interact(this);
                    }
                }
            }
        }
        else
        {
            if(_interactableUIGameObject != null)
            {
                _interactableUIGameObject.SetActive(false);
            }
            if(_itemInteractableGameObject != null && _inputHandler.InteractInput)
            {
                _itemInteractableGameObject.SetActive(false);
            }
        }
    }
    private void HandleSprinting()
    {
        if(_inputHandler.RunFlag)
        {
            if(_playerStatsManager.CurrentStamina <= 0)
            {
                _isSprinting = false;
                _inputHandler.RunFlag = false;
            }
            if(_inputHandler.MoveAmount > 0.5f && _playerStatsManager.CurrentStamina > 0)
            {
                _isSprinting = true;
            }
        }
        else
        {
            _isSprinting = false;
        }
    }
}
