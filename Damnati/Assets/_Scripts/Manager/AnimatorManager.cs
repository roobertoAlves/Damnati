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
    
    public void CanRotate()
    {
        Anim.SetBool("CanRotate", true);
    }
    public void StopRotation()
    {
        Anim.SetBool("CanRotate", false);
    }
    public void EnableCombo()
    {
        Anim.SetBool("CanDoCombo", true);
    }
    public void DisableCombo()
    {
        Anim.SetBool("CanDoCombo", false);
    }
    public void EnableIsInvulnerable()
    {
        Anim.SetBool("IsInvulnerable", true);
    }
    public void DisableIsInvulnerable()
    {
        Anim.SetBool("IsInvulnerable", false);
    }
    public void EnableIsParrying()
    {
        _characterManager.IsParrying = true;
    }
    public void DisableIsParrying()
    {
        _characterManager.IsParrying = false;
    }
    public void EnableCanBeRiposted()
    {
        _characterManager.CanBeRiposted = true;
    }
    public void DisableCanBeRiposted()
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
