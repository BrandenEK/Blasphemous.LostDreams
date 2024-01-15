using Framework.Managers;
using Gameplay.GameControllers.Effects.Player.Healing;
using Gameplay.GameControllers.Entities;
using Gameplay.GameControllers.Penitent.Damage;
using HarmonyLib;
using System.Collections;
using UnityEngine;

namespace LostDreams.DamageRemoval;

[HarmonyPatch(typeof(PenitentDamageArea), nameof(PenitentDamageArea.TakeDamage))]
public class Penitent_Damage_Patch
{
    [HarmonyPriority(Priority.High)]
    public static void Prefix(ref Hit hit)
    {
        if (Main.LostDreams.EffectHandler.IsActive("damage-removal") && !Core.Logic.Penitent.Status.Unattacable)
        {
            Main.LostDreams.Log("RB503: Preventing damage");
            hit.DamageAmount = 0;

            Healing_Start_Patch.HealingFlag = true;
            Object.FindObjectOfType<HealingAura>()?.StartAura(Core.Logic.Penitent.Status.Orientation);
            Core.Logic.Penitent.Audio.PrayerInvincibility();
        }
    }
}

[HarmonyPatch(typeof(HealingAura), "StartAura")]
class Healing_Start_Patch
{
    public static void Postfix(HealingAura __instance, Animator ____auraAnimator, SpriteRenderer ____auraRenderer)
    {
        if (HealingFlag)
        {
            HealingFlag = false;
            ____auraRenderer.color = new Color(0.139f, 0.459f, 0.557f);
            ____auraAnimator.Play(0, 0, 0.28f);
            __instance.StartCoroutine(TurnOffHealing());
        }

        IEnumerator TurnOffHealing()
        {
            yield return new WaitForSeconds(1.8f);
            __instance.StopAura();
            yield return new WaitForEndOfFrame();
            ____auraRenderer.color = Color.white;
        }
    }

    public static bool HealingFlag { get; set; }
}
