using Gameplay.GameControllers.Entities;
using UnityEngine;

namespace Blasphemous.LostDreams.Components;

public class ModDamageArea : DamageArea
{
    private void Awake()
    {
        damageAreaCollider = GetComponent<BoxCollider2D>();
    }
}

