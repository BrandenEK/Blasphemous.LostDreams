using Framework.Dialog;
using Framework.Managers;
using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace Blasphemous.LostDreams.Components;

public class ModInteractable : MonoBehaviour
{
    public void Interact()
    {
        Main.LostDreams.Log("Using interactable");
        Core.Logic.Penitent.Animator.SetBool("IS_DIALOGUE_MODE", true);

        if (Core.Dialog.StartConversation("DLG_LD_001", true, false))
        {
            Core.Dialog.OnDialogFinished += OnDialogEnd;
        }
    }

    private void OnDialogEnd(string id, int response)
    {
        Core.Logic.Penitent.Animator.SetBool("IS_DIALOGUE_MODE", false);
    }
}

[HarmonyPatch(typeof(CustomInteraction), "OnUse")]
class Interaction_Use_Patch
{
    public static void Postfix(CustomInteraction __instance)
    {
        ModInteractable interactable = __instance.GetComponent<ModInteractable>();
        if (interactable != null)
            interactable.Interact();
    }
}

[HarmonyPatch(typeof(DialogManager), nameof(DialogManager.Start))]
class t
{
    public static void Postfix(Dictionary<string, DialogObject> ___allDialogs)
    {
        Main.LostDreams.LogWarning("Adding more dialogs!");

        DialogObject test = new()
        {
            id = "DLG_LD_001",
            dialogType = DialogObject.DialogType.Lines,
            dialogLines = new List<string>()
            {
                "This is example text line one",
                "Now this is the seconds line..."
            }
        };

        ___allDialogs.Add(test.id, test);
    }
}