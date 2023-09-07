using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{ 
    public Animator Anim;
    protected CharacterManager _characterManager;
    protected CharacterStatsManager _characterStatsManager;
    public bool canRotate;    


    protected virtual void Awake()
    {
        _characterManager = GetComponent<CharacterManager>();
        _characterStatsManager = GetComponent<CharacterStatsManager>();
    }
    
    public void PlayTargetAnimation(string targetAnim, bool isInteracting, bool canRotate = false)
    {
        Anim.applyRootMotion = isInteracting;
        Anim.SetBool("CanRotate", canRotate);
        Anim.SetBool("IsInteracting", isInteracting);
        Anim.CrossFade(targetAnim, 0.2f);
        Debug.Log(targetAnim);
    }
    public void PlayerTargetAnimationWithRootRotation (string targetAnim, bool isInteracting)
    {
        Anim.applyRootMotion = isInteracting;
        Anim.SetBool("IsRotatingWithRootMotion", true);
        Anim.SetBool("IsInteracting", isInteracting);
        Anim.CrossFade(targetAnim, 0.2f);
    }

    #region Combat and Animation Events
    
    public virtual void CanRotate()
    {
        Anim.SetBool("CanRotate", true);
    }
    public virtual void StopRotation()
    {
        Anim.SetBool("CanRotate", false);
    }
    public virtual void EnableCombo()
    {
        Anim.SetBool("CanDoCombo", true);
    }
    public virtual void DisableCombo()
    {
        Anim.SetBool("CanDoCombo", false);
    }
    public virtual void EnableIsInvulnerable()
    {
        Anim.SetBool("IsInvulnerable", true);
    }
    public virtual void DisableIsInvulnerable()
    {
        Anim.SetBool("IsInvulnerable", false);
    }
    public virtual void EnableIsParrying()
    {
        _characterManager.IsParrying = true;
    }
    public virtual void DisableIsParrying()
    {
        _characterManager.IsParrying = false;
    }
    public virtual void EnableCanBeRiposted()
    {
        _characterManager.CanBeRiposted = true;
    }
    public virtual void DisableCanBeRiposted()
    {
        _characterManager.CanBeRiposted = false;
    }

    #endregion 
    public virtual void TakeCriticalDamageAnimationEvent()
    {
        _characterStatsManager.TakeDamageNoAnimation(_characterManager.PendingCriticalDamage);
        _characterManager.PendingCriticalDamage = 0;
    }
}
