using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetAnimatorBool : StateMachineBehaviour
{
    [SerializeField] private string _isUsingRightHandBool = "IsUsingRightHand";
    [SerializeField] private bool _isUsingRightHandStatus = false;

    [SerializeField] private string _isUsingLeftHandBool = "IsUsingLeftHand";
    [SerializeField] private bool _isUsingLeftHandStatus = false;

    [SerializeField] private string _isInteractingBool = "IsInteracting";
    [SerializeField] private bool _isInteractingStatus = false;

    [SerializeField] private string _isRotatingWithRootMotionBool = "IsRotatingWithRootMotion";
    [SerializeField] private bool _isRotatingWithRootMotionStatus = false;

    [SerializeField] private string _canRotateBool = "CanRotate";
    [SerializeField] private bool _canRotateStatus = true;
    
    [SerializeField] private string _isMirroredBool = "IsMirrored";
    [SerializeField] private bool _isMirroredStatus = false;
    
    [SerializeField] private string _isInvulnerableBool = "IsInvulnerable";
    [SerializeField] private bool _isInvulnerableStatus = false;

    #region GET & SET

    public string IsUsingRightHandBool { get { return _isUsingRightHandBool; } set { _isUsingRightHandBool = value;}}
    public bool IsUsingRightHandStatus { get { return _isUsingRightHandStatus; } set { _isUsingRightHandStatus = value; }}
    public string IsUsingLeftHandBool { get { return _isUsingLeftHandBool; } set { _isUsingLeftHandBool = value;}}
    public bool IsUsingLeftHandStatus { get { return _isUsingLeftHandStatus; } set { _isUsingLeftHandStatus = value; }}
    #endregion

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        CharacterManager character = animator.GetComponent<CharacterManager>();

        character.IsUsingLeftHand = false;
        character.IsUsingRightHand = false; 
        character.IsAttacking = false;
        character.IsBeingRiposted = false;
        character.IsPerformingRiposte = false;
        character.CanBeParried = false;
        character.CanBeRiposted = false;
        
        animator.SetBool(_isInteractingBool, _isInteractingStatus);
        animator.SetBool(_isRotatingWithRootMotionBool, _isRotatingWithRootMotionStatus);
        animator.SetBool(_canRotateBool, _canRotateStatus);
        animator.SetBool(_isInvulnerableBool, _isInvulnerableStatus);
        animator.SetBool(_isMirroredBool, _isMirroredStatus);
    }
}
