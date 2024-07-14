using Blasphemous.LostDreams.Components;
using Framework.Managers;
using Gameplay.GameControllers.Entities;
using System.Collections.Generic;
using UnityEngine;

namespace Blasphemous.LostDreams.Items.RosaryBeads;

internal class RB512 : EffectOnEquip
{
    private float _currentTimer;
    private Vector3 _shockwavePosition;

    private bool _isActive = false;

    private readonly RB512ExplosionAttack _explosionAttack;
    private readonly RB512Config _config;

    public RB512(RB512Config config)
    {
        _config = config;
        _explosionAttack = new(_config);

        Main.LostDreams.EventHandler.OnUseFlask += CreateExplosion;
    }

    private void CreateExplosion(ref bool cancel)
    {
        if (!IsEquipped)
            return;

        _currentTimer = 0f;
        _isActive = true;
        
        Core.Logic.Penitent.Audio.PlayVerticalAttackLanding();
    }

    protected override void OnUpdate()
    {
        if (!_isActive)
            return;

        _currentTimer += Time.deltaTime;
        if (_currentTimer >= EXPLOSION_DELAY && _currentTimer < EXPLOSION_DELAY + Time.deltaTime)
        { 
            _explosionAttack.StartAttack();
            _shockwavePosition = Core.Logic.Penitent.GetPosition();
            Core.Logic.CameraManager.ShockwaveManager.Shockwave(
                _shockwavePosition,
                SHOCKWAVE_DURATION * 0.5f,
                0.05f, 
                _config.ATTACK_RADIUS * 0.1f);
        }
        else if (_currentTimer >= EXPLOSION_DELAY + SHOCKWAVE_DURATION * 0.5f && _currentTimer < EXPLOSION_DELAY + SHOCKWAVE_DURATION * 0.5f + Time.deltaTime)
        {
            _isActive = false;
            Core.Logic.CameraManager.ShockwaveManager.Shockwave(
                _shockwavePosition,
                SHOCKWAVE_DURATION * 0.5f,
                _config.ATTACK_RADIUS * 0.1f,
                1.5f) ;
        }
        
    }

    private const float EXPLOSION_DELAY = 0.5f;
    private const float SHOCKWAVE_DURATION = 0.5f;
}

/// <summary> 
/// Properties for RB512
/// </summary>
public class RB512Config
{
    /// <summary> The damage amount of the explosion </summary>
    public float DAMAGE = 100f;
    /// <summary> The damage element of the explosion </summary>
    public DamageArea.DamageElement DAMAGE_ELEMENT = DamageArea.DamageElement.Normal;
    /// <summary> The damage type of the explosion </summary>
    public DamageArea.DamageType DAMAGE_TYPE = DamageArea.DamageType.Normal;
    /// <summary> The radius from the player that the explosion should effect </summary>
    public float ATTACK_RADIUS = 3f;
}

/// <summary>
/// Constructor for RB512's explosion attack
/// </summary>
internal class RB512ExplosionAttack(RB512Config config)
{
    private readonly RB512Config _config = config;

    private Hit _hit;
    private List<IDamageable> _damageableEntities;

    /// <summary>
    /// Initiate a RB512 explosion attack
    /// </summary>
    public void StartAttack()
    {
        Vector3 centerPosition = Core.Logic.Penitent.transform.position;
        _damageableEntities = GetDamageableEntitiesWithinCircleArea(centerPosition, _config.ATTACK_RADIUS);
        _hit = CreateHit();
        AttackDamageableEntities(_hit, _damageableEntities);
        DisplayExplosionVfx();
        if (_damageableEntities.Count > 0)
        {
            OnHit();
        }
    }

    /// <summary>
    /// Effects that are triggered only when hitting at least 1 entity.
    /// </summary>
    private void OnHit()
    {
        Core.Logic.ScreenFreeze.Freeze(0.05f, 0.05f);
        Core.Logic.CameraManager.ProCamera2DShake.Shake(
            0.35f, Vector3.down * 0.5f, 12, 0.2f, 0.01f, default(Vector3), 0.01f, true);        
    }

    /// <summary>
    /// Return a list of enemies within a circular area
    /// </summary>
    public List<IDamageable> GetDamageableEntitiesWithinCircleArea(Vector3 centerPos, float radius)
    {
        Collider2D[] allCollidersInRange = Physics2D.OverlapCircleAll(centerPos, radius);
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

            if (damageableComponent != null && damageableComponent is Enemy)
            {
                allDamageablesInRange.Add(damageableComponent);
            }
        }

        return allDamageablesInRange;
    }

    private void AttackDamageableEntities(Hit hit, List<IDamageable> targets)
    {
        for (int i = 0; i < targets.Count; i++)
        {
            targets[i].Damage(hit);
        }
    }

    private void DisplayExplosionVfx()
    {
        var obj = new GameObject("RB512 Explosion");
        obj.transform.position = Core.Logic.Penitent.GetPosition() + Vector3.up;

        var sr = obj.AddComponent<SpriteRenderer>();
        sr.sortingLayerName = "Player";
        sr.sortingOrder = 1000;

        var anim = obj.AddComponent<ModAnimator>();
        anim.Animation = Main.LostDreams.AnimationStorage["explosion_mercury"];
        anim.Loop = false;

        obj.AddComponent<ModVanisher>();

    }

    private Hit CreateHit()
    {
        return new Hit
        {
            DamageAmount = _config.DAMAGE,
            DamageType = _config.DAMAGE_TYPE,
            DamageElement = _config.DAMAGE_ELEMENT,
            AttackingEntity = Core.Logic.Penitent.gameObject
        };
    }
}

