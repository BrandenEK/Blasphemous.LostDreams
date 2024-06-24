using Framework.Managers;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Blasphemous.LostDreams.Components;

public class ModInteractable : MonoBehaviour
{
    public IEnumerable<string> Dialogs { get; set; }

    public void Interact()
    {
        Main.LostDreams.Log("Using interactable");
        Core.Logic.Penitent.Animator.SetBool("IS_DIALOGUE_MODE", true);

        if (Core.Dialog.StartConversation(GetDialogToUse(), true, false))
        {
            Core.Dialog.OnDialogFinished += OnDialogEnd;
        }
    }

    private void OnDialogEnd(string id, int response)
    {
        Core.Logic.Penitent.Animator.SetBool("IS_DIALOGUE_MODE", false);
    }

    private string GetDialogToUse()
    {
        if (Dialogs == null || !Dialogs.Any())
            throw new System.Exception("ModInteractable has no dialog ids applied through the level editor");

        foreach (string text in Dialogs)
        {
            // If no condition, return this one immediately
            if (!text.Contains(':'))
                return text.Trim();

            // If there is a condition, check for it and maybe return
            string[] parts = text.Split(':');
            if (Core.Events.GetFlag(parts[0].Trim()))
                return parts[1].Trim();
        }

        return Dialogs.Last();
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
