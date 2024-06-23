using Framework.Managers;
using HarmonyLib;
using UnityEngine;

namespace Blasphemous.LostDreams.Components;

public class ModInteractable : MonoBehaviour
{
    public void Interact()
    {
        Main.LostDreams.Log("Using interactable");
        Core.Dialog.StartConversation("DLG_2034", false, false);
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