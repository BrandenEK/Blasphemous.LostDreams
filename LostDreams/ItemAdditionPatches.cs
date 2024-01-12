using Framework.Managers;
using Gameplay.UI.Widgets;
using HarmonyLib;
using ModdingAPI;
using Tools.Playmaker2.Action;

namespace LostDreams;

[HarmonyPatch(typeof(CutscenePlay), nameof(CutscenePlay.OnEnter))]
class Item_Cutscene_Patch
{
    public static void Postfix(CutscenePlay __instance)
    {
        string item = __instance.cutscene?.name switch
        {
            "CTS10-EndingA" => "QI501",
            "CTS09-EndingB" => "QI502",
            "CTS301-EndingC" => "QI503",
            _ => null
        };

        if (item == null)
            return;

        Main.LostDreams.QueueItem(item);
        Core.InventoryManager.AddQuestItem(item);
    }
}

[HarmonyPatch(typeof(CreditsWidget), "EndOfCredits")]
class Item_Credits_Patch
{
    public static void Postfix()
    {
        string queued = Main.LostDreams.PopQueuedItem();

        if (queued != string.Empty)
            Main.LostDreams.DisplayItem(queued);
    }
}
