using Blasphemous.Framework.Levels.Loaders;
using Blasphemous.LostDreams.Components;
using System.Collections;
using UnityEngine;

namespace Blasphemous.LostDreams.Levels;

public class EmptyLoader(string name) : ILoader
{
    public GameObject Result { get; private set; }

    public IEnumerator Apply()
    {
        Result = new GameObject(name);
        yield break;
    }
}

public class NpcLoader : ILoader
{
    public GameObject Result { get; private set; }

    public IEnumerator Apply()
    {
        GameObject obj = new("NPC");

        var sr = obj.AddComponent<SpriteRenderer>();
        sr.sortingLayerName = "Player";
        sr.sortingOrder = -1000;

        obj.AddComponent<ModAnimator>();
        obj.AddComponent<Animator>();
        obj.AddComponent<BoxCollider2D>();
        obj.AddComponent<ModDamageArea>();

        var loader = new InteractableLoader();
        yield return loader.Apply();

        var interactableObject = loader.Result;
        obj.transform.SetParent(interactableObject.transform);
        obj.transform.localPosition = Vector3.zero;

        Result = interactableObject;
        yield break;
    }
}

public class InteractableLoader : ILoader
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
