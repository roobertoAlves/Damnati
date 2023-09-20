using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item Actions/Fire Arrow Action")]
public class FireArrowAction : ItemActions
{
    public override void PerformAction(PlayerManager player)
    {
        //cria o local de instancia da flecha
        ArrowInstantiationLocation arrowInstantiationLocation;
        arrowInstantiationLocation = player.PlayerWeaponSlot.RightHandSlot.GetComponentInChildren<ArrowInstantiationLocation>();

        //Anima o arco atirando a flecha
        Animator bowAnimator = player.PlayerWeaponSlot.RightHandSlot.GetComponentInChildren<Animator>();
        bowAnimator.SetBool("IsDrawn", false);
        bowAnimator.Play("Bow Fire");
        Destroy(player.PlayerEffects.CurrentRangeFX); //Destroi o modelo da flecha carregada

        //reseta o player segurando a flecha
        player.PlayerAnimator.PlayTargetAnimation("Bow Fire", true);
        player.Animator.SetBool("IsHoldingArrow", false);

        //Criando e atirando a flecha

        GameObject liveArrow = Instantiate(player.PlayerInventory.currentAmmo.liveAmmoModel, arrowInstantiationLocation.transform.position, player.PlayerCamera.CameraPivotTransform.rotation);
        Rigidbody rb = liveArrow.GetComponentInChildren<Rigidbody>();
        RangedProjectileDamageCollider damageCollider = liveArrow.GetComponentInChildren<RangedProjectileDamageCollider>();

        if(player.IsAiming)
        {
            Ray ray = player.PlayerCamera.CameraObject.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hitPoint;

            if(Physics.Raycast(ray, out hitPoint, 100.0f))
            {
                liveArrow.transform.LookAt(hitPoint.point);
                Debug.Log(hitPoint.transform.name);
            }
            else
            {
                liveArrow.transform.rotation = Quaternion.Euler(player.PlayerCamera.CameraTransform.localEulerAngles.x, player.LockOnTransform.eulerAngles.y, 0);

            }
        }
        else
        {
            
            //Dando velocidade a flecha

            if(player.PlayerCamera.CurrentLockOnTarget != null)
            {

                //Enquanto "lockado" sempre irá olhar para o inimigo, podendo copiar a direção que a flecha tomar quando disparada

                Quaternion arrowRotation = Quaternion.LookRotation(player.PlayerCamera.CurrentLockOnTarget.LockOnTransform.position - liveArrow.gameObject.transform.position);
                liveArrow.transform.rotation = arrowRotation;
            }
            else
            {
                liveArrow.transform.rotation = Quaternion.Euler(player.PlayerCamera.CameraPivotTransform.eulerAngles.x, player.LockOnTransform.eulerAngles.y, 0);
            }
        }

        rb.AddForce(liveArrow.transform.forward * player.PlayerInventory.currentAmmo.forwardVelocity);
        rb.AddForce(liveArrow.transform.up * player.PlayerInventory.currentAmmo.upwardVelocity);
        rb.useGravity = player.PlayerInventory.currentAmmo.useGravity;
        rb.mass = player.PlayerInventory.currentAmmo.ammoMass;
        liveArrow.transform.parent = null;

        //fazendo que a flecha inflija dano
        damageCollider.characterManager = player;
        damageCollider.AmmoItem = player.PlayerInventory.currentAmmo;
        damageCollider.PhysicalDamage = player.PlayerInventory.currentAmmo.physicalDamage;
    }
}
