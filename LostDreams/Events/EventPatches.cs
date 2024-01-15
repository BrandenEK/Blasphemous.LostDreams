using Gameplay.GameControllers.Entities;
using Gameplay.GameControllers.Penitent.Damage;
using HarmonyLib;
using Tools.Level.Interactables;

namespace LostDreams.Events;

[HarmonyPatch(typeof(Entity), "KillInstanteneously")]
class Penitent_Death_Patch
{
    public static void Postfix(Entity __instance) => Main.LostDreams.EventHandler.KillEntity(__instance);
}

[HarmonyPatch(typeof(PrieDieu), "OnUse")]
class PrieDieu_Use_Patch
{
    public static void Prefix() => Main.LostDreams.EventHandler.UsePrieDieu();
}

[HarmonyPatch(typeof(PenitentDamageArea), "TakeDamage")]
public class Penitent_Damage_Patch
{
    public static void Postfix() => Main.LostDreams.EventHandler.DamagePlayer();
}