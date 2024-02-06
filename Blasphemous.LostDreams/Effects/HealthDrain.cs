using HarmonyLib;
using Tools.Level.Interactables;

namespace Blasphemous.LostDreams.Effects;

public class HealthDrain : IToggleEffect
{
    public bool IsActive => !IsUsingPrieDieu &&
        (Main.LostDreams.PenitenceHandler.IsActive("PE_LD01") || Main.LostDreams.ItemHandler.IsEquipped("RB551"));

    public static bool IsUsingPrieDieu { get; set; }
}

// Control flag for when a prie dieu is in use
[HarmonyPatch(typeof(PrieDieu), "OnUpdate")]
class PrieDieu_Update_Patch
{
    public static void Postfix(PrieDieu __instance) => HealthDrain.IsUsingPrieDieu = __instance.BeingUsed;
}