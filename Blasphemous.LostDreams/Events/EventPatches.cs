using Gameplay.GameControllers.Enemies.Framework.Damage;
using Gameplay.GameControllers.Entities;
using Gameplay.GameControllers.Penitent.Abilities;
using Gameplay.GameControllers.Penitent.Animator;
using Gameplay.GameControllers.Penitent.Attack;
using Gameplay.GameControllers.Penitent.Damage;
using HarmonyLib;
using Tools.Level.Interactables;

namespace Blasphemous.LostDreams.Events;

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

[HarmonyPatch(typeof(PenitentDamageArea), "RaiseDamageEvent")]
class Penitent_Damage_Patch
{
    public static void Prefix(ref Hit hit) => Main.LostDreams.EventHandler.DamagePlayer(ref hit);
}

[HarmonyPatch(typeof(EnemyDamageArea), "TakeDamageAmount")]
class Enemy_Damage_Patch
{
    public static void Prefix(ref Hit hit) => Main.LostDreams.EventHandler.DamageEnemy(ref hit);
}

[HarmonyPatch(typeof(Healing), "Heal")]
class Heal_Start_Patch
{
    public static bool Prefix() => !Main.LostDreams.EventHandler.UseFlask();
}

[HarmonyPatch(typeof(PenitentAttackAnimations), "SetSwordSlash")]
class PenitentAttackAnimations_SetSwordSlash_AttackStart_Patch
{
    public static void Prefix(PenitentSword.AttackType type)
    {
        Main.LostDreams.EventHandler.SwordAttack(type);
    }
}