using Blasphemous.Framework.Levels;
using Blasphemous.Framework.Levels.Modifiers;
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
    protected string _layer;

    /// <summary>
    /// The offset of the collider to the attached GameObject
    /// </summary>
    protected Vector2 _offset;

    /// <summary>
    /// Whether the collider uses properties instead of constructor to initialize itself
    /// </summary>
    protected readonly bool _useProperties = false;

    /// <summary>
    /// Reads properties instead of using constructor to initialize the parameters
    /// </summary>
    public BaseColliderModifier()
    {
        _useProperties = true;
    }

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
        if (_useProperties)
        {
            _layer = data.properties[0];
        }

        obj.name = $"Collider[{data.id}]";
        obj.layer = LayerMask.NameToLayer(_layer);
    }


}
