using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLocomotionManager : MonoBehaviour
{
    private CharacterManager _characterManager;

    [SerializeField] private Vector3 _moveDirection;
    [SerializeField] private LayerMask _groundLayer;

    [Header("Gravity Settings")]
    [Space(15)]
    private float _inAirTimer;
    [SerializeField] private float _groundCheckSphereRadius;
    [SerializeField] protected Vector3 yVelocity;
    [SerializeField] protected float gravityForce = -25;
    [SerializeField] protected float groundYVelocity = -20;
    [SerializeField] protected float fallStartYVelocity = -7;
    protected bool fallingVelocitySet = false;

    #region GET & SET
    public float MoveDirectionY  { get { return _moveDirection.y; } set { _moveDirection.y = value;}}
    public Vector3 MoveDirection { get { return _moveDirection; } set { _moveDirection = value; }}
    public LayerMask GroundLayer { get { return _groundLayer; }}
    public float InAirTimer { get { return _inAirTimer; } set { _inAirTimer = value; }}
    
    #endregion
   
    protected virtual void Awake() 
    {
        _characterManager = GetComponent<CharacterManager>();
    }
    protected virtual void Start()
    {

    }
    protected virtual void Update()
    {
        _characterManager.IsGrounded = Physics.CheckSphere(_characterManager.transform.position, _groundCheckSphereRadius, _groundLayer);
        _characterManager.Animator.SetBool("IsGrounded", _characterManager.IsGrounded);
        HandleGroundCheck();
    }
    public virtual void HandleGroundCheck()
    {
        if(_characterManager.IsGrounded)
        {
            if(yVelocity.y < 0)
            {
                _inAirTimer = 0;
                fallingVelocitySet = false;
                yVelocity.y = groundYVelocity;
            }
        }
        else
        {
            if(!fallingVelocitySet)
            {
                fallingVelocitySet = true;
                yVelocity.y = fallStartYVelocity;
            }
            
            _inAirTimer = _inAirTimer + Time.deltaTime;
            yVelocity.y += gravityForce * Time.deltaTime;
        }  
        
        _characterManager.Animator.SetFloat("InAirTimer", _inAirTimer);
        _characterManager.CharacterController.Move(yVelocity * Time.deltaTime);
    }   

    private void OnDrawGizmosSelected() 
    {
        Gizmos.DrawSphere(transform.position, _groundCheckSphereRadius);
    }
}
