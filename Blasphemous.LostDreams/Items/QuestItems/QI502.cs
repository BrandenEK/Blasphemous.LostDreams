using Blasphemous.ModdingAPI;
using Framework.Managers;
using Gameplay.GameControllers.Penitent.Effects;
using HarmonyLib;

namespace Blasphemous.LostDreams.Items.QuestItems;

internal class QI502 : EffectOnAcquire { }

/// <summary>
/// Fully restore stats when picking up guilt
/// </summary>
[HarmonyPatch(typeof(GuiltDropRecover), "GiveGuiltBonus")]
class QI502_GuiltDrop_GiveBonus_Patch
{
    public static void Postfix()
    {
        if (!Main.LostDreams.QuestItemList.QI502.IsAcquired)
            return;

        ModLog.Info("QI502: Increasing guilt drop bonus");
        Core.Logic.Penitent.Stats.Life.SetToCurrentMax();
        Core.Logic.Penitent.Stats.Fervour.SetToCurrentMax();
    }
}