using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorController : AnimatorManager
{
    private EnemyManager _enemyManager;
    private EnemyBossManager _enemyBossManager;
    private EnemyStats _enemyStats;
    private void Awake() 
    {
        Anim = GetComponent<Animator>();
        _enemyBossManager = GetComponent<EnemyBossManager>();
        _enemyManager = GetComponent<EnemyManager>();  
        _enemyStats = GetComponent<EnemyStats>();  
    }

    public override void TakeCriticalDamageAnimationEvent()
    {
        _enemyStats.TakeDamageNoAnimation(_enemyManager.PendingCriticalDamage);
        _enemyManager.PendingCriticalDamage = 0;
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
        _enemyManager.IsParrying = true;
    }
    public void DisableIsParrying()
    {
        _enemyManager.IsParrying = false;
    }
    public void EnableCanBeRiposted()
    {
        _enemyManager.CanBeRiposted = true;
    }
    public void DisableCanBeRiposted()
    {
        _enemyManager.CanBeRiposted = false;
    }

    #endregion 

    public void InstantiateBossParticleFX()
    {
        BossFXTransform bossFXTransform = GetComponentInChildren<BossFXTransform>();
        GameObject phaseFX = Instantiate(_enemyBossManager.ParticleFX, bossFXTransform.transform);
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
