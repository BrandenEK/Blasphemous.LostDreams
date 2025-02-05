using Blasphemous.Framework.Levels;
using Blasphemous.Framework.Levels.Modifiers;
using Blasphemous.LostDreams.Components;
using Blasphemous.LostDreams.Npc;
using Framework.FrameworkCore;
using Gameplay.GameControllers.Entities;
using System.Collections.Generic;
using System.Linq;
using Tools.Level.Interactables;
using UnityEngine;

namespace Blasphemous.LostDreams.Levels;

/// <summary>
/// Abstract class of all types of collider modifiers
/// </summary>
public abstract class ColliderModifier : IModifier
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
    public ColliderModifier(string layer, Vector2 offset)
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
            Main.LostDreams.LogWarning("Invalid color parameters given!");
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
            Main.LostDreams.LogWarning("Invalid color parameters given!");
            color = new Color(0, 0, 0, 0);
            return false;
        }

        return true;
    }
}

/// <summary>
/// A modifier that adds a box collider to an object
/// </summary>
public class BoxColliderModifier : ColliderModifier
{
    private readonly Vector2 _size;

    /// <summary>
    /// Specify the layer, size, and offset of the collider
    /// </summary>
    public BoxColliderModifier(string layer, Vector2 size, Vector2 offset)
        : base(layer, offset)
    {
        _size = size;
    }

    /// <summary>
    /// Specify the size of the collider, offset set to (0, 0)
    /// </summary>
    public BoxColliderModifier(string layer, Vector2 size)
        : this(layer, size, new Vector2(0, 0)) { }

    public override void Apply(GameObject obj, ObjectData data)
    {
        base.Apply(obj, data);

        var collider = obj.AddComponent<BoxCollider2D>();
        collider.size = _size;
        collider.offset = _offset;

        // if there's a color property, color the collider
        if (TryGetColorFromObjectData(data, out Color color))
        {
            GenerateColoredMesh(color, ref obj, out Mesh mesh, out MeshFilter mf, out MeshRenderer mr);

            // move the mesh to the same position and size of the collider
            Vector3[] vertices =
            [
            new Vector3(-0.5f * _size.x, -0.5f * _size.y) + (Vector3)_offset, // bottom left
            new Vector3(0.5f * _size.x, -0.5f * _size.y) + (Vector3)_offset,  // bottom right
            new Vector3(0.5f * _size.x, 0.5f * _size.y) + (Vector3)_offset,   // top right
            new Vector3(-0.5f * _size.x, 0.5f * _size.y) + (Vector3)_offset   // top left
            ];

            int[] triangles =
                [
                0, 2, 1,
                0, 3, 2
                ];

            mesh.vertices = vertices;
            mesh.triangles = triangles;
        }
    }
}

/// <summary>
/// A modifier that adds a polygon collider to an object
/// </summary>
public class PolygonColliderModifier : ColliderModifier
{
    private readonly Vector2[] _points;

    /// <summary>
    /// Specify the points and offset of the PolygonCollider
    /// </summary>
    public PolygonColliderModifier(string layer, Vector2[] points, Vector2 offset) 
        : base(layer, offset)
    {
        _points = points;
    }

    /// <summary>
    /// Specify the points of the PolygonCollider, offset set to (0, 0)
    /// </summary>
    public PolygonColliderModifier(string layer, Vector2[] points)
        : this(layer, points, new Vector2(0, 0)) { }

    public override void Apply(GameObject obj, ObjectData data)
    {
        base.Apply(obj, data);

        var collider = obj.AddComponent<PolygonCollider2D>();
        collider.points = _points;
        collider.offset = _offset;
    }
}

public class NpcModifier : IModifier
{
    public void Apply(GameObject obj, ObjectData data)
    {
        NpcInfo info = Main.LostDreams.NpcStorage[data.id];

        obj.name = info.Id;

        // Modify body properties (Animator, hitbox, etc)
        GameObject body = obj.transform.GetChild(1).gameObject;

        var anim = body.GetComponent<ModAnimator>();
        anim.Animation = Main.LostDreams.AnimationStorage[info.Animation];

        var collider = body.GetComponent<BoxCollider2D>();
        collider.size = new Vector2(info.ColliderWidth, info.ColliderHeight);
        collider.offset = new Vector2(0, info.ColliderHeight / 2);

        var entity = body.AddComponent<Entity>();
        entity.Status.CastShadow = true;
        entity.Status.IsGrounded = true;
        body.AddComponent<EntityShadow>();

        // Modify function properties (Dialog, etc)
        GameObject function = obj.transform.GetChild(0).gameObject;

        var interactable = function.GetComponent<ModInteractable>();
        interactable.Dialogs = data.properties;
    }
}

public class DoorModifier : IModifier
{
    public void Apply(GameObject obj, ObjectData data)
    {
        obj.name = $"Door[{data.id}]";

        Door door = obj.GetComponent<Door>();
        door.identificativeName = data.id;
        door.targetScene = data.properties[0];
        door.targetDoor = data.properties[1];
        door.exitOrientation = data.properties[2] switch
        {
            "left" => EntityOrientation.Left,
            "right" => EntityOrientation.Right,
            _ => throw new System.Exception("Invalid door orientation")
        };
        door.spawnPoint.transform.position = new()
        {
            x = float.Parse(data.properties[3]),
            y = float.Parse(data.properties[4]),
        };
    }
}