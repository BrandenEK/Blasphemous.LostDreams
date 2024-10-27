
using Blasphemous.LostDreams.Levels;
using Blasphemous.ModdingAPI;
using Framework.FrameworkCore.Attributes.Logic;
using Framework.Managers;
using Gameplay.GameControllers.Effects.Player.Healing;
using Gameplay.GameControllers.Entities;
using Steamworks;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using HarmonyLib;
using Gameplay.GameControllers.Penitent.Damage;
using Gameplay.GameControllers.Penitent;
using UnityEngine.Serialization;
using System.Collections;
using Gameplay.UI;

namespace Blasphemous.LostDreams.Items.RosaryBeads;

internal class RB514 : EffectOnEquip
{
    private readonly RB514Config _config;

    /// <summary>
    /// Dictionary of all possible effects triggered by the bead. 
    /// Key: functions that return a bool indicating whether the flask heal is cancelled. 
    /// Value: probability of corresponding function to be chosen at random. 
    /// </summary>
    private readonly Dictionary<RB514RandomEffect, float> _effectsToProbabilities;

    private readonly float _movementSpeedBonus;
    private readonly float _fervourRegenMovementSpeedReduction;
    private readonly Sprite _healingAuraSprite;
    private GameObject _healingAuraGameObject;
    private readonly Sprite _toxicMistSprite;
    private GameObject[] _toxicMistGameObjects = new GameObject[MAX_MIST_OBJECT_COUNT + 1];
    private GameObject _toxicMistPrefab;
    private int _mistCounter = 0;

    private delegate void RB514OnUpdateEvent();

    private event RB514OnUpdateEvent OnUpdateRB514;

    private const int MAX_MIST_OBJECT_COUNT = 20;

    private bool IsAnyEffectActive
    {
        get
        {
            foreach (var effect in _effectsToProbabilities.Keys)
            {
                if (effect.isActive == true)
                    return true;
            }

            return false;
        }
    }

    public RB514(RB514Config config)
    {
        _config = config;

        // initialize buff values that are not in config
        float baseMovementSpeed = 5;
        _movementSpeedBonus = _config.MOVEMENT_SPEED_INCREASE_RATIO * baseMovementSpeed;
        _fervourRegenMovementSpeedReduction = _config.FERVOUR_REGEN_MOVEMENT_SPEED_REDUCTION_RATIO * baseMovementSpeed;

        // import sprites
        Main.LostDreams.FileHandler.LoadDataAsSprite("effects/RB514_healing_aura.png", out _healingAuraSprite);
        Main.LostDreams.FileHandler.LoadDataAsSprite("effects/RB514_toxic_mist.png", out _toxicMistSprite);

        // initialize all random effects and their probability of being selected
        _effectsToProbabilities = new()
        {
            { new RB514RandomEffect(
                "Gain lifesteal instead",
                true,
                _config.LIFESTEAL_DURATION,
                () => Main.LostDreams.EventHandler.OnEnemyDamaged += LifestealEffectOnHit,
                () => Main.LostDreams.EventHandler.OnEnemyDamaged -= LifestealEffectOnHit,
                () => { }), 
              1f/7f },
            { new RB514RandomEffect(
                "Nullify next hit", 
                false, 
                float.MaxValue, 
                () => Main.LostDreams.EventHandler.OnPlayerDamaged += NullifyDamageOnBeingHit,
                () => Main.LostDreams.EventHandler.OnPlayerDamaged -= NullifyDamageOnBeingHit, 
                () => { }), 
              1f/7f },
            { new RB514RandomEffect(
                "Boost movement speed", 
                false, 
                _config.MOVEMENT_SPEED_INCREASE_DURATION, 
                () => Core.Logic.Penitent.PlatformCharacterController.MaxWalkingSpeed += _movementSpeedBonus,
                () => Core.Logic.Penitent.PlatformCharacterController.MaxWalkingSpeed -= _movementSpeedBonus, 
                () => { }) ,
              1f/7f },
            { new RB514RandomTickingEffect(
                "Emit mist", 
                false, 
                _config.MIST_EFFECT_TOTAL_DURATION, 
                MistEffectOnActivate, 
                () => { },
                () => { }, 
                new List<RB514RandomTickingEffect.TickingEffect>()
                    {
                        new(_config.MIST_SPAWN_TICK_INTERVAL, SpawnMist), 
                        new(_config.MIST_DAMAGE_TICK_INTERVAL, MistDealDamage)
                    }) ,
              1f/7f },
            { new RB514RandomTickingEffect(
                "Healing aura instead", 
                true, 
                _config.AURA_HEAL_DURATION, 
                AuraHealEffectOnActivate,
                AuraHealEffectOnDeactivate, 
                () => { }, 
                _config.AURA_HEAL_TICK_INTERVAL, 
                AuraHealEffectOnTick), 
              1f/7f },
            { new RB514RandomTickingEffect(
                "Fervour regen", 
                false, 
                _config.FERVOUR_REGEN_DURATION, 
                () => Core.Logic.Penitent.PlatformCharacterController.MaxWalkingSpeed += _fervourRegenMovementSpeedReduction, 
                () => Core.Logic.Penitent.PlatformCharacterController.MaxWalkingSpeed -= _fervourRegenMovementSpeedReduction, 
                () => { }, 
                _config.FERVOUR_REGEN_TICK_INTERVAL,
                () =>
                    {
                        float fervourIncreaseEachTick = _config.FERVOUR_REGEN_TOTAL_AMOUNT / (_config.FERVOUR_REGEN_DURATION / _config.FERVOUR_REGEN_TICK_INTERVAL);
                        Core.Logic.Penitent.Stats.Fervour.Current += fervourIncreaseEachTick;
                    }), 
              1f/7f },
            { new RB514RandomEffect(
                "Unstoppable stance", 
                false, 
                _config.UNSTOPPABLE_STANCE_DURATION, 
                () => RB514_TrueUnstoppableStance_Patch.IsActive = true,
                () => RB514_TrueUnstoppableStance_Patch.IsActive = false, 
                () => { }), 
              1f/7f }
        };
    }

    protected override void OnEquip()
    {
        Main.LostDreams.EventHandler.OnUseFlask += OnUseFlask;

        foreach (RB514RandomEffect effect in _effectsToProbabilities.Keys)
        {
            OnUpdateRB514 += effect.OnUpdate;
        }
    }

    protected override void OnUnequip()
    {
        Main.LostDreams.EventHandler.OnUseFlask -= OnUseFlask;

        foreach (RB514RandomEffect effect in _effectsToProbabilities.Keys)
        {
            effect.DeactivateEffect();
            OnUpdateRB514 -= effect.OnUpdate;
        }
    }

    protected override void OnUpdate()
    {
        OnUpdateRB514?.Invoke();
    }

    private void OnUseFlask(ref bool cancel)
    {
        // choose an effect based on probability of each effect
        List<float> cumulativeProbabilities = new();
        float cumulProb = 0f;
        foreach (float probability in _effectsToProbabilities.Values.ToList())
        {
            cumulProb += probability;
            cumulativeProbabilities.Add(cumulProb);
        }

        float randomValue = UnityEngine.Random.value;
        for (int i = 0; i < cumulativeProbabilities.Count; i++)
        {
            if (randomValue <= cumulativeProbabilities[i])
            {
#if DEBUG
                Main.LostDreams.Log($"Triggered RB514 effect: {_effectsToProbabilities.Keys.ToList()[i].name}!");
#endif
                _effectsToProbabilities.Keys.ToList()[i].ActivateEffect();
                cancel = _effectsToProbabilities.Keys.ToList()[i].isFlaskHealCancelled;
                if (cancel == true)
                {
                    // flask should still be consumed when healing is cancelled
                    Core.Logic.Penitent.Stats.Flask.Current -= 1;
                }
                break;
            }
        }

    }


    private void LifestealEffectOnHit(ref Hit hit)
    {
        if (hit.AttackingEntity == Core.Logic.Penitent.gameObject)
        {
            Core.Logic.Penitent.Stats.Life.Current += hit.DamageAmount * _config.LIFESTEAL_RATIO;
        }
    }

    private void NullifyDamageOnBeingHit(ref Hit hit)
    {
        if (hit.DamageAmount <= Mathf.Epsilon)
            return;

        hit.DamageAmount = 0;
        _effectsToProbabilities.Keys.First(x => x.name == "Nullify next hit").DeactivateEffect();

        RB503_Healing_Start_Patch.HealingFlag = true;
        Object.FindObjectOfType<HealingAura>()?.StartAura(Core.Logic.Penitent.Status.Orientation);
        Core.Logic.Penitent.Audio.PrayerInvincibility();
    }

    /// <summary>
    /// Create a template object at index 0 of the GameObject array, and make it inactive. 
    /// Each actual trigger of the crimson mist copies this object.
    /// </summary>
    private void MistEffectOnActivate()
    {
        _mistCounter = 0;
        // construct toxic mist template GameObject and its children sprite GameObject
        GameObject obj = new("RB514 toxic mist template");
        obj.SetActive(false);
        GameObject spriteObject = new("sprite");
        spriteObject.transform.SetParent(obj.transform);
        spriteObject.transform.position = obj.transform.position;
        spriteObject.AddComponent<SpriteRenderer>();
        var sr = obj.GetComponentInChildren<SpriteRenderer>();
        obj.AddComponent<BoxCollider2D>();
        var collider = obj.GetComponent<BoxCollider2D>();

        // initialize collider
        collider.size = new Vector2(2.5f, 1.5f);

        // initialize sprite
        sr.sprite = _toxicMistSprite;
        sr.sortingOrder = 1000;
        int pixelsPerUnit = 32;
        spriteObject.transform.localScale = new Vector3(
            collider.size.x * pixelsPerUnit / sr.sprite.rect.size.x,
            collider.size.y * pixelsPerUnit / sr.sprite.rect.size.y,
            1);

        // finalize initialization
        _toxicMistPrefab = obj;
    }

    private void SpawnMist()
    {
        // instantiate the template object and make it active
        if (_toxicMistGameObjects == null)
            return;

        if (_toxicMistGameObjects.Length == 0)
            return;

        _mistCounter++;
        _toxicMistGameObjects[_mistCounter] = UnityEngine.GameObject.Instantiate(
            _toxicMistPrefab,
            Core.Logic.Penitent.GetPosition(),
            default(Quaternion));
        GameObject obj = _toxicMistGameObjects[_mistCounter];
        obj.SetActive(true);

        // start a coroutine that despawn the mist after its lifetime is due
        UIController.instance.StartCoroutine(DespawnMist(obj));

        IEnumerator DespawnMist(GameObject mist)
        {
            yield return new WaitForSeconds(_config.MIST_CLOUD_LIFETIME);
            mist.SetActive(false);
        }
    }

    private void MistDealDamage()
    {
        if (_toxicMistGameObjects == null)
            return;

        if (_toxicMistGameObjects.Length == 0)
            return;

        foreach (var mistObject in _toxicMistGameObjects.Where(
            x =>
            {
                if (x == null) 
                    return false;
                return x.activeSelf == true;
            }))
        {
            List<IDamageable> damageableEntities = GetObjectsOfGivenTypeWithinCollider<IDamageable>(mistObject.GetComponent<BoxCollider2D>());

            if (damageableEntities.Count == 0)
                continue;

            foreach (var entity in damageableEntities)
            {
                if (entity is Penitent)
                    continue;

                Hit hit = new()
                {
                    AttackingEntity = Core.Logic.Penitent.gameObject, 
                    DamageAmount = _config.MIST_DAMAGE,
                    DamageElement = DamageArea.DamageElement.Toxic,
                    DamageType = DamageArea.DamageType.Simple
                };

                entity.Damage(hit);
            }
        }
    }


    private void AuraHealEffectOnActivate()
    {
        // construct aura GameObject and its children sprite GameObject
        _healingAuraGameObject = new("RB514 healing aura");
        _healingAuraGameObject.SetActive(false);
        GameObject spriteObject = new("sprite");
        spriteObject.transform.SetParent(_healingAuraGameObject.transform);
        spriteObject.transform.position = _healingAuraGameObject.transform.position;
        spriteObject.AddComponent<SpriteRenderer>();
        var sr = _healingAuraGameObject.GetComponentInChildren<SpriteRenderer>();
        _healingAuraGameObject.AddComponent<CircleCollider2D>();
        var collider = _healingAuraGameObject.GetComponent<CircleCollider2D>();

        // initialize collider
        _healingAuraGameObject.transform.position = Core.Logic.Penitent.GetPosition();
        collider.radius = _config.AURA_RADIUS;

        // initialize sprite
        sr.sprite = _healingAuraSprite;
        sr.sortingOrder = 100000;
        int pixelsPerUnit = 32;
        spriteObject.transform.localScale = new Vector3(
            collider.radius * 2 * pixelsPerUnit / sr.sprite.rect.size.x,
            collider.radius * 2 * pixelsPerUnit / sr.sprite.rect.size.y,
            1);

        // finalize initialization
        _healingAuraGameObject.SetActive(true);
    }

    private void AuraHealEffectOnDeactivate()
    {
        _healingAuraGameObject?.SetActive(false);
    }

    private void AuraHealEffectOnTick()
    {
        if (!_healingAuraGameObject)
            return;

        var collider = _healingAuraGameObject.GetComponent<CircleCollider2D>();
        List<Penitent> penitents = GetObjectsOfGivenTypeWithinCollider<Penitent>(collider);

        // heal TPO if it's inside the aura
        if (penitents.Count == 0)
            return;

        float originalFlaskHeal = Core.Logic.Penitent.Stats.FlaskHealth.Final;
        float healPerTick = _config.AURA_TOTAL_HEAL_RATIO * originalFlaskHeal / (_config.AURA_HEAL_DURATION / _config.AURA_HEAL_TICK_INTERVAL);
        Core.Logic.Penitent.Stats.Life.Current += healPerTick;
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
/// Effect triggered by RB514. 
/// </summary>
internal class RB514RandomEffect
{
    private protected readonly float _duration;
    private protected readonly System.Action _effectOnActivate;
    private protected readonly System.Action _effectOnDeactivate;
    private protected readonly System.Action _effectOnEachUpdate;

    internal readonly bool isFlaskHealCancelled;
    internal readonly string name;
    internal bool isActive = false;
    internal float timer = 0f;

    internal RB514RandomEffect(
        string name, 
        bool isFlaskHealCancelled,
        float duration, 
        System.Action effectOnActivate,
        System.Action effectOnDeactivate, 
        System.Action effectOnEachUpdate)
    {
        this.name = name;
        this.isFlaskHealCancelled = isFlaskHealCancelled;
        this._duration = duration;
        this._effectOnActivate = effectOnActivate;
        this._effectOnDeactivate = effectOnDeactivate;
        this._effectOnEachUpdate = effectOnEachUpdate;
    }

    private protected virtual void OnActivateEffect()
    {
        _effectOnActivate();
    }

    private protected virtual void OnDeactivateEffect()
    {
        _effectOnDeactivate();
    }

    /// <summary>
    /// Subscribed to <see cref="RB514.OnUpdateRB514"/>
    /// </summary>
    internal virtual void OnUpdate()
    {
        if (!isActive)
            return;

        timer += Time.deltaTime;
        _effectOnEachUpdate();

        if (timer >= _duration)
        {
            DeactivateEffect();
            timer = 0;
        }
    }

    internal virtual void ActivateEffect()
    {
        if (isActive)
        {
            Main.LostDreams.LogWarning($"Attempting to re-activate an already-active effect `{name}` for RB514. Deactivating it first");
            DeactivateEffect();
        }

        isActive = true;
        OnActivateEffect();
    }

    internal virtual void DeactivateEffect()
    {
        if (!isActive)
        {
            return;
        }

        isActive = false;
        OnDeactivateEffect();
    }

}

/// <summary>
/// Effect triggered by RB514 that last instataneously (i.e. does not require timing). 
/// </summary>
internal class RB514RandomInstantaneousEffect : RB514RandomEffect
{
    internal RB514RandomInstantaneousEffect(
        string name,
        bool isFlaskHealCancelled,
        System.Action effectOnActivate)
    : base(
        name,
        isFlaskHealCancelled,
        -1f,
        effectOnActivate,
        () => { },
        () => { })
    { }
}

/// <summary>
/// Effect triggered by RB514 that has one or multiple effect(s) on fixed intervals.
/// </summary>
internal class RB514RandomTickingEffect : RB514RandomEffect
{
    private List<TickingEffect> _tickingEffects;

    internal class TickingEffect
    {
        internal readonly float tickInterval;
        internal readonly System.Action effectOnTick;
        internal float nextTick;

        internal TickingEffect(float tickInterval, System.Action effectOnTick)
        {
            this.tickInterval = tickInterval;
            this.effectOnTick = effectOnTick;
        }
    }

    internal RB514RandomTickingEffect(
        string name,
        bool isFlaskHealCancelled,
        float duration,
        System.Action effectOnActivate,
        System.Action effectOnDeactivate,
        System.Action effectOnEachUpdate, 
        float tickInterval, 
        System.Action effectOnTick)
    : base(
        name, 
        isFlaskHealCancelled,
        duration, 
        effectOnActivate, 
        effectOnDeactivate, 
        effectOnEachUpdate)
    {
        _tickingEffects = [new(tickInterval, effectOnTick)];
    }

    internal RB514RandomTickingEffect(
        string name,
        bool isFlaskHealCancelled,
        float duration,
        System.Action effectOnActivate,
        System.Action effectOnDeactivate,
        System.Action effectOnEachUpdate,
        List<TickingEffect> effectsOnTick)
    : base(
        name,
        isFlaskHealCancelled,
        duration,
        effectOnActivate,
        effectOnDeactivate,
        effectOnEachUpdate)
    {
        _tickingEffects = effectsOnTick;
    }


    private protected override void OnActivateEffect()
    {
        base.OnActivateEffect();
        foreach (var effect in _tickingEffects)
        {
            effect.nextTick = effect.tickInterval;
        }
    }

    internal override void OnUpdate()
    {
        if (!isActive)
            return;

        base.OnUpdate();
        foreach (var effect in _tickingEffects)
        {
            if (timer >= effect.nextTick)
            {
                effect.effectOnTick();
                effect.nextTick += effect.tickInterval;
            }
        }
    }
}


/// <summary> 
/// Properties for RB514.
/// </summary>
public class RB514Config
{
    /// <summary>
    /// The ratio of life converted by damaging an enemy by lifesteal effect.
    /// </summary>
    public float LIFESTEAL_RATIO = 0.15f;

    public float LIFESTEAL_DURATION = 7.5f;

    /// <summary>
    /// Boost of movement speed (as ratio to base movement speed). 
    /// final_movement_speed = base_movement_speed * (1 + this). 
    /// </summary>
    public float MOVEMENT_SPEED_INCREASE_RATIO = 2f;

    public float MOVEMENT_SPEED_INCREASE_DURATION = 7.5f;

    public float MIST_EFFECT_TOTAL_DURATION = 8f;

    /// <summary>
    /// The interval at which each mist spawn after the previous one
    /// </summary>
    public float MIST_SPAWN_TICK_INTERVAL = 1f;

    /// <summary>
    /// The interval at which mist clouds deal instances of damage to enemies within.
    /// </summary>
    public float MIST_DAMAGE_TICK_INTERVAL = 0.5f;

    public float MIST_CLOUD_LIFETIME = 3f;

    public float MIST_DAMAGE = 20f;

    /// <summary>
    /// Total heal of the healing aura (as ratio to base flask heal). 
    /// final_heal = base_heal * this. 
    /// </summary>
    public float AURA_TOTAL_HEAL_RATIO = 1.5f;

    public float AURA_HEAL_DURATION = 7.5f;

    /// <summary>
    /// Interval between each instance of heal, in seconds. 
    /// Each tick heals (AURA_TOTAL_HEAL_RATIO * base_heal / (AURA_HEAL_DURATION / AURA_HEAL_TICK_INTERVAL) )
    /// </summary>
    public float AURA_HEAL_TICK_INTERVAL = 0.5f;

    public float AURA_RADIUS = 3f;

    public float FERVOUR_REGEN_TOTAL_AMOUNT = 75f;

    public float FERVOUR_REGEN_DURATION = 7.5f;

    /// <summary>
    /// Interval between each instance of fervour regen, in seconds. 
    /// Each tick regen fervour of (FERVOUR_REGEN_TOTAL_AMOUNT / (FERVOUR_REGEN_DURATION / FERVOUR_REGEN_TICK_INTERVAL) )
    /// </summary>
    public float FERVOUR_REGEN_TICK_INTERVAL = 0.5f;

    /// <summary>
    /// Reduction ratio to movement speed during the fervour regen effect. 
    /// Despite it being a "reduction", its value is negative. 
    /// </summary>
    public float FERVOUR_REGEN_MOVEMENT_SPEED_REDUCTION_RATIO = -0.5f;

    public float UNSTOPPABLE_STANCE_DURATION = 7.5f;
}

/// <summary>
/// Gives true unstoppable stance when "Unstoppable stance" effect of <see cref="RB514"/> is triggered. 
/// True unstoppable stance ignores knockback effect of all DamageTypes (including heavy attacks)
/// </summary>
[HarmonyPatch(typeof(PenitentDamageArea), "SetDamageAnimation")]
class RB514_TrueUnstoppableStance_Patch
{
    public static bool IsActive { get; set; } = false;

    public static bool Prefix()
    {
        return !IsActive;
    }
}