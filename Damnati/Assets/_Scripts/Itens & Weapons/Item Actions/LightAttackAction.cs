using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item Actions/Light Attack Actions")]
public class LightAttackAction : ItemActions
{
    public override void PerformAction(PlayerManager player)
    {
        player.PlayerAnimator.EraseHandIKForWeapon();
        player.PlayerEffects.PlayWeaponFX(false);

        if(player.IsSprinting)
        {
            HandleRunningAttack(player);
            Debug.Log("correndo");
            return;
        }

        if(player.CanDoCombo)
        {
            player.PlayerInput.ComboFlag = true;
            Debug.Log("combo");
            HandleLightWeaponCombo(player);
            player.PlayerInput.ComboFlag = false;
        }

        else
        {
            if(player.IsInteracting || player.CanDoCombo)
            {
                return;
            }

            Debug.Log("ataque");
            HandleLightAttack(player);
        }
    }
    private void HandleLightAttack(PlayerManager player)
    {
        if(player.IsUsingLeftHand)
        {
            player.PlayerAnimator.PlayTargetAnimation(player.PlayerCombat.OH_Light_Attack_01, true, false, true);   
            player.PlayerCombat.LastAttack = player.PlayerCombat.OH_Light_Attack_01; 
        }
        else if(player.IsUsingRightHand)
        {
            if(player.PlayerInput.TwoHandFlag)
            {
                player.PlayerAnimator.PlayTargetAnimation(player.PlayerCombat.TH_Light_Attack_01, true);
                player.PlayerCombat.LastAttack = player.PlayerCombat.TH_Light_Attack_01;
            }
            else
            {
                player.PlayerAnimator.PlayTargetAnimation(player.PlayerCombat.OH_Light_Attack_01, true);
                player.PlayerCombat.LastAttack = player.PlayerCombat.OH_Light_Attack_01;
            }
        }
    }
    private void HandleRunningAttack(PlayerManager player)
    {
        if(player.IsUsingLeftHand)
        {
            player.PlayerAnimator.PlayTargetAnimation(player.PlayerCombat.OH_Running_Attack_01, true, false, true);
            player.PlayerCombat.LastAttack = player.PlayerCombat.OH_Running_Attack_01;
        }
        else if(player.IsUsingRightHand)
        {
            if(player.PlayerInput.TwoHandFlag)
            {
                player.PlayerAnimator.PlayTargetAnimation(player.PlayerCombat.TH_Running_Attack_01, true);
                player.PlayerCombat.LastAttack = player.PlayerCombat.TH_Running_Attack_01;
            }
            else
            {
                player.PlayerAnimator.PlayTargetAnimation(player.PlayerCombat.OH_Running_Attack_01, true);
                player.PlayerCombat.LastAttack = player.PlayerCombat.OH_Running_Attack_01;
            }
        }
    }
    public void HandleLightWeaponCombo(PlayerManager player)
    {
        if(player.PlayerAnimator.Anim.GetBool("IsInteracting") == true && player.PlayerAnimator.Anim.GetBool("CanDoCombo") == false)
        {
            return;
        }

        if(player.PlayerInput.ComboFlag)
        {
            player.PlayerAnimator.Anim.SetBool("CanDoCombo", false);
            
            if(player.IsUsingLeftHand)
            {
                if(player.PlayerCombat.LastAttack == player.PlayerCombat.OH_Light_Attack_01)
                {
                    player.PlayerAnimator.PlayTargetAnimation(player.PlayerCombat.OH_Light_Attack_02, true, false, true);
                    player.PlayerCombat.LastAttack = player.PlayerCombat.OH_Light_Attack_02;
                }
                else if(player.PlayerCombat.LastAttack == player.PlayerCombat.OH_Light_Attack_02)
                {
                    player.PlayerAnimator.PlayTargetAnimation(player.PlayerCombat.OH_Light_Attack_03, true, false, true);
                    player.PlayerCombat.LastAttack = player.PlayerCombat.OH_Light_Attack_03;
                }
                else if(player.PlayerCombat.LastAttack == player.PlayerCombat.OH_Light_Attack_03)
                {
                    player.PlayerAnimator.PlayTargetAnimation(player.PlayerCombat.OH_Light_Attack_01, true, false, true);
                    player.PlayerCombat.LastAttack = player.PlayerCombat.OH_Light_Attack_01;
                }
            }
            else if(player.IsUsingRightHand)
            {
                if(player.IsTwoHandingWeapon)
                {
                    if(player.PlayerCombat.LastAttack == player.PlayerCombat.TH_Light_Attack_01)
                    {
                        player.PlayerAnimator.PlayTargetAnimation(player.PlayerCombat.TH_Light_Attack_02, true);
                        player.PlayerCombat.LastAttack = player.PlayerCombat.TH_Light_Attack_02;
                    }
                    else if(player.PlayerCombat.LastAttack == player.PlayerCombat.TH_Light_Attack_02)
                    {
                        player.PlayerAnimator.PlayTargetAnimation(player.PlayerCombat.TH_Light_Attack_03, true);
                        player.PlayerCombat.LastAttack = player.PlayerCombat.TH_Light_Attack_03;
                    }
                    else if(player.PlayerCombat.LastAttack == player.PlayerCombat.TH_Light_Attack_03)
                    {
                        player.PlayerAnimator.PlayTargetAnimation(player.PlayerCombat.TH_Light_Attack_01, true);
                        player.PlayerCombat.LastAttack = player.PlayerCombat.TH_Light_Attack_01;
                    }
                }
                else
                {
                    if(player.PlayerCombat.LastAttack == player.PlayerCombat.OH_Light_Attack_01)
                    {
                        player.PlayerAnimator.PlayTargetAnimation(player.PlayerCombat.OH_Light_Attack_02, true);
                        player.PlayerCombat.LastAttack = player.PlayerCombat.OH_Light_Attack_02;
                    }
                    else if(player.PlayerCombat.LastAttack == player.PlayerCombat.OH_Light_Attack_02)
                    {
                        player.PlayerAnimator.PlayTargetAnimation(player.PlayerCombat.OH_Light_Attack_03, true);
                        player.PlayerCombat.LastAttack = player.PlayerCombat.OH_Light_Attack_03;
                    }
                    else if(player.PlayerCombat.LastAttack == player.PlayerCombat.OH_Light_Attack_03)
                    {
                        player.PlayerAnimator.PlayTargetAnimation(player.PlayerCombat.OH_Light_Attack_01, true);
                        player.PlayerCombat.LastAttack = player.PlayerCombat.OH_Light_Attack_01;
                    }
                }
            }
        }
    }
}
