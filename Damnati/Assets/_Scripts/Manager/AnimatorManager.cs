using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{ 
    public Animator Anim;
    public bool canRotate;

    
    public void PlayTargetAnimation(string targetAnim, bool isInteracting, bool canRotate = false)
    {
        Anim.applyRootMotion = isInteracting;
        Anim.SetBool("CanRotate", canRotate);
        Anim.SetBool("IsInteracting", isInteracting);
        Anim.CrossFade(targetAnim, 0.2f);
    }
    public virtual void TakeCriticalDamageAnimationEvent(){}
}
