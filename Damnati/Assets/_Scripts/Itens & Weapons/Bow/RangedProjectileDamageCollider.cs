using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedProjectileDamageCollider : DamageCollider
{
    public RangedAmmoItem AmmoItem;
    protected bool hasAlredyPenetratedASurface;
    protected GameObject penetratedProjectile;

    protected override void OnTriggerEnter(Collider collision) 
    {
        if(collision.tag == "Character")
        {
            shieldHasBeenHit = false;
            hasBeenParried = false;
             
            CharacterStatsManager enemyStats = collision.GetComponent<CharacterStatsManager>();
            CharacterManager enemyManager = collision.GetComponent<CharacterManager>();
            CharacterEffectsManager enemyEffects = collision.GetComponent<CharacterEffectsManager>();
            BlockingCollider shield = collision.transform.GetComponentInChildren<BlockingCollider>();
           
            if(enemyManager != null)
            {
                if(enemyStats.TeamIDNumber == TeamIDNumber)
                {
                    return;
                }

                CheckForParry(enemyManager);
                CheckForBlock(enemyManager, enemyStats, shield);
            }

            if(enemyStats != null)
            {
                if(enemyStats.TeamIDNumber == TeamIDNumber || hasBeenParried || shieldHasBeenHit)
                {
                    return;
                }

                enemyStats.PoiseResetTimer = enemyStats.TotalPoiseResetTime;
                enemyStats.TotalPoiseDefense = enemyStats.TotalPoiseDefense - PoiseBreak;

                //Detecta onde o colisor da arma fez o primeiro contato

                Vector3 contactPoint = collision.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
                float directionHitFrom = (Vector3.SignedAngle(characterManager.transform.forward, enemyManager.transform.forward, Vector3.up));
                ChooseWhichDirectionDamageCameFrom(directionHitFrom);
                enemyEffects.PlayerBloodSplatterFX(contactPoint);

                if(enemyStats.TotalPoiseDefense > PoiseBreak)
                {
                    enemyStats.TakeDamageNoAnimation(PhysicalDamage, 0);
                }
                else
                {
                    enemyStats.TakeDamage(PhysicalDamage, 0, currentDamageAnimation);
                }
            }
        }

        if(!hasAlredyPenetratedASurface && penetratedProjectile == null)
        {
            hasAlredyPenetratedASurface = true;

            Vector3 contactPoint = collision.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
            GameObject penetratedArrow = Instantiate(AmmoItem.penetradedModel, contactPoint, Quaternion.Euler(0, 0, 0));

            penetratedProjectile = penetratedArrow;
            penetratedArrow.transform.parent = collision.transform;
            penetratedArrow.transform.rotation = Quaternion.LookRotation(gameObject.transform.forward);
        }

        Destroy(transform.root.gameObject);
    }
}
