using Framework.Managers;
using Gameplay.GameControllers.Entities;
using System.Collections.Generic;
using UnityEngine;

namespace Blasphemous.LostDreams.Items.RosaryBeads;

internal class RB512 : EffectOnEquip
{
    private float _currentTimer;
    float _explosionDelay = 0.5f;
    float _shockwaveDuration = 0.5f;
    private Vector3 _shockwavePosition;

    private bool _isActive = false;

    private RB512ExplosionAttack _explosionAttack;
    private RB512Config _config;


    public RB512(RB512Config config)
    {
        _config = config;
        _explosionAttack = new(_config);

        Main.LostDreams.EventHandler.OnUseFlask += CreateExplosion;
    }

    private void CreateExplosion(ref bool cancel)
    {
        if (!IsEquipped)
        { return; }

        _currentTimer = 0f;
        _isActive = true;
        
        Core.Logic.Penitent.Audio.PlayVerticalAttackLanding();

    }

    protected override void OnUpdate()
    {
        if (!_isActive)
        { return; }

        _currentTimer += Time.deltaTime;
        if ( _currentTimer >= _explosionDelay 
            && _currentTimer < _explosionDelay + Time.deltaTime )
        { 
            _explosionAttack.StartAttack();
            _shockwavePosition = Core.Logic.Penitent.GetPosition();
            Core.Logic.CameraManager.ShockwaveManager.Shockwave(
                _shockwavePosition, 
                _shockwaveDuration * 0.5f,
                0.05f, 
                _config.ATTACK_RADIUS * 0.1f);
        }
        else if (_currentTimer >= _explosionDelay + _shockwaveDuration * 0.5f
            && _currentTimer < _explosionDelay + _shockwaveDuration * 0.5f + Time.deltaTime)
        {
            _isActive = false;
            Core.Logic.CameraManager.ShockwaveManager.Shockwave(
                _shockwavePosition,
                _shockwaveDuration * 0.5f,
                _config.ATTACK_RADIUS * 0.1f,
                1.5f) ;
        }
        
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

    private List<IDamageable> _damageableEntities;

    /// <summary>
    /// Constructor for RB512's explosion attack
    /// </summary>
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

    /// <summary>
    /// Initiate a RB512 explosion attack
    /// </summary>
    public void StartAttack()
    {
        Vector3 centerPosition = Core.Logic.Penitent.transform.position;
        _damageableEntities = GetDamageableEntitiesWithinCircleArea(centerPosition, _config.ATTACK_RADIUS);
        AttackDamageableEntities(_hit, _damageableEntities);
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
    /// <param name="centerPos">The center of the circle</param>
    /// <param name="radius">The radius of the circle</param>
    /// <returns></returns>
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

            if (damageableComponent != null)
            {
                if (damageableComponent is not Enemy)
                { continue; }

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
}

