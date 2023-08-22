using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    [HideInInspector]
    public Animator Anim;
    public bool canRotate;

    
    public void PlayTargetAnimation(string targetAnim, bool isInteracting)
    {
        Anim.applyRootMotion = isInteracting;
        Anim.SetBool("IsInteracting", isInteracting);
        Anim.CrossFade(targetAnim, 0.3f);
    }
    public virtual void TakeCriticalDamageAnimationEvent(){}
}
