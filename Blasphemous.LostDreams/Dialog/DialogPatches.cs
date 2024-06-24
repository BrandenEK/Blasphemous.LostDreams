using Blasphemous.ModdingAPI;
using Framework.Dialog;
using Framework.Managers;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;

namespace Blasphemous.LostDreams.Dialog;

//[HarmonyPatch(typeof(DialogManager), nameof(DialogManager.Start))]
//class DialogManager_Start_Patch
//{
//    public static void Postfix(Dictionary<string, DialogObject> ___allDialogs)
//    {
//        Main.LostDreams.LogWarning("Adding dialogs to dialogmanager");

//        foreach (DialogInfo info in Main.LostDreams.DialogStorage.All)
//        {
//            ___allDialogs.Add(info.Id, new DialogObject()
//            {
//                id = info.Id,
//                dialogType = info.Type,
//                dialogLines = info.TextLines.Select(Main.LostDreams.LocalizationHandler.Localize).ToList(),
//                answersLines = info.ResponseLines.Select(Main.LostDreams.LocalizationHandler.Localize).ToList(),
//                itemType = info.Item == null ? InventoryManager.ItemType.Bead : ItemModder.GetItemTypeFromId(info.Item),
//                item = info.Item,
//            });
//        }
//    }
//}

[HarmonyPatch(typeof(DialogManager), nameof(DialogManager.StartConversation))]
class DialogManager_StartConversation_Patch
{
    public static void Prefix(string conversiationId, Dictionary<string, DialogObject> ___allDialogs)
    {
        if (!Main.LostDreams.DialogStorage.TryGetValue(conversiationId, out DialogInfo info))
            return;

        ___allDialogs.Add(conversiationId, new DialogObject()
        {
            id = info.Id,
            dialogType = info.Type,
            dialogLines = info.TextLines.Select(Main.LostDreams.LocalizationHandler.Localize).ToList(),
            answersLines = info.ResponseLines.Select(Main.LostDreams.LocalizationHandler.Localize).ToList(),
            itemType = info.Item == null ? InventoryManager.ItemType.Bead : ItemModder.GetItemTypeFromId(info.Item),
            item = info.Item,
        });
    }

    public static void Postfix(string conversiationId, Dictionary<string, DialogObject> ___allDialogs)
    {
        if (!Main.LostDreams.DialogStorage.TryGetValue(conversiationId, out DialogInfo info))
            return;

        ___allDialogs.Remove(conversiationId);
    }
}