using Blasphemous.Framework.Levels.Loaders;
using Blasphemous.LostDreams.Components;
using System.Collections;
using UnityEngine;

namespace Blasphemous.LostDreams.Levels.Loaders;

/// <summary>
/// Loads an interactable object
/// </summary>
internal class InteractableLoader : ILoader
{
    public GameObject Result { get; private set; }

    public IEnumerator Apply()
    {
        var loader = new SceneLoader("D05Z01S23_LOGIC", "LOGIC/ACT_CorpseDLC");
        yield return loader.Apply();

        var obj = loader.Result;
        var interactable = obj.transform.GetChild(2).gameObject;

        interactable.AddComponent<ModInteractable>();
        RemoveItemNeeded(interactable.GetComponent<CustomInteraction>());

        Object.Destroy(obj.transform.GetChild(1).gameObject);
        Object.Destroy(obj.transform.GetChild(0).gameObject);
        Object.Destroy(obj.GetComponent<PlayMakerFSM>());

        Result = obj;
        yield break;
    }

    private void RemoveItemNeeded(CustomInteraction interact)
    {
        interact.GetType().GetField("needObject", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(interact, false);
    }
}
