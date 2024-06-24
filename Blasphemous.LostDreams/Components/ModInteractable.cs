using Framework.Managers;
using HarmonyLib;
using UnityEngine;

namespace Blasphemous.LostDreams.Components;

public class ModInteractable : MonoBehaviour
{
    public void Interact()
    {
        Main.LostDreams.Log("Using interactable");
        Core.Logic.Penitent.Animator.SetBool("IS_DIALOGUE_MODE", true);

        if (Core.Dialog.StartConversation("DLG_LD_NPC01_00", true, false))
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
