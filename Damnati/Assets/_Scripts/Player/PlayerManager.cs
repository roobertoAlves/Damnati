using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private InputHandler _inputHandler;
    private AnimatorHandler _animatorHandler;
    private CameraHandler _cameraHandler;
    private PlayerLocomotion _playerLocomotion;
    private PlayerStats _playerStats;

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
    private bool _isInvulnerable;
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
    public bool IIsDodge { get { return _isRollingOrSteppingBack; } set { _isRollingOrSteppingBack = value; }}
    public bool IsAttacking { get { return _isAttacking; } set { _isAttacking = value; }}
    public bool TwoHandFlag { get { return _twoHandFlag; } set { _twoHandFlag = value; }}
    public bool IsUsingLeftHand { get { return _isUsingLeftHand; } set { _isUsingLeftHand = value; }}
    public bool IsUsingRightHand { get { return _isUsingRightHand; } set { _isUsingRightHand = value; }}
    public bool IsInvulnerable { get { return _isInvulnerable; } set { _isInvulnerable = value; }}
    public bool IsHitEnemy { get { return _isHitEnemy; } set { _isHitEnemy = value; }}
    public bool IsInRage { get { return _isInRage; } set { _isInRage = value; }}
   
    #endregion  

    private void Awake() 
    {
        _inputHandler = FindObjectOfType<InputHandler>();
        _cameraHandler = GetComponent<CameraHandler>();
        _animatorHandler = GetComponent<AnimatorHandler>();
        _playerLocomotion = GetComponent<PlayerLocomotion>();
        _interactableUI = FindObjectOfType<InteractableUI>();
        _playerStats = GetComponent<PlayerStats>();

        _cameraHandler = CameraHandler.singleton;
    }
    private void Update() 
    {   
        float delta = Time.deltaTime;

        _isInteracting = _animatorHandler.Anim.GetBool("IsInteracting");
        _canDoCombo = _animatorHandler.Anim.GetBool("CanDoCombo");
        _isUsingLeftHand = _animatorHandler.Anim.GetBool("IsUsingLeftHand");
        _isUsingRightHand = _animatorHandler.Anim.GetBool("IsUsingRightHand");
        _isInvulnerable = _animatorHandler.Anim.GetBool("IsInvulnerable");

        _inputHandler.TickInput(delta);
        _playerLocomotion.HandleMovement(delta);
        _playerLocomotion.HandleDodge(delta);
        _playerLocomotion.HandleGravity(delta,_playerLocomotion.MoveDirection);
        _playerLocomotion.HandleAttack(delta);
        _playerLocomotion.HandleTwoWeapon(delta);
        CheckForInteractableObject();
        _playerStats.RegenerateStamina();

        if(_isHitEnemy && !_isInRage)
        {
            _playerStats.RegenerateRage();

            if (_cameraHandler != null)
            {
                _cameraHandler.FollowTarget(delta);
                _cameraHandler.HandleCameraRotation(delta, _inputHandler.HorizontalCameraMovement, _inputHandler.VerticalCameraMovement);
            }
        }
    }

    private void LateUpdate() 
    {
        _inputHandler.SBFlag = false;
        _isSprinting = _inputHandler.RunFlag;

        if(_isInAir)
        {
            _playerLocomotion.InAirTimer = _playerLocomotion.InAirTimer + Time.deltaTime;
        }    
    }
    
    public void CheckForInteractableObject()
    {
        RaycastHit hit;

        if(Physics.SphereCast(transform.position, 0.3f, transform.forward, out hit, 1f, _playerLocomotion.IgnoreForGroundCheck))
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
}
