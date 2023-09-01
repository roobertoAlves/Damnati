using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetAnimatorBool : StateMachineBehaviour
{
    [SerializeField] private string targetBool;
    [SerializeField] private bool status;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(targetBool, status);
    }

    /*
    [SerializeField] private string _isInteractingBool = "IsInteracting";
    [SerializeField] private bool _isInteractingStatus = false;

    [SerializeField] private string _isRotatingWithRootMotionBool = "IsRotatingWithRootMotion";
    [SerializeField] private bool _isRotatingWithRootMotionStatus = false;

    [SerializeField] private string _canRotateBool = "CanRotate";
    [SerializeField] private bool _canRotateStatus = true;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(_isInteractingBool, _isInteractingStatus);
        animator.SetBool(_isRotatingWithRootMotionBool, _isRotatingWithRootMotionStatus);
        animator.SetBool(_canRotateBool, _canRotateStatus);
    }
    */
}
