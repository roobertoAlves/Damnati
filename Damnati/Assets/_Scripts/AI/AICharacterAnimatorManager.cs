using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICharacterAnimatorManager : CharacterAnimatorManager
{
    private AICharacterManager _aiCharacterManager;
    protected override void Awake() 
    {
        base.Awake();
        _aiCharacterManager = GetComponent<AICharacterManager>();
    }
    
    public void InstantiateBossParticleFX()
    {
        BossFXTransform bossFXTransform = GetComponentInChildren<BossFXTransform>();
        GameObject phaseFX = Instantiate(_aiCharacterManager.EnemyBossManager.ParticleFX, bossFXTransform.transform);
    }

    public void PlayWeaponTrailFX()
    {
        _aiCharacterManager.AICharacterEffectsManager.PlayWeaponFX(false);
    }

    public override void OnAnimatorMove()
    {
        if(_character.IsInteracting)
        {
            return;
        }

        Vector3 velocity = _character.Animator.deltaPosition;
        _character.CharacterController.Move(velocity);

        if(_aiCharacterManager.IsRotatingWithRootMotion)
        {
            _character.transform.rotation *= _character.Animator.deltaRotation;
        }
    }
}
