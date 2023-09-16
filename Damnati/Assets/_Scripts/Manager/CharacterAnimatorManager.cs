using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.TextCore.Text;

public class CharacterAnimatorManager : MonoBehaviour
{ 
    public Animator Anim;
    protected CharacterManager _characterManager;
    protected CharacterStatsManager _characterStatsManager;
    public bool canRotate;  

    protected RigBuilder rigBuilder;
    [SerializeField] private TwoBoneIKConstraint _leftHandConstraint;
    [SerializeField] private TwoBoneIKConstraint _rightHandConstrait;
    private bool _handIKWeightsReset = false;

    #region GET & SET

    public TwoBoneIKConstraint LeftHandConstraint { get { return _leftHandConstraint; } set { _leftHandConstraint = value; }}
    public TwoBoneIKConstraint RightHandConstraint { get { return _rightHandConstrait; } set { _rightHandConstrait = value; }}

    #endregion
    protected virtual void Awake()
    {
        _characterManager = GetComponent<CharacterManager>();
        _characterStatsManager = GetComponent<CharacterStatsManager>();
        rigBuilder = GetComponent<RigBuilder>();
    }
    
    public void PlayTargetAnimation(string targetAnim, bool isInteracting, bool canRotate = false, bool mirrorAnim = false)
    {
        Anim.applyRootMotion = isInteracting;
        Anim.SetBool("CanRotate", canRotate);
        Anim.SetBool("IsInteracting", isInteracting);
        Anim.SetBool("IsMirrored", mirrorAnim);
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
        _characterStatsManager.TakeDamageNoAnimation(_characterManager.PendingCriticalDamage, 0);
        _characterManager.PendingCriticalDamage = 0;
    }
    
    public virtual void SetHandIKForWeapon(RightHandIKTarget rightHandTarget, LeftHandIKTarget leftHandTarget, bool isTwoHandingWeapon)
    {
        if(isTwoHandingWeapon)
        {
            if(rightHandTarget != null)
            {
                _rightHandConstrait.data.target = rightHandTarget.transform;
                _rightHandConstrait.data.targetPositionWeight = 1;
                _rightHandConstrait.data.targetRotationWeight = 1;
            }
            if(leftHandTarget != null)
            {
                _leftHandConstraint.data.target = leftHandTarget.transform;
                _leftHandConstraint.data.targetPositionWeight = 1;
                _leftHandConstraint.data.targetRotationWeight = 1;
            }
        }
        else
        {
            _rightHandConstrait.data.target = null;
            _leftHandConstraint.data.target = null;
        }

        rigBuilder.Build();
    }
    public virtual void CheckHandIKWeight(RightHandIKTarget rightHandIK, LeftHandIKTarget leftHandIK, bool isTwoHandingWeapon)
    {
        if(_characterManager.IsInteracting)
        {
            return;
        }

        if(_handIKWeightsReset)
        {
            _handIKWeightsReset = false;

            if(_rightHandConstrait.data.target != null)
            {
                Debug.Log("Yeet");

                _rightHandConstrait.data.target = rightHandIK.transform;
                _rightHandConstrait.data.targetPositionWeight = 1;
                _rightHandConstrait.data.targetRotationWeight = 1;
            }
            if(_leftHandConstraint.data.target != null)
            {
                _leftHandConstraint.data.target = leftHandIK.transform;
                _leftHandConstraint.data.targetPositionWeight = 1;
                _leftHandConstraint.data.targetRotationWeight = 1;
            }
        }
    }
    public virtual void EraseHandIKForWeapon()
    {
        _handIKWeightsReset = true;

        if(_rightHandConstrait.data.target != null)
        {
            _rightHandConstrait.data.targetPositionWeight = 0;
            _rightHandConstrait.data.targetRotationWeight = 0;
        }
        if(_leftHandConstraint.data.target != null)
        {
            _leftHandConstraint.data.targetPositionWeight = 0;
            _leftHandConstraint.data.targetRotationWeight = 0;
        }
    }
}
