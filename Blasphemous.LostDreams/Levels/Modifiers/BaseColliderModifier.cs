using Blasphemous.Framework.Levels.Modifiers;
using Blasphemous.Framework.Levels;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Blasphemous.ModdingAPI;

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

    /// <summary>
    /// Generate components of a colored mesh of the given color
    /// </summary>
    protected void GenerateColoredMesh(
        Color color,
        ref GameObject obj,
        out Mesh mesh,
        out MeshFilter meshFilter,
        out MeshRenderer meshRenderer)
    {
        // Create a mesh and set up the mesh filter and renderer
        meshFilter = obj.AddComponent<MeshFilter>();
        meshRenderer = obj.AddComponent<MeshRenderer>();

        // Create a mesh for the polygon
        mesh = new Mesh();
        meshFilter.mesh = mesh;

        // Create a material and set a color
        Material material = new Material(Shader.Find("Sprites/Default"));
        material.color = color;

        // Assign the material to the MeshRenderer
        meshRenderer.material = material;
    }

    /// <summary>
    /// Try to find a property from object JSON that contains color parameters, and output the corresponding color
    /// </summary>
    protected bool TryGetColorFromObjectData(ObjectData data, out Color color)
    {
        string colorString;
        try
        {
            colorString = data.properties.First(x => x.ToLower().StartsWith("color:"));
        }
        catch
        {
            color = new Color(0, 0, 0, 0);
            return false;
        }

        List<float> colorParams = new();
        try
        {
            colorParams = colorString.Substring(6).Trim().Split(',').Select(float.Parse).ToList();
        }
        catch
        {
            ModLog.Warn("Invalid color parameters given!");
            color = new Color(0, 0, 0, 0);
            return false;
        }

        if (colorParams.Count == 3)
        {
            color = new Color(colorParams[0], colorParams[1], colorParams[2]);
        }
        else if (colorParams.Count == 4)
        {
            color = new Color(colorParams[0], colorParams[1], colorParams[2], colorParams[3]);
        }
        else
        {
            ModLog.Warn("Invalid color parameters given!");
            color = new Color(0, 0, 0, 0);
            return false;
        }

        return true;
    }
}
