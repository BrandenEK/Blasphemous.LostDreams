using Blasphemous.Framework.Levels.Modifiers;
using Blasphemous.Framework.Levels;
using Blasphemous.LostDreams.Components;
using Blasphemous.LostDreams.Npc;
using Gameplay.GameControllers.Entities;
using UnityEngine;

namespace Blasphemous.LostDreams.Levels.Modifiers;

/// <summary>
/// Sets the animation, collider, and dialog properties for an NPC
/// </summary>
internal class NpcModifier : IModifier
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
