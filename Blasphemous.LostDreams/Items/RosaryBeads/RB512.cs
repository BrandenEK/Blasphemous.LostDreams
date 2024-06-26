using Framework.Managers;
using Gameplay.GameControllers.Entities;
using Gameplay.GameControllers.Entities.Weapon;
using Gameplay.GameControllers.Penitent;
using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Blasphemous.LostDreams.Items.RosaryBeads;

internal class RB512 : EffectOnEquip
{
    private RB512Config _config;

    private RB512ExplosionAttack _explosionAttack;

    public RB512(RB512Config config)
    {
        _config = config;
        _explosionAttack = new(_config);

        Main.LostDreams.EventHandler.OnUseFlask += CreateExplosion;
    }

    private void CreateExplosion(ref bool cancel)
    {
        _explosionAttack.StartAttack();
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

    public float ATTACK_RADIUS = 3f;
}


public class RB512ExplosionAttack
{
    private Hit _hit;

    private RB512Config _config;

    public RB512ExplosionAttack(RB512Config config)
    {
        _config = config;

        _hit = new Hit
        {
            DamageAmount = _config.DAMAGE,
            DamageType = _config.DAMAGE_TYPE,
            DamageElement = _config.DAMAGE_ELEMENT
        };
    }

    public void StartAttack()
    {
        Vector3 centerPosition = Core.Logic.Penitent.transform.position;
        List<IDamageable> damageableEntities = GetDamageableEntitiesWithinCircleArea(centerPosition, _config.ATTACK_RADIUS);
        for (int i = 0; i < damageableEntities.Count; i++)
        {
            Main.LostDreams.Log($"RB512's #{i} entity attacked: {damageableEntities[i]}");
        }
        AttackDamageableEntities(_hit, damageableEntities);
        if (damageableEntities.Count > 0)
        {
            Core.Logic.Penitent.SleepTimeByHit(_hit);
        }
    }

    private void OnHit(Hit weaponHit)
    {
        Core.Logic.CameraManager.ProCamera2DShake.Shake(
            0.5f, Vector3.down * 0.5f, 12, 0.2f, 0.01f, default(Vector3), 0.01f, true);
        
    }

    public List<IDamageable> GetDamageableEntitiesWithinCircleArea(Vector3 centerPos, float radius)
    {
        Collider2D[] allCollidersInRange = Physics2D.OverlapCircleAll(centerPos, radius) ;

        GameObject[] allObjectsInRange = new GameObject[allCollidersInRange.Length];
        List<IDamageable> allDamageablesInRange = new();

        for (int i = 0; i < allCollidersInRange.Length; i++)
        {
            GameObject gameObject = allCollidersInRange[i].gameObject;
            allObjectsInRange.SetValue(gameObject, i);
        }

        if (allObjectsInRange.Length < 0)
        {
            return allDamageablesInRange;
        }

        for (int j = 0; j < allObjectsInRange.Length; j++)
        {
            IDamageable damageableComponent = allObjectsInRange[j].GetComponentInParent<IDamageable>();

            if (damageableComponent != null)
            {
                if (damageableComponent is not Enemy)
                { continue; }

                allDamageablesInRange.Add(damageableComponent);
            }
        }


        for (int i = 0; i < allDamageablesInRange.Count; i++)
        {
            Main.LostDreams.Log($"#{i} damageable detected: {allDamageablesInRange[i]}");
        }

        return allDamageablesInRange;
    }

    private void AttackDamageableEntities(Hit hit, List<IDamageable> targets)
    {
        this.OnHit(hit);
        for (int i = 0; i < targets.Count; i++)
        {
            targets[i].Damage(hit);
        }
    }
}
