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

    #region GET & SET

    public GameObject ItemInteractableGameObject {get { return _itemInteractableGameObject; } set { _itemInteractableGameObject = value; }}

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

        IsInteracting = _animator.GetBool("IsInteracting");
        CanDoCombo = _animator.GetBool("CanDoCombo");
        IsUsingRightHand = _animator.GetBool("IsUsingRightHand");
        IsUsingLeftHand = _animator.GetBool("IsUsingLeftHand");
        IsInvulnerable = _animator.GetBool("IsInvulnerable");
        
        _animator.SetBool("IsInAir", IsInAir);
        _animator.SetBool("IsDead", _playerStatsManager.IsDead);
        _animator.SetFloat("InAirTimer", _playerLocomotionManager.InAirTimer);
        _animator.SetBool("IsGrounded", IsGrounded);
        _animator.SetBool("IsBlocking", IsBlocking);

        _inputHandler.TickInput(delta);
        _playerAnimatorManager.canRotate = _animator.GetBool("CanRotate");
        _playerLocomotionManager.HandleDodge(delta);
        _playerStatsManager.RegenerateStamina();

        CheckForInteractableObject();

        if(_inputHandler.IsHitEnemy && !_inputHandler.IsInRage)
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
        if(IsInAir)
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
                IsSprinting = false;
                _inputHandler.RunFlag = false;
            }
            if(_inputHandler.MoveAmount > 0.5f && _playerStatsManager.CurrentStamina > 0)
            {
                IsSprinting = true;
            }
        }
        else
        {
            IsSprinting = false;
        }
    }
}
