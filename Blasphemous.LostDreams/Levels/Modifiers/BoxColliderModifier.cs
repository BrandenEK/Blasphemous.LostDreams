using Blasphemous.Framework.Levels;
using UnityEngine;

namespace Blasphemous.LostDreams.Levels.Modifiers;

/// <summary>
/// A modifier that adds a box collider to an object
/// </summary>
internal class BoxColliderModifier : BaseColliderModifier
{
    private readonly Vector2 _size;

    /// <summary>
    /// Specify the layer, size, and offset of the collider
    /// </summary>
    public BoxColliderModifier(string layer, Vector2 size, Vector2 offset) : base(layer, offset)
    {
        _size = size;
    }

    /// <summary>
    /// Specify the size of the collider, offset set to (0, 0)
    /// </summary>
    public BoxColliderModifier(string layer, Vector2 size) : this(layer, size, new Vector2(0, 0)) { }

    public override void Apply(GameObject obj, ObjectData data)
    {
        base.Apply(obj, data);

        var collider = obj.AddComponent<BoxCollider2D>();
        collider.size = _size;
        collider.offset = _offset;
    }
}
