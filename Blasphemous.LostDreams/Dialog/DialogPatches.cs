using Blasphemous.LostDreams.Extensions;
using Blasphemous.ModdingAPI.Helpers;
using Framework.Dialog;
using Framework.Managers;
using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace Blasphemous.LostDreams.Dialog;

/// <summary>
/// Adds the custom dialog when the conversation starts, and removes it at the end
/// </summary>
[HarmonyPatch(typeof(DialogManager), nameof(DialogManager.StartConversation))]
class DialogManager_StartConversation_Patch
{
    public static void Prefix(string conversiationId, Dictionary<string, DialogObject> ___allDialogs)
    {
        if (!Main.LostDreams.DialogStorage.TryGetValue(conversiationId, out DialogInfo info))
            return;

        DialogObject dialog = ScriptableObject.CreateInstance<DialogObject>();
        dialog.id = info.Id;
        dialog.dialogType = info.Type;
        dialog.dialogLines = Main.LostDreams.LocalizationHandler.LocalizeLines($"{info.Id}.text");
        dialog.answersLines = Main.LostDreams.LocalizationHandler.LocalizeLines($"{info.Id}.resp");
        dialog.itemType = info.Item == null ? InventoryManager.ItemType.Bead : ItemHelper.GetItemTypeFromId(info.Item);
        dialog.item = info.Item;

        ___allDialogs.Add(conversiationId, dialog);
    }

    public static void Postfix(string conversiationId, Dictionary<string, DialogObject> ___allDialogs)
    {
        if (!Main.LostDreams.DialogStorage.TryGetValue(conversiationId, out _))
            return;

        ___allDialogs.Remove(conversiationId);
    }
}