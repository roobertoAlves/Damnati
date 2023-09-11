using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorManager : CharacterAnimatorManager
{
    private EnemyManager _enemyManager;
    private EnemyBossManager _enemyBossManager;
    private EnemyEffectsManager _enemyEffectsManager;


    protected override void Awake() 
    {
        base.Awake();
        Anim = GetComponent<Animator>();
        _enemyBossManager = GetComponent<EnemyBossManager>();
        _enemyManager = GetComponent<EnemyManager>();
        _enemyEffectsManager = GetComponent<EnemyEffectsManager>();   
    }
    
    public void InstantiateBossParticleFX()
    {
        BossFXTransform bossFXTransform = GetComponentInChildren<BossFXTransform>();
        GameObject phaseFX = Instantiate(_enemyBossManager.ParticleFX, bossFXTransform.transform);
    }

    public void PlayWeaponTrailFX()
    {
        _enemyEffectsManager.PlayWeaponFX(false);
    }

    private void OnAnimatorMove()
    {
        float delta = Time.deltaTime;
        _enemyManager.EnemyRb.drag = 0;
        Vector3 deltaPosition = Anim.deltaPosition;
        deltaPosition.y = 0;
        Vector3 velocity = deltaPosition / delta;
        _enemyManager.EnemyRb.velocity = velocity;

        if(_enemyManager.IsRotatingWithRootMotion)
        {
            _enemyManager.transform.rotation *= Anim.deltaRotation;
        }
    }
}
