using Framework.Managers;
using Gameplay.GameControllers.Effects.Player.Healing;
using Gameplay.GameControllers.Entities;
using Gameplay.GameControllers.Penitent;
using Gameplay.GameControllers.Penitent.Damage;
using HarmonyLib;
using System.Collections;
using Tools.Level.Interactables;
using UnityEngine;

namespace LostDreams.DamageRemovalOnce;

[HarmonyPatch(typeof(Entity), "KillInstanteneously")]
class Penitent_Death_Patch
{
    public static void Postfix(Entity __instance)
    {
        if (__instance is Penitent)
        {
            Main.LostDreams.Log("RB503: Regain damage removal (death)");
            DamageRemovalEffect.RegainDamageRemoval();
        }
    }
}

[HarmonyPatch(typeof(PrieDieu), "OnUse")]
class PrieDieu_Use_Patch
{
    public static void Prefix()
    {
        Main.LostDreams.Log("RB503: Regain damage removal (priedieu)");
        DamageRemovalEffect.RegainDamageRemoval();
    }
}

[HarmonyPatch(typeof(PenitentDamageArea), "TakeDamage")]
public class Penitent_Damage_Patch
{
    public static void Prefix(ref Hit hit)
    {
        if (Main.LostDreams.EffectHandler.IsActive("damage-removal") && DamageRemovalEffect.WillRemoveDamage)
        {
            Main.LostDreams.Log("RB503: Removing all damage");
            hit.DamageAmount = 0;
            DamageRemovalEffect.UseDamageRemoval();

            DamageRemovalEffect.HealingFlag = true;
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
        if (DamageRemovalEffect.HealingFlag)
        {
            DamageRemovalEffect.HealingFlag = false;
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
}
