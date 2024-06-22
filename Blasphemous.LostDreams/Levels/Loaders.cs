using Blasphemous.Framework.Levels.Loaders;
using Blasphemous.LostDreams.Components;
using Gameplay.GameControllers.Entities;
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

        var anim = obj.AddComponent<ModAnimator>();
        var animator = obj.AddComponent<Animator>();
        var collider = obj.AddComponent<BoxCollider2D>();
        var damagearea = obj.AddComponent<ModDamageArea>();
        var entity = obj.AddComponent<Entity>();
        entity.Status.CastShadow = true;
        entity.Status.IsGrounded = true;
        var shadow = obj.AddComponent<EntityShadow>();

        Result = obj;
        yield break;
    }
}
