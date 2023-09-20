using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.TextCore.Text;

public class CharacterAnimatorManager : MonoBehaviour
{ 
    protected CharacterManager _character;

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
        _character = GetComponent<CharacterManager>();
        rigBuilder = GetComponent<RigBuilder>();
    }
    
    public void PlayTargetAnimation(string targetAnim, bool isInteracting, bool canRotate = false, bool mirrorAnim = false)
    {
        _character.Animator.applyRootMotion = isInteracting;
        _character.Animator.SetBool("CanRotate", canRotate);
        _character.Animator.SetBool("IsInteracting", isInteracting);
        _character.Animator.SetBool("IsMirrored", mirrorAnim);
        _character.Animator.CrossFade(targetAnim, 0.2f);
        Debug.Log("Target Animation: " + targetAnim + " Can Rotate Status: " + canRotate);
    }
    public void PlayerTargetAnimationWithRootRotation (string targetAnim, bool isInteracting)
    {
        _character.Animator.applyRootMotion = isInteracting;
        _character.Animator.SetBool("IsRotatingWithRootMotion", true);
        _character.Animator.SetBool("IsInteracting", isInteracting);
        _character.Animator.CrossFade(targetAnim, 0.2f);
    }

    #region Combat and Animation Events
    
    public virtual void CanRotate()
    {
        _character.Animator.SetBool("CanRotate", true);
    }
    public virtual void StopRotation()
    {
        _character.Animator.SetBool("CanRotate", false);
    }
    public virtual void EnableCombo()
    {
        _character.Animator.SetBool("CanDoCombo", true);
    }
    public virtual void DisableCombo()
    {
        _character.Animator.SetBool("CanDoCombo", false);
    }
    public virtual void EnableIsInvulnerable()
    {
        _character.Animator.SetBool("IsInvulnerable", true);
    }
    public virtual void DisableIsInvulnerable()
    {
        _character.Animator.SetBool("IsInvulnerable", false);
    }
    public virtual void EnableIsParrying()
    {
        _character.IsParrying = true;
    }
    public virtual void DisableIsParrying()
    {
        _character.IsParrying = false;
    }
    public virtual void EnableCanBeRiposted()
    {
        _character.CanBeRiposted = true;
    }
    public virtual void DisableCanBeRiposted()
    {
        _character.CanBeRiposted = false;
    }

    #endregion 
    public virtual void TakeCriticalDamageAnimationEvent()
    {
        _character.CharacterStats.TakeDamageNoAnimation(_character.PendingCriticalDamage, 0);
        _character.PendingCriticalDamage = 0;
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
        if(_character.IsInteracting)
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
