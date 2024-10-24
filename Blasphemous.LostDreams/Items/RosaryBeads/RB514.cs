
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

    private RawBonus _movementSpeedBonus;
    private RawBonus _fervourRegenMovementSpeedReduction;

    private delegate void RB514OnUpdateEvent();

    private event RB514OnUpdateEvent OnUpdateRB514;

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
                () => Core.Logic.Penitent.Stats.Speed.AddRawBonus(_movementSpeedBonus),
                () => Core.Logic.Penitent.Stats.Speed.RemoveRawBonus(_movementSpeedBonus), 
                () => { }) ,
              1f/7f },
            { new RB514RandomTickingEffect(
                "Emit mist", 
                false, 
                _config.MIST_DURATION, 
                () => { }, 
                () => { },
                () => { }, 
                _config.MIST_TICK_INTERVAL, 
                MistEffectOnTick) ,
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
                () => Core.Logic.Penitent.Stats.Speed.AddRawBonus(_fervourRegenMovementSpeedReduction), 
                () => Core.Logic.Penitent.Stats.Speed.RemoveRawBonus(_fervourRegenMovementSpeedReduction), 
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

        _movementSpeedBonus = new(Core.Logic.Penitent.Stats.Speed.Base * _config.MOVEMENT_SPEED_INCREASE_RATIO);
        _fervourRegenMovementSpeedReduction = new(Core.Logic.Penitent.Stats.Speed.Base * _config.FERVOUR_REGEN_MOVEMENT_SPEED_REDUCTION_RATIO);
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
#if DEBUG
        Main.LostDreams.Log($"Start rolling random effect for RB514");
        Main.LostDreams.Log($"Current random value for rolling: {randomValue}");
#endif
        for (int i = 0; i < cumulativeProbabilities.Count; i++)
        {
            if (randomValue <= cumulativeProbabilities[i])
            {
#if DEBUG
                Main.LostDreams.Log($"Triggered RB514 effect: {_effectsToProbabilities.Keys.ToList()[i].name}!");
#endif
                _effectsToProbabilities.Keys.ToList()[i].ActivateEffect();
                cancel = _effectsToProbabilities.Keys.ToList()[i].isFlaskHealCancelled;
                break;
            }
        }

    }


    private void LifestealEffectOnHit(ref Hit hit)
    {
        if (hit.AttackingEntity == Core.Logic.Penitent)
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

    private void MistEffectOnTick()
    {
        // WIP: spawn the mist object
    }

    private void AuraHealEffectOnActivate()
    {
        // WIP
    }

    private void AuraHealEffectOnDeactivate()
    {
        // WIP
    }

    private void AuraHealEffectOnTick()
    {
        // WIP
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
            Main.LostDreams.LogWarning($"Attempting to re-activate an already-active effect `{name}` for RB514.");

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
/// Effect triggered by RB514 that has an effect on fixed intervals.
/// </summary>
internal class RB514RandomTickingEffect : RB514RandomEffect
{
    private readonly float _tickInterval;
    private readonly System.Action _effectOnTick;
    private float _nextTick;

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
        this._tickInterval = tickInterval;
        this._effectOnTick = effectOnTick;
    }

    private protected override void OnActivateEffect()
    {
        base.OnActivateEffect();
        _nextTick = 0f;
    }

    internal override void OnUpdate()
    {
        base.OnUpdate();

        if (timer >= _nextTick)
        {
            _effectOnTick();
            _nextTick += _tickInterval;
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

    public float MIST_DURATION = 8f;

    public float MIST_TICK_INTERVAL = 1f;

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
    public static bool IsActive { get; set; }

    public static bool Prefix()
    {
        return !IsActive;
    }
}