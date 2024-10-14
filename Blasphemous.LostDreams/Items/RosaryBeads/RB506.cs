using Framework.FrameworkCore;
using Framework.Managers;
using Gameplay.GameControllers.Entities;
using Gameplay.GameControllers.Penitent;
using Gameplay.GameControllers.Penitent.Attack;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
        _projectileAttack.InstantiateProjectileGameObjects();

        Main.LostDreams.EventHandler.OnSwordAttack += OnSwordAttack;
        Main.LostDreams.EventHandler.OnPlayerDamaged += OnPlayerDamaged;
    }

    protected override void OnUnequip()
    {
        isBeamReady = false;
        _projectileAttack.KillAllProjectiles();

        Main.LostDreams.EventHandler.OnSwordAttack -= OnSwordAttack;
        Main.LostDreams.EventHandler.OnPlayerDamaged -= OnPlayerDamaged;

    }

    protected override void OnUpdate()
    {
        if (!IsEquipped)
            return;

        _projectileAttack.OnUpdate();

        if (!isBeamReady)
            _timer += Time.deltaTime;

        if (_timer >= _config.COOLDOWN)
        {
            _timer = 0f;
            isBeamReady = IsAtFullHealth;
        }
    }

    internal void OnSwordAttack(PenitentSword.AttackType attackType)
    {
        if (isBeamReady)
        {
            _projectileAttack.StartAttack(attackType);
            isBeamReady = false;
            _timer = 0f;
        }
    }

    internal void OnPlayerDamaged(ref Hit hit)
    {
        if (hit.DamageAmount > Mathf.Epsilon)
        {
            isBeamReady = false;
        }
    }
}


internal class RB506ProjectileAttack
{
    private readonly RB506Config _config;
    private readonly Sprite _projectileSprite;

    private int _activeProjectileCount = 0;
    private const int MAX_PROJECTILE_COUNT = 5;
    private RB506Projectile[] _projectiles = new RB506Projectile[MAX_PROJECTILE_COUNT + 1];

    internal bool HasAnyActiveProjectile
    {
        get => _activeProjectileCount > 0;
    }

    internal float ProjectileMaxExistTime
    {
        get => _config.PROJECTILE_RANGE / _config.PROJECTILE_SPEED;
    }


    internal RB506ProjectileAttack(RB506Config cfg)
    {
        _config = cfg;
        Main.LostDreams.FileHandler.LoadDataAsSprite($"effects/holy_water_projectile.png", out Sprite sprite);
        _projectileSprite = sprite;
    }

    internal void InstantiateProjectileGameObjects()
    {
        for (int i = 0; i < _projectiles.Length; i++)
        {
            // set default objects to inactive
            _projectiles[i] = new()
            {
                gameObject = new($"{i}")
            };

            _projectiles[i].gameObject.SetActive(false);
        }
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
    /// Initiate an attack by creating a projectile and send it flying
    /// </summary>
    internal void StartAttack(PenitentSword.AttackType attackType)
    {

        // construct projectile GameObject
        _activeProjectileCount++;
        int currentProjectileIndex = _activeProjectileCount;

        _projectiles[currentProjectileIndex].gameObject = new GameObject($"RB506_Projectile_{currentProjectileIndex}");

        // adding and getting required components for GameObject
        _projectiles[currentProjectileIndex].gameObject.AddComponent<Rigidbody2D>();
        _projectiles[currentProjectileIndex].gameObject.AddComponent<BoxCollider2D>();
        GameObject spriteObject = new($"RB506_Projectile_{currentProjectileIndex}_sprite");
        spriteObject.transform.SetParent(_projectiles[currentProjectileIndex].gameObject.transform);
        spriteObject.AddComponent<SpriteRenderer>();

        var rb = _projectiles[currentProjectileIndex].gameObject.GetComponent<Rigidbody2D>();
        var collider = _projectiles[currentProjectileIndex].gameObject.GetComponent<BoxCollider2D>();
        var sr = _projectiles[currentProjectileIndex].gameObject.GetComponentInChildren<SpriteRenderer>();

        // getting sprite for projectile GameObject
        sr.sprite = _projectileSprite;
        sr.sortingOrder = 100000;
        int pixelsPerUnit = 32;
        spriteObject.transform.localScale = new Vector3(
            collider.size.x * pixelsPerUnit / sr.sprite.rect.size.x,
            collider.size.y * pixelsPerUnit / sr.sprite.rect.size.y,
            1);

        // configurating collider and rigidBody
        collider.size = new Vector2(2.8f, 1f);
        rb.isKinematic = true;
        collider.isTrigger = true;

        // set projectile starting position, offset, and rotation
        _projectiles[currentProjectileIndex].SetProjectileDirection(attackType);
        _projectiles[currentProjectileIndex].SetProjectileInitialPosition();

        // send projectile flying
        collider.attachedRigidbody.velocity = _projectiles[currentProjectileIndex].direction * _config.PROJECTILE_SPEED;
        collider.attachedRigidbody.gravityScale = 0;

        // trigger damage hitbox
        _projectiles[currentProjectileIndex].hit = CreateHit();
        _projectiles[currentProjectileIndex].attackedEntities.Clear();
        _projectiles[currentProjectileIndex].gameObject.SetActive(true);
        // collision check is done by OnUpdate()

        // terminate the projectile when reaching max distance or hitting a wall
        // termination is handled via OnUpdate()
    }

    /// <summary>
    /// Triggered when hitting an enemy
    /// </summary>
    internal void OnHit()
    {
        // misc. vfx code can be put here (e.g. camera shake, particle spawn)
    }

    /// <summary>
    /// Checks damage collision of each active projectile. 
    /// Triggered by <see cref="RB506.OnUpdate"/>. 
    /// </summary>
    internal void OnUpdate()
    {
        if (HasAnyActiveProjectile)
        {
            foreach (RB506Projectile projectile in _projectiles.Where(x => x.gameObject.activeSelf == true))
            {
                // collision detection and damage inflicting

                var collider = projectile.gameObject.GetComponent<BoxCollider2D>();
                List<IDamageable> damageableEntities = projectile.GetObjectsOfGivenTypeWithinCollider<IDamageable>(collider);
                foreach (IDamageable entity in damageableEntities.Where(x => !projectile.attackedEntities.Contains(x)))
                {
                    if (entity is Penitent)
                        return;

                    entity.Damage(projectile.hit);
                    OnHit();
                    projectile.attackedEntities.Add(entity);
                }

                // terminate projectiles reaching max distance
                projectile.timer += Time.deltaTime;
                if (projectile.timer >= ProjectileMaxExistTime)
                {
                    projectile.timer = 0;
                    projectile.gameObject.SetActive(false);
                    _activeProjectileCount--;
                }
            }
        }
    }

    internal void KillProjectile(int id)
    {
        if (_projectiles[id].gameObject == null)
            return;

        _projectiles[id]?.gameObject?.SetActive(false);
    }

    internal void KillAllProjectiles()
    {
        if (_projectiles.Length <= 0)
            return;
        for (int i = 0; i < _projectiles.Length; i++)
        {
            KillProjectile(i);
        }
    }
}

internal class RB506Projectile
{
    internal Hit hit;
    internal Vector2 direction;
    internal Vector2 offset;
    internal Quaternion rotation;
    internal GameObject gameObject;
    internal List<IDamageable> attackedEntities = new();
    internal float timer = 0f;

    internal RB506Projectile() { }

    /// <summary>
    /// Set the normalized direction, offset, and rotation of the projectile 
    /// according to the attack direction of TPO (front / up / crouch)
    /// </summary>
    internal void SetProjectileDirection(PenitentSword.AttackType attackType)
    {
        Vector2 direction = new();
        Vector2 offset = new();
        Vector3 rotation = new();
        var penitent = Core.Logic.Penitent;

        if (attackType == PenitentSword.AttackType.Crouch || penitent.IsCrouched || penitent.IsCrouchAttacking)
        {
            direction = new Vector2(1, -0.33f);
            offset = new Vector2(2f, -0.33f);
            rotation = new Vector3(0f, 0f, -30f);
        }
        else if (attackType == PenitentSword.AttackType.AirUpward || attackType == PenitentSword.AttackType.GroundUpward)
        {
            direction = new Vector2(0, 1);
            offset = new Vector2(0.3f, 2.5f);
            rotation = new Vector3(0f, 180f, -90f);
        }
        else
        {
            direction = new Vector2(1, 0);
            offset = new Vector2(1.8f, 1.2f);
            rotation = new Vector3(0, 0, 0);
        }

        if (penitent.Status.Orientation == EntityOrientation.Left)
        {
            direction.x = -direction.x;
            offset.x = -offset.x;
            rotation.z = -rotation.z;
        }

        this.direction = direction.normalized;
        this.offset = offset;

        Quaternion rotationQ = default(Quaternion);
        rotationQ.eulerAngles = rotation;
        this.rotation = rotationQ;
    }

    internal void SetProjectileInitialPosition()
    {
        this.gameObject.transform.position =
            (Vector2)Core.Logic.Penitent.transform.position
            + this.offset;
        this.gameObject.transform.rotation = this.rotation;
    }

    internal List<T> GetObjectsOfGivenTypeWithinCollider<T>(Collider2D collider)
    {
        Collider2D[] allCollidersInRange = Physics2D.OverlapAreaAll(collider.bounds.min, collider.bounds.max);
        return GetParentsOfGivenTypeFromColliders<T>(allCollidersInRange);
    }

    internal List<T> GetParentsOfGivenTypeFromColliders<T>(Collider2D[] colliders)
    {

        GameObject[] parentGameObjects = new GameObject[colliders.Length];
        List<T> parentsOfType = new();

        for (int i = 0; i < colliders.Length; i++)
        {
            GameObject gameObject = colliders[i].gameObject;
            parentGameObjects.SetValue(gameObject, i);
        }

        if (parentGameObjects.Length < 0)
        {
            return parentsOfType;
        }

        for (int j = 0; j < parentGameObjects.Length; j++)
        {
            T ComponentOfType = parentGameObjects[j].GetComponentInParent<T>();

            if (ComponentOfType != null && ComponentOfType is T) // can only damage enemies
            {
                parentsOfType.Add(ComponentOfType);
            }
        }

        return parentsOfType;
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

    /// <summary>
    /// Speed at which the projectile travels (in units/second)
    /// </summary>
    internal float PROJECTILE_SPEED = 10f;

    /// <summary>
    /// Max distance accross which the projectile can travel (in units of Unity)
    /// </summary>
    internal float PROJECTILE_RANGE = 5f;
}

