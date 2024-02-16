using Framework.Managers;
using Gameplay.GameControllers.Penitent.Effects;
using HarmonyLib;

namespace Blasphemous.LostDreams.Effects;

[HarmonyPatch(typeof(GuiltDropRecover), "GiveGuiltBonus")]
class GuiltDrop_GiveBonus_Patch
{
    public static void Postfix()
    {
        if (Main.LostDreams.ItemHandler.IsEquipped("QI502"))
        {
            Main.LostDreams.Log("QI502: Increasing guilt drop bonus");
            Core.Logic.Penitent.Stats.Life.SetToCurrentMax();
            Core.Logic.Penitent.Stats.Fervour.SetToCurrentMax();
        }
    }
}
