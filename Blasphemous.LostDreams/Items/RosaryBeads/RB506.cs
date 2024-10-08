using Blasphemous.ModdingAPI;
using Framework.Managers;
using Gameplay.GameControllers.Entities;
using Rewired;
using UnityEngine;
using Framework.FrameworkCore;
using Gameplay.GameControllers.Penitent.Attack;

namespace Blasphemous.LostDreams.Items.RosaryBeads;

internal class RB506 : EffectOnEquip
{
    private readonly RB506Config _config;
    private RB506ProjectileAttack _projectileAttack;
    private float _timer = 0f;
    internal bool isBeamReady = false;

    internal bool IsAtFullHealth
    {
        get
        {
            if (Core.Logic.Penitent == null)
                return false;

            var penitentStats = Core.Logic.Penitent.Stats;
            return penitentStats.Life.Current + Mathf.Epsilon >= penitentStats.Life.CurrentMax;
        }
    }

    internal RB506(RB506Config cfg)
    {
        _config = cfg;
        _projectileAttack = new RB506ProjectileAttack(cfg);
    }

    protected override void OnEquip()
    {
        _timer = 0f;
        isBeamReady = false;
        Main.LostDreams.EventHandler.OnSwordAttack += OnSwordAttack;
    }

    protected override void OnUnequip()
    {
        isBeamReady = false;
        Main.LostDreams.EventHandler.OnSwordAttack -= OnSwordAttack;
    }

    protected override void OnUpdate()
    {
        if (isBeamReady || !IsEquipped)
            return;

        _timer += Time.deltaTime;
        if (_timer >= _config.COOLDOWN)
        {
            _timer = 0f;
            isBeamReady = IsAtFullHealth;
        }
    }

    internal void OnSwordAttack(PenitentSword.AttackType attackType)
    {
        _projectileAttack.StartAttack(attackType);
    }
}


internal class RB506ProjectileAttack
{
    private readonly RB506Config _config;
    private Hit _hit;



    internal RB506ProjectileAttack(RB506Config cfg)
    {
        _config = cfg;
    }

    internal Hit CreateHit()
    {
        return new Hit()
        {
            AttackingEntity = Core.Logic.Penitent.gameObject,
            DamageAmount = _config.BASE_DAMAGE + Core.Logic.Penitent.Stats.Strength.Final * _config.DAMAGE_STRENGTH_MULTIPLIER,
            DamageType = _config.DAMAGE_TYPE,
            DamageElement = _config.DAMAGE_ELEMENT
        };
    }

    /// <summary>
    /// Returns a normalized direction of the projectile, 
    /// according to the attack direction of TPO (front / up / crouch)
    /// </summary>
    internal Vector2 GetProjectileDirection(PenitentSword.AttackType attackType)
    {
        Vector2 result = new();
        var penitent = Core.Logic.Penitent;
        if (attackType == PenitentSword.AttackType.Crouch || penitent.IsCrouched || penitent.IsCrouchAttacking)
        {
            result = new Vector2(1, -0.5f); // angled at -30 degrees downwards
        }
        else if (attackType == PenitentSword.AttackType.AirUpward || attackType == PenitentSword.AttackType.GroundUpward)
        {
            result = new Vector2(0, 1);
        }
        else
        {
            result = new Vector2(1, 0);
        }

        if (penitent.Status.Orientation == EntityOrientation.Left)
        {
            // if facing left, make `x` coordinate negative
            result.x = -result.x;
        }
        return result.normalized;
    }

    /// <summary>
    /// Initiate an attack by creating a projectile and send it flying
    /// </summary>
    internal void StartAttack(PenitentSword.AttackType attackType)
    {
        // construct projectile GameObject
        GameObject obj = new("RB506_Projectile");
        obj.AddComponent<SpriteRenderer>();
        var sr = obj.GetComponent<SpriteRenderer>();
        obj.AddComponent<Rigidbody2D>();
        var rb = obj.GetComponent<Rigidbody2D>();
        obj.AddComponent<BoxCollider2D>();
        var collider = obj.GetComponent<BoxCollider2D>();
        collider.size = new Vector2(2.8f, 1f);

        // set projectile starting position
        // give the projectile a forward offset according to facing direction of TPO
        Vector2 startingOffset = new(1.8f, 0f);
        obj.transform.position = Core.Logic.Penitent.Status.Orientation == EntityOrientation.Right
            ? (Vector2)Core.Logic.Penitent.transform.position + startingOffset
            : (Vector2)Core.Logic.Penitent.transform.position - startingOffset;

        // send projectile flying
        collider.attachedRigidbody.velocity = GetProjectileDirection(attackType) * _config.PROJECTILE_SPEED;

        // trigger damage hitbox
        _hit = CreateHit();

        // terminate the projectile when reaching max distance or hit a wall

    }

    /// <summary>
    /// Triggered when hitting an enemy
    /// </summary>
    internal void OnHit()
    {

    }
}

/// <summary>
/// Config for RB506
/// </summary>
public class RB506Config
{
    /// <summary>
    /// Cooldown before next beam can be fired (in seconds)
    /// </summary>
    internal float COOLDOWN = 3f;

    /// <summary>
    /// Base damage of the beam
    /// </summary>
    internal float BASE_DAMAGE = 40f;

    /// <summary>
    /// The scaling of attack damage to the beam's damage.
    /// </summary>
    internal float DAMAGE_STRENGTH_MULTIPLIER = 1f;

    internal DamageArea.DamageType DAMAGE_TYPE = DamageArea.DamageType.Normal;

    internal DamageArea.DamageElement DAMAGE_ELEMENT = DamageArea.DamageElement.Lightning;

    internal float PROJECTILE_SPEED = 100f;

    /// <summary>
    /// Max distance accross which the projectile can travel (in units of Unity)
    /// </summary>
    internal float PROJECTILE_RANGE = 5f;
}