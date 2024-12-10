using Framework.Managers;
using Gameplay.GameControllers.Entities;
using Gameplay.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using EntityOrientation = Framework.FrameworkCore.EntityOrientation;

namespace Blasphemous.LostDreams.Items.RosaryBeads;

internal class RB504 : EffectOnEquip
{
    private readonly float _perpetvaMaxSpeed;
    private readonly Sprite _perpetvaSprite;
    private readonly Sprite _lightningStrikeSprite;
    private readonly Vector3 _offsetToTargetLeft;
    private readonly RB504Config _config;

    private GameObject _perpetvaGameObject;
    private GameObject _lightningStrikeGameObject;
    private GameObject _targetEnemyGameObject;
    private PerpetvaState _perpetvaState = PerpetvaState.SpawningOrDespawning;
    private Vector3 _perpetvaVelocity = Vector3.zero;
    private float _attackCooldownTimer = 0f;

    private GameObject TargetGameObject
    {
        get
        {
            switch (_perpetvaState)
            {
                case PerpetvaState.AttackingEnemy:
                    return _targetEnemyGameObject;
                case PerpetvaState.FollowingPenitent:
                    return Core.Logic.Penitent.gameObject;
                case PerpetvaState.SpawningOrDespawning:
                    return Core.Logic.Penitent.gameObject;
            }
            return null;
        }
    }

    private Vector3 TargetPosition
    {
        get
        {
            Vector3 offset = _offsetToTargetLeft;
            if (_perpetvaState == PerpetvaState.AttackingEnemy
                && IsPerpetvaToTheRightofTarget)
            {
                // Perpetva should move to the side of enemy closer to its current position
                offset.x = -offset.x;
            }
            else if ((_perpetvaState == PerpetvaState.FollowingPenitent || _perpetvaState == PerpetvaState.SpawningOrDespawning)
                && Core.Logic.Penitent.Status.Orientation == EntityOrientation.Left)
            {
                // Perpetva should be at the back of Penitent
                offset.x = -offset.x;
            }
            return TargetGameObject.transform.position + offset;
        }
    }

    private bool IsPerpetvaFacingRight
    {
        get
        {
            var sr = _perpetvaGameObject.GetComponentInChildren<SpriteRenderer>();
            return sr.gameObject.transform.localScale.x > 0;
        }
    }

    private bool IsPerpetvaToTheRightofTarget
    {
        get => _perpetvaGameObject.transform.position.x > TargetGameObject.transform.position.x;
    }

    private enum PerpetvaState
    {
        SpawningOrDespawning,
        FollowingPenitent,
        AttackingEnemy
    }

    internal RB504(RB504Config cfg)
    {
        _config = cfg;
        _perpetvaMaxSpeed = 10f;
        Main.LostDreams.FileHandler.LoadDataAsSprite("effects/RB504_perpetva.png", out _perpetvaSprite);
        Main.LostDreams.FileHandler.LoadDataAsSprite("effects/RB504_lightning_bolt.png", out _lightningStrikeSprite, new() { PixelsPerUnit = 32 });
        _offsetToTargetLeft = new Vector3(-2.5f, 2.5f, 0f);
    }

    protected override void OnEquip()
    {
        if (_perpetvaGameObject == null || _lightningStrikeGameObject == null)
        {
            CreateGameObjects();
        }
        _perpetvaGameObject.transform.position = TargetPosition;
        UIController.instance.StartCoroutine(PerpetvaSpawnAnimation());
    }

    protected override void OnUpdate()
    {
        UpdatePerpetvaFacingDirection(_perpetvaState);
        UpdatePerpetvaMovement(_perpetvaState);

        if (_perpetvaState == PerpetvaState.FollowingPenitent)
        {
            _attackCooldownTimer += Time.deltaTime;
            if (_attackCooldownTimer >= _config.ATTACK_COOLDOWN)
            {
                bool foundEnemy = TryDetectEnemy();
                if (foundEnemy)
                {
                    _attackCooldownTimer = 0f;
                    _perpetvaState = PerpetvaState.AttackingEnemy;
                    UIController.instance.StartCoroutine(AttackTargetEnemyCoroutine());
                }
            }
        }

    }

    protected override void OnUnequip()
    {
        UIController.instance.StartCoroutine(PerpetvaDespawnAnimation());
    }

    private void CreateGameObjects()
    {
        // create perpetva gameObject
        _perpetvaGameObject = new GameObject("RB504 Perpetva");
        // sprite
        GameObject spriteObject = new GameObject("sprite");
        spriteObject.transform.SetParent(_perpetvaGameObject.transform);
        spriteObject.transform.localPosition = Vector3.zero;
        var sr = spriteObject.AddComponent<SpriteRenderer>();
        sr.sprite = _perpetvaSprite;
        sr.sortingOrder = 1000;
        // physics
        var rb = _perpetvaGameObject.AddComponent<Rigidbody2D>();
        rb.isKinematic = true;
        rb.gravityScale = 0;
        // finalize
        _perpetvaGameObject.SetActive(false);

        // create lightning strike gameObject
        _lightningStrikeGameObject = new GameObject("RB504 lightning strike");
        // collider
        var colliderLightning = _lightningStrikeGameObject.AddComponent<BoxCollider2D>();
        colliderLightning.size = new Vector2(1, 3);
        // sprite
        GameObject spriteObjectLightning = new GameObject("sprite");
        spriteObjectLightning.transform.SetParent(_lightningStrikeGameObject.transform);
        spriteObjectLightning.transform.localPosition = Vector3.zero;
        var srLightning = spriteObjectLightning.AddComponent<SpriteRenderer>();
        srLightning.sprite = _lightningStrikeSprite;
        srLightning.sortingOrder = 10000;
        int pixelsPerUnit = 32;
        spriteObjectLightning.transform.localScale = new Vector3(
            colliderLightning.size.x * pixelsPerUnit / sr.sprite.rect.size.x,
            colliderLightning.size.y * pixelsPerUnit / sr.sprite.rect.size.y,
            1);
        // finalize
        _lightningStrikeGameObject.SetActive(false);
    }

    private IEnumerator PerpetvaSpawnAnimation()
    {
        _perpetvaState = PerpetvaState.SpawningOrDespawning;
        _perpetvaGameObject.SetActive(true);
        // WIP : animation code
        yield return null;
        _perpetvaState = PerpetvaState.FollowingPenitent;
        yield break;
    }

    private IEnumerator PerpetvaDespawnAnimation()
    {
        _perpetvaState = PerpetvaState.SpawningOrDespawning;
        // WIP : animation code
        _perpetvaGameObject.SetActive(false);
        yield break;
    }
    private void UpdatePerpetvaMovement(PerpetvaState perpetvaState)
    {
        if (perpetvaState == PerpetvaState.SpawningOrDespawning)
            return;

        // smooth movement using Vector3.Slerp()
        float damping = 3;
        _perpetvaGameObject.transform.position = Vector3.Slerp(
            _perpetvaGameObject.transform.position,
            TargetPosition,
            Time.deltaTime * damping);

        /*
        // alternative smooth movement using Vector3.SmoothDamp()
        _perpetvaGameObject.transform.position = Vector3.SmoothDamp(
            _perpetvaGameObject.transform.position,
            TargetPosition,
            ref _perpetvaVelocity,
            Time.deltaTime,
            _perpetvaMaxSpeed);
        */
    }

    private void UpdatePerpetvaFacingDirection(PerpetvaState perpetvaState)
    {
        SpriteRenderer sr = _perpetvaGameObject.GetComponentInChildren<SpriteRenderer>();
        if (perpetvaState == PerpetvaState.SpawningOrDespawning)
        {
            CheckAndTurnRight();
            return;
        }

        if (IsPerpetvaToTheRightofTarget)
        {
            // on the right of the reference object, face left
            CheckAndTurnLeft();
        }
        else
        {
            // on the left of the reference object, face right
            CheckAndTurnRight();
        }

        void CheckAndTurnRight()
        {
            if (!IsPerpetvaFacingRight)
                TurnDirection();
        }

        void CheckAndTurnLeft()
        {
            if (IsPerpetvaFacingRight)
                TurnDirection();
        }

        void TurnDirection()
        {
            sr.gameObject.transform.localScale = new Vector3(
                -sr.gameObject.transform.localScale.x,
                sr.gameObject.transform.localScale.y,
                sr.gameObject.transform.localScale.z);
        }
    }

    private bool TryDetectEnemy()
    {
        // find all enemies in detection radius
        List<GameObject> allEnemyGameObjectsInRange = SelectEnemiesFromColliders(
            Physics2D.OverlapCircleAll(
                (Vector2)_perpetvaGameObject.transform.position,
                _config.DETECTION_RADIUS))
            .Select(x => x.gameObject)
            .ToList();

        // if no enemies are found, return false
        if (allEnemyGameObjectsInRange.Count <= 0)
        {
            return false;
        }

        // find the enemy closest to Perpetva
        allEnemyGameObjectsInRange.Sort(
            (GameObject objX, GameObject objY) =>
            {
                float xDist = (objX.transform.position - _perpetvaGameObject.transform.position).magnitude;
                float yDist = (objY.transform.position - _perpetvaGameObject.transform.position).magnitude;
                return xDist > yDist ? 1 : -1;
            });

        _targetEnemyGameObject = allEnemyGameObjectsInRange[0];
        return true;
    }

    private IEnumerator AttackTargetEnemyCoroutine()
    {
        // WIP : Should add Lightning strike VFX and SFX

        float delayBeforeStartAttack = 0.5f;
        float durationOfLightningHitbox = 1f;
        float intervalOfHitboxCheck = 0.2f;
        float delayBeforeReturningToPenitent = 0.5f;
        Vector3 lightningBoltOffset = new Vector3(0, 1.6f, 0);

        // wait for a while before summoning the lightning bolt
        yield return new WaitForSeconds(delayBeforeStartAttack);

        // start attacking by activating the lightning bolt GameObject
        _lightningStrikeGameObject.transform.position = _targetEnemyGameObject.transform.position + lightningBoltOffset;
        _lightningStrikeGameObject.SetActive(true);
        var collider = _lightningStrikeGameObject.GetComponent<Collider2D>();

        // attack enemies in the lightning at regular intervals (not doing that every frame to reduce lag)
        float attackTimer = 0f;
        List<IDamageable> alreadyDamagedTargets = new();
        while (attackTimer < durationOfLightningHitbox)
        {
            List<IDamageable> enemyIDamageables = GetEnemiesWithinCollider(collider)
                .Select(x => x.gameObject.GetComponent<IDamageable>())
                .ToList();
            foreach (var damageable in enemyIDamageables)
            {
                if (alreadyDamagedTargets.Contains(damageable))
                    continue;

                damageable.Damage(
                    new Hit()
                    {
                        AttackingEntity = _perpetvaGameObject,
                        DamageAmount = _config.ATTACK_DAMAGE,
                        DamageElement = DamageArea.DamageElement.Lightning,
                        DamageType = DamageArea.DamageType.Normal,
                    });
                alreadyDamagedTargets.Add(damageable);
            }
            attackTimer += intervalOfHitboxCheck;
            yield return new WaitForSeconds(intervalOfHitboxCheck);
        }
        // despawn the lightning bolt GameObject
        _lightningStrikeGameObject.SetActive(false);

        // finish attacking, wait a while before returning to idle
        yield return new WaitForSeconds(delayBeforeReturningToPenitent);
        _perpetvaState = PerpetvaState.FollowingPenitent;
        _attackCooldownTimer = delayBeforeStartAttack + durationOfLightningHitbox + delayBeforeReturningToPenitent;
        yield return null;
    }

    private List<Enemy> GetEnemiesWithinCollider(Collider2D collider)
    {
        Collider2D[] collidersInRange = Physics2D.OverlapAreaAll(collider.bounds.min, collider.bounds.max);
        return SelectEnemiesFromColliders(collidersInRange);
    }

    private List<Enemy> SelectEnemiesFromColliders(Collider2D[] colliders)
    {
        List<Enemy> result = new();
        foreach (Collider2D col in colliders)
        {
            // judge if the collider is an enemy's damage area hitbox collider
            bool isEnemyDamageHitbox = false;
            if (col.gameObject.GetComponent<Enemy>() != null
                && col.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                // the hitbox is attacked directly to the enemy GameObject that has an `Enemy` script
                isEnemyDamageHitbox = true;
            }
            if (col.gameObject.layer == LayerMask.NameToLayer("Enemy")
                && (col.gameObject.GetComponent<DamageAreaSwapper>() != null || col.gameObject.name.Trim().Equals("DamageArea"))
                && col.GetComponentInParent<Enemy>() != null)
            {
                // other case: if the damage hitbox is stored in an children gameObject with the name "DamageArea" or with the script `DamageAreaSwapper`
                //   while its parent has the `Enemy` script
                isEnemyDamageHitbox = true;
            }

            if (isEnemyDamageHitbox && col.GetComponentInParent<IDamageable>() != null)
            {
                result.Add(col.GetComponentInParent<Enemy>());
            }
        }
        return result;
    }
}

/// <summary> Properties for RB504 </summary>
public class RB504Config
{
    /// <summary>
    /// Damage of Perpetva's lightning strike attack
    /// </summary>
    public float ATTACK_DAMAGE = 40f;

    /// <summary>
    /// Cooldown interval between Perpetva's lightning strike attacks, INCLUDING timed used during its attack animation. 
    /// If it's shorter than attack animation duration, Perpetva will attack continuously.
    /// </summary>
    public float ATTACK_COOLDOWN = 2f;

    /// <summary>
    /// How far is Perpetva able to detect and attack enemies
    /// </summary>
    public float DETECTION_RADIUS = 7f;
}