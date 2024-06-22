using Blasphemous.Framework.Levels;
using Blasphemous.Framework.Levels.Modifiers;
using Blasphemous.LostDreams.Animation;
using UnityEngine;

namespace Blasphemous.LostDreams.Levels;

/// <summary>
/// A modifier that adds a collider to an empty object
/// </summary>
public class ColliderModifier : IModifier
{
    private readonly string _name;
    private readonly Vector2 _size;

    /// <summary>
    /// Specify the name and size of the collider
    /// </summary>
    public ColliderModifier(string name, Vector2 size)
    {
        _name = name;
        _size = size;
    }

    /// <summary>
    /// Adds a collider component and sets the layer to floor
    /// </summary>
    public void Apply(GameObject obj, ObjectData data)
    {
        obj.name = _name;
        obj.layer = LayerMask.NameToLayer("Floor");

        var collider = obj.AddComponent<BoxCollider2D>();
        collider.size = _size;
    }
}


public class NpcModifier : IModifier
{
    public void Apply(GameObject obj, ObjectData data)
    {
        var sr = obj.AddComponent<SpriteRenderer>();
        sr.sortingLayerName = "After Player";

        var anim = obj.AddComponent<ModAnimator>();
        anim.Animation = Main.LostDreams.AnimationLoader[data.properties[0]];
    }
}
