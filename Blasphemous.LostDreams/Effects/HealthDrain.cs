using Framework.Managers;
using HarmonyLib;
using Tools.Level.Interactables;

namespace Blasphemous.LostDreams.Effects;

public class HealthDrain : IToggleEffect
{
    public bool IsActive => !IsUsingPrieDieu &&
        (Main.LostDreams.PenitenceHandler.IsActive("PE_LD01") || Main.LostDreams.ItemHandler.IsEquipped("RB551"));

    public static bool IsUsingPrieDieu { get; set; }

    public HealthDrain()
    {
        Main.LostDreams.EventHandler.OnEnemyDamaged += () => HealPlayer(HIT_HEAL_AMOUNT);
        Main.LostDreams.EventHandler.OnEnemyKilled += () => HealPlayer(KILL_HEAL_AMOUNT);
    }

    private void HealPlayer(float amount)
    {
        Core.Logic.Penitent.Stats.Life.Current += amount;
    }

    private const float HIT_HEAL_AMOUNT = 5f;
    private const float KILL_HEAL_AMOUNT = 15f;
}

// Control flag for when a prie dieu is in use
[HarmonyPatch(typeof(PrieDieu), "OnUpdate")]
class PrieDieu_Update_Patch
{
    public static void Postfix(PrieDieu __instance) => HealthDrain.IsUsingPrieDieu = __instance.BeingUsed;
}