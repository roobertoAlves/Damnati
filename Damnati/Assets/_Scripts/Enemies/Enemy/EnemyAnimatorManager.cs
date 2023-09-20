using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorManager : CharacterAnimatorManager
{
    private EnemyManager _enemy;
    protected override void Awake() 
    {
        base.Awake();
        _enemy = GetComponent<EnemyManager>();
    }
    
    public void InstantiateBossParticleFX()
    {
        BossFXTransform bossFXTransform = GetComponentInChildren<BossFXTransform>();
        GameObject phaseFX = Instantiate(_enemy.EnemyBossManager.ParticleFX, bossFXTransform.transform);
    }

    public void PlayWeaponTrailFX()
    {
        _enemy.EnemyEffectsManager.PlayWeaponFX(false);
    }

    private void OnAnimatorMove()
    {
        float delta = Time.deltaTime;
        _enemy.EnemyRb.drag = 0;
        Vector3 deltaPosition = _enemy.Animator.deltaPosition;
        deltaPosition.y = 0;
        Vector3 velocity = deltaPosition / delta;
        _enemy.EnemyRb.velocity = velocity;

        if(_enemy.IsRotatingWithRootMotion)
        {
            _enemy.transform.rotation *= _enemy.Animator.deltaRotation;
        }
    }
}
