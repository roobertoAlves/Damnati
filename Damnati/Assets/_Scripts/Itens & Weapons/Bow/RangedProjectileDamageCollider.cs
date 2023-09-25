using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedProjectileDamageCollider : DamageCollider
{
    public RangedAmmoItem AmmoItem;
    protected bool hasAlredyPenetratedASurface;

    private Rigidbody _arrowRigidbody;
    private CapsuleCollider _arrowCapsuleCollider;

    protected override void Awake()
    {
        damageCollider = GetComponent<Collider>();
        damageCollider.gameObject.SetActive(true);
        damageCollider.enabled = true;
        _arrowCapsuleCollider = GetComponent<CapsuleCollider>();
        _arrowRigidbody = GetComponent<Rigidbody>();
    }
    private void OnCollisionEnter(Collision collision) 
    {
        shieldHasBeenHit = false;
        hasBeenParried = false;

        CharacterManager enemyManager = collision.gameObject.GetComponentInParent<CharacterManager>();
            
        if(enemyManager != null)
        {
            if(enemyManager.CharacterStats.TeamIDNumber == TeamIDNumber)
            {
                return;
            }

            CheckForParry(enemyManager);
            CheckForBlock(enemyManager);

            if(hasBeenParried || shieldHasBeenHit)
            {
                return;
            }

            enemyManager.CharacterStats.PoiseResetTimer = enemyManager.CharacterStats.TotalPoiseResetTime;
            enemyManager.CharacterStats.TotalPoiseDefense = enemyManager.CharacterStats.TotalPoiseDefense - PoiseBreak;

            //Detecta onde o colisor da arma fez o primeiro contato

            Vector3 contactPoint = collision.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
            float directionHitFrom = (Vector3.SignedAngle(characterManager.transform.forward, enemyManager.transform.forward, Vector3.up));
            ChooseWhichDirectionDamageCameFrom(directionHitFrom);
            enemyManager.CharacterEffects.PlayerBloodSplatterFX(contactPoint);

            if(enemyManager.CharacterStats.TotalPoiseDefense > PoiseBreak)
            {
                enemyManager.CharacterStats.TakeDamageNoAnimation(PhysicalDamage, 0);
            }
            else
            {
                enemyManager.CharacterStats.TakeDamage(PhysicalDamage, 0, currentDamageAnimation, characterManager);
            }
        }

        if(!hasAlredyPenetratedASurface)
        {
            hasAlredyPenetratedASurface = true;
            _arrowRigidbody.isKinematic = true;
            _arrowCapsuleCollider.enabled = false;

            gameObject.transform.position = collision.GetContact(0).point;
            gameObject.transform.rotation = Quaternion.LookRotation(transform.forward);
            gameObject.transform.parent = collision.collider.transform;
        }
    }

    private void FixedUpdate() 
    {
        if(_arrowRigidbody.velocity != Vector3.zero)
        {
            _arrowRigidbody.rotation = Quaternion.LookRotation(_arrowRigidbody.velocity);
        }    
    }
}
