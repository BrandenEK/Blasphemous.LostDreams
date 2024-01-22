using Framework.Inventory;
using Framework.Managers;
using Gameplay.GameControllers.Penitent.Abilities;
using HarmonyLib;

namespace LostDreams.TarantoBuff;

[HarmonyPatch(typeof(PrayerUse), "StartUsingPrayer")]
class Prayer_Start_Patch
{
    public static void Postfix(Prayer prayer)
    {
        if (!Main.LostDreams.EffectHandler.IsActive("taranto-buff") || prayer == null || prayer.name != "PR09")
            return;

        Main.LostDreams.Log("RB504: Buffing Taranto prayer");
        Core.InventoryManager.GetPrayer("PR07").Use();
        //Core.InventoryManager.GetPrayer("PR08").Use();
    }
}
