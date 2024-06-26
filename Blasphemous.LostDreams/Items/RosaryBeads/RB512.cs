using Framework.Managers;
using Gameplay.GameControllers.Entities;
using Gameplay.GameControllers.Entities.Weapon;
using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace Blasphemous.LostDreams.Items.RosaryBeads;

internal class RB512 : EffectOnEquip
{
    private RB512Config _config;

    public RB512(RB512Config config)
    {
        _config = config;
        //_explosionWeapon = new(_config);

        // WIP
        //Main.LostDreams.EventHandler.OnUseFlask += CreateExplosion;
    }

    private void CreateExplosion(ref bool cancel)
    {
        //_explosionWeapon.DefaultAttack();
    }
    
}

/// <summary> 
/// Properties for RB512
/// </summary>
public class RB512Config
{
    public float DAMAGE = 100f;
    public DamageArea.DamageElement DAMAGE_ELEMENT = DamageArea.DamageElement.Normal;
    public DamageArea.DamageType DAMAGE_TYPE = DamageArea.DamageType.Normal;
}
