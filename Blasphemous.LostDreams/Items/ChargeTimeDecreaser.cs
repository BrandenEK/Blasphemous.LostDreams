using Gameplay.GameControllers.AnimationBehaviours.Player.Attack;
using Gameplay.GameControllers.Penitent.Abilities;
using HarmonyLib;
using UnityEngine;

namespace Blasphemous.LostDreams.Items;

public static class ChargeTimeDecreaser
{
    public static bool IsActive => Main.LostDreams.EffectHandler.IsActive("RB501");
}

/// <summary>
/// Reduce how much time to activate charge
/// </summary>
[HarmonyPatch(typeof(ChargedAttack), "SetChargingTimeByTier")]
class ChargedAttack_SetTimer_Patch
{
    public static void Postfix(ref float ____currentChargingTime)
    {
        // tier 1 = 1.5, tier 2 = 0.75
        if (ChargeTimeDecreaser.IsActive)
        {
            Main.LostDreams.Log("RB501: Reducing charge time");
            ____currentChargingTime = 0.05f;
        }
    }
}

/// <summary>
/// Increase speed of charging animation
/// </summary>
[HarmonyPatch(typeof(ChargedAttackBehaviour), "OnStateEnter")]
class ChargedAnimation_Enter_Patch
{
    public static void Postfix(Animator animator)
    {
        if (ChargeTimeDecreaser.IsActive)
        {
            animator.speed = 4;
        }
    }
}
[HarmonyPatch(typeof(ChargedAttackBehaviour), "OnStateExit")]
class ChargedAnimation_Exit_Patch
{
    public static void Postfix(Animator animator)
    {
        if (ChargeTimeDecreaser.IsActive)
        {
            animator.speed = 1;
        }
    }
}