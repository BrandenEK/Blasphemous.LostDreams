using Blasphemous.Framework.Levels;
using UnityEngine;

namespace Blasphemous.LostDreams.Levels.Modifiers;

/// <summary>
/// A modifier that adds a box collider to an object
/// </summary>
internal class BoxColliderModifier : BaseColliderModifier
{
    private Vector2 _size;

    /// <summary>
    /// Reads properties instead of using constructor to initialize the parameters
    /// </summary>
    public BoxColliderModifier() : base() { }

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

        if (_useProperties)
        {
            _size = Main.StringToVector3(data.properties[1]);
            _offset = Main.StringToVector3(data.properties[2]);
        }

        var collider = obj.AddComponent<BoxCollider2D>();
        collider.size = _size;
        collider.offset = _offset;
    }
}
