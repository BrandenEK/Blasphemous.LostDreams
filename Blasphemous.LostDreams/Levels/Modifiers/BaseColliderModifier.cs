using Blasphemous.Framework.Levels.Modifiers;
using Blasphemous.Framework.Levels;
using UnityEngine;

namespace Blasphemous.LostDreams.Levels.Modifiers;

/// <summary>
/// Abstract class of all types of collider modifiers
/// </summary>
internal abstract class BaseColliderModifier : IModifier
{
    /// <summary>
    /// The layer of the collider. 
    /// For solid collider, choose layer "Floor". 
    /// For droppable platforms, choose layer "OneWayDown"
    /// </summary>
    protected readonly string _layer;

    /// <summary>
    /// The offset of the collider to the attached GameObject
    /// </summary>
    protected readonly Vector2 _offset;

    /// <summary>
    /// Abstract constructor, specifying the layer and offset of the collider
    /// </summary>
    public BaseColliderModifier(string layer, Vector2 offset)
    {
        _layer = layer;
        _offset = offset;
    }

    /// <summary>
    /// Adds a collider component and sets the layer
    /// </summary>
    public virtual void Apply(GameObject obj, ObjectData data)
    {
        obj.name = $"Collider[{data.id}]";
        obj.layer = LayerMask.NameToLayer(_layer);
    }
}
