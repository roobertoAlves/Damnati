using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetAnimatorBool : StateMachineBehaviour
{

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

    [SerializeField] private string _isUsingRightHandBool = "IsUsingRightHand";
    [SerializeField] private bool _isUsingRightHandStatus = false;
    [SerializeField] private string _isUsingLeftHandBool = "IsUsingLeftHand";
    [SerializeField] private bool _isUsingLeftHandStatus = false;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(_isInteractingBool, _isInteractingStatus);
        animator.SetBool(_isRotatingWithRootMotionBool, _isRotatingWithRootMotionStatus);
        animator.SetBool(_canRotateBool, _canRotateStatus);
        animator.SetBool(_isInvulnerableBool, _isInvulnerableStatus);
        animator.SetBool(_isUsingRightHandBool, _isUsingRightHandStatus);
        animator.SetBool(_isUsingLeftHandBool, _isUsingLeftHandStatus);
        animator.SetBool(_isMirroredBool, _isMirroredStatus);
    }
}
