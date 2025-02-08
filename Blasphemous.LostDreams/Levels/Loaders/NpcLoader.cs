using Blasphemous.Framework.Levels.Loaders;
using Blasphemous.LostDreams.Components;
using System.Collections;
using UnityEngine;

namespace Blasphemous.LostDreams.Levels.Loaders;

/// <summary>
/// Loads an interactable NPC object
/// </summary>
internal class NpcLoader : ILoader
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
