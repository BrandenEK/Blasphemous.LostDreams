using Framework.Managers;
using Gameplay.GameControllers.Effects.Player.Healing;
using Gameplay.GameControllers.Entities;
using HarmonyLib;
using System.Collections;
using UnityEngine;

namespace Blasphemous.LostDreams.Items.RosaryBeads;

internal class RB503 : EffectOnEquip
{
    private bool _alreadyUsed = false;

    public RB503()
    {
        Main.LostDreams.EventHandler.OnPlayerKilled += RegainDamageRemoval;
        Main.LostDreams.EventHandler.OnUsePrieDieu += RegainDamageRemoval;
        Main.LostDreams.EventHandler.OnExitGame += RegainDamageRemoval;
        Main.LostDreams.EventHandler.OnPlayerDamaged += PlayerTakeDamage;
    }

    private void RegainDamageRemoval()
    {
        _alreadyUsed = false;
    }

    private void PlayerTakeDamage(ref Hit hit)
    {
        if (_alreadyUsed || !IsEquipped)
            return;

        Main.LostDreams.Log("RB503: Preventing damage");
        hit.DamageAmount = 0;
        _alreadyUsed = true;

        RB503_Healing_Start_Patch.HealingFlag = true;
        Object.FindObjectOfType<HealingAura>()?.StartAura(Core.Logic.Penitent.Status.Orientation);
        Core.Logic.Penitent.Audio.PrayerInvincibility();
    }
}

/// <summary>
/// Show blue aura when damage is prevented
/// </summary>
[HarmonyPatch(typeof(HealingAura), "StartAura")]
class RB503_Healing_Start_Patch
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