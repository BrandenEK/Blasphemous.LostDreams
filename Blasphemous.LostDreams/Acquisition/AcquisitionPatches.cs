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

        Main.Blasphemous.LostDreams.AcquisitionHandler.GiveItem(item, true, true);
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
        Main.Blasphemous.LostDreams.AcquisitionHandler.DisplayQueuedItem();
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
