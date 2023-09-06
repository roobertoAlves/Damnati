using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetAnimatorBoolAI : ResetAnimatorBool
{
    [SerializeField] private string _isPhaseShifting = "IsPhaseShifting";
    [SerializeField] private bool _isPhaseShiftingStatus = false;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        animator.SetBool(_isPhaseShifting, _isPhaseShiftingStatus);
    }  
}
