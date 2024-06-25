using Blasphemous.ModdingAPI;
using Framework.Managers;
using Gameplay.UI.Widgets;
using HarmonyLib;
using Tools.Playmaker2.Action;

namespace Blasphemous.LostDreams.Acquisition;

/// <summary>
/// After completing the game, give one of the ending reward items
/// </summary>
[HarmonyPatch(typeof(CutscenePlay), nameof(CutscenePlay.OnEnter))]
class Item_Cutscene_Patch
{
    public static void Postfix(CutscenePlay __instance)
    {
        string item = __instance.cutscene?.name switch
        {
            //"CTS10-EndingA" => "QI501",
            "CTS09-EndingB" => "QI502",
            //"CTS301-EndingC" => "QI503",
            _ => null
        };

        if (item == null)
            return;

        Main.LostDreams.AcquisitionHandler.GiveItem(item, true, true);
    }
}

/// <summary>
/// Display one of the ending reward items after the credits have finished
/// </summary>
[HarmonyPatch(typeof(CreditsWidget), "EndOfCredits")]
class Item_Credits_Patch
{
    public static void Postfix()
    {
        Main.LostDreams.AcquisitionHandler.DisplayQueuedItem();
    }
}

/// <summary>
/// When petting the dog, set a flag for the level editor
/// </summary>
[HarmonyPatch(typeof(InputManager), nameof(InputManager.SetBlocker))]
class Item_Dog_Patch
{
    public static void Postfix(string name)
    {
        if (name == "dog_block")
            Core.Events.SetFlag("PET_DOG", true);
    }
}

/// <summary>
/// When finishing Nacimiento dialog, give one of the items
/// </summary>
[HarmonyPatch(typeof(ShowMessage), nameof(ShowMessage.OnEnter))]
class ShowMessage_Enter_Patch
{
    public static bool Prefix(ShowMessage __instance)
    {
        if (__instance.textId == null || __instance.textId.Value != "MSG_10101")
            return true;

        string item = Core.Logic.Penitent.Stats.FlaskHealth.GetUpgrades() switch
        {
            1 => "RB510",
            2 => "RB511",
            3 => "RB512",
            4 => "RB513",
            5 => "RB514",
            _ => null
        };

        if (item == null)
            return true;

        ItemModder.AddAndDisplayItem(item);
        return false;
    }
}

//[HarmonyPatch(typeof(DialogStart), nameof(DialogStart.DialogEnded))]
//class DialogStart_End_Patch
//{
//    public static void Prefix(string id)
//    {
//    }
//}
