using Framework.Managers;
using Gameplay.GameControllers.Penitent.Effects;
using HarmonyLib;

namespace LostDreams.GuiltFragment;

[HarmonyPatch(typeof(GuiltDropRecover), "GiveGuiltBonus")]
class GuiltDrop_GiveBonus_Patch
{
    public static void Postfix()
    {
        if (Main.LostDreams.EffectHandler.IsActive("guilt-fragment"))
        {
            Main.LostDreams.Log("QI502: Increasing guilt drop bonus");
            Core.Logic.Penitent.Stats.Life.SetToCurrentMax();
            Core.Logic.Penitent.Stats.Fervour.SetToCurrentMax();
        }
    }
}
