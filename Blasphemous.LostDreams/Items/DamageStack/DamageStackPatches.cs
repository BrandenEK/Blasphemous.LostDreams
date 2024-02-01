using Framework.Managers;
using Gameplay.GameControllers.Enemies.Framework.Damage;
using Gameplay.GameControllers.Entities;
using Gameplay.GameControllers.Penitent.Damage;
using HarmonyLib;

namespace Blasphemous.LostDreams.Items.DamageStack;

[HarmonyPatch(typeof(PenitentDamageArea), nameof(PenitentDamageArea.TakeDamage))]
public class Penitent_Damage_Patch
{
    [HarmonyPriority(Priority.High)]
    public static void Prefix(ref Hit hit)
    {
        if (Main.Blasphemous.LostDreams.EffectHandler.IsActive("damage-stack") && !Core.Logic.Penitent.Status.Unattacable)
        {
            hit.DamageAmount *= DamageStackEffect.CurrentMultiplier;
        }
    }
}

[HarmonyPatch(typeof(EnemyDamageArea), nameof(EnemyDamageArea.TakeDamage))]
public class Enemy_Damage_Patch
{
    [HarmonyPriority(Priority.High)]
    public static void Prefix(ref Hit hit)
    {
        if (Main.Blasphemous.LostDreams.EffectHandler.IsActive("damage-stack"))
        {
            hit.DamageAmount *= DamageStackEffect.CurrentMultiplier;
        }
    }
}
