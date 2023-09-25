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

    private void OnAnimatorMove()
    {
        float delta = Time.deltaTime;
        _aiCharacterManager.EnemyRb.drag = 0;
        Vector3 deltaPosition = _aiCharacterManager.Animator.deltaPosition;
        deltaPosition.y = 0;
        Vector3 velocity = deltaPosition / delta;
        _aiCharacterManager.EnemyRb.velocity = velocity;

        if(_aiCharacterManager.IsRotatingWithRootMotion)
        {
            _aiCharacterManager.transform.rotation *= _aiCharacterManager.Animator.deltaRotation;
        }
    }
}
