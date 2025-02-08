using Blasphemous.Framework.Levels;
using UnityEngine;

namespace Blasphemous.LostDreams.Levels.Modifiers;

/// <summary>
/// A modifier that adds a polygon collider to an object
/// </summary>
internal class PolygonColliderModifier : BaseColliderModifier
{
    private readonly Vector2[] _points;

    /// <summary>
    /// Specify the points and offset of the PolygonCollider
    /// </summary>
    public PolygonColliderModifier(string layer, Vector2[] points, Vector2 offset) : base(layer, offset)
    {
        _points = points;
    }

    /// <summary>
    /// Specify the points of the PolygonCollider, offset set to (0, 0)
    /// </summary>
    public PolygonColliderModifier(string layer, Vector2[] points) : this(layer, points, new Vector2(0, 0)) { }

    public override void Apply(GameObject obj, ObjectData data)
    {
        base.Apply(obj, data);

        var collider = obj.AddComponent<PolygonCollider2D>();
        collider.points = _points;
        collider.offset = _offset;
    }
}
