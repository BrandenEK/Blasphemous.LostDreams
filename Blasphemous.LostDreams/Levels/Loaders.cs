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
    public GameObject Result {get; private set; }

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

        Result = obj;
        yield break;
    }
}
