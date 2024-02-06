using HarmonyLib;
using Tools.Level.Interactables;

namespace Blasphemous.LostDreams.Effects;

public class HealthDrain : IToggleEffect
{
    public bool IsActive => Main.LostDreams.PenitenceHandler.IsActive("PE_LD01") && !IsUsingPrieDieu;

    public static bool IsUsingPrieDieu { get; set; }
}

// Control flag for when a prie dieu is in use
[HarmonyPatch(typeof(PrieDieu), "OnUpdate")]
class PrieDieu_Update_Patch
{
    public static void Postfix(PrieDieu __instance) => HealthDrain.IsUsingPrieDieu = __instance.BeingUsed;
}