using Framework.FrameworkCore.Attributes;
using Framework.Managers;
using Gameplay.GameControllers.Entities;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Blasphemous.LostDreams.Items.Penitences;

internal class PE501 : Penitence
{
    private readonly PE501Config _config;

    private float _nextDrainTime;
    private float _timeToResumeDrain;

    /// <summary>
    /// Should drain health if penitence is active and not resting at prie dieu or other input blocks
    /// </summary>
    public bool ShouldDrainHealth => IsActive && !ShouldPreventDrain();

    /// <summary>
    /// Should apply thorns if penitence is active or bead is equipped
    /// </summary>
    public bool ShouldApplyThorns => IsActive || Main.LostDreams.RosaryBeadList.RB551.IsEquipped;

    /// <summary>
    /// Should reverse drain if the current time has not passed the reverse cutoff yet
    /// </summary>
    public bool ShouldReverseDrain => Time.time < _timeToResumeDrain;

    internal PE501(PE501Config config)
    {
        _config = config;

        Main.LostDreams.EventHandler.OnEnemyDamaged += HitEnemy;
        Main.LostDreams.EventHandler.OnEnemyKilled += KillEnemy;
        Main.LostDreams.EventHandler.OnPlayerDamaged += PlayerTakeDamage;
        Main.LostDreams.EventHandler.OnUseFlask += OnDrinkFlask;
    }

    protected override void OnActivate()
    {
        _nextDrainTime = Time.time + _config.DRAIN_DELAY;
        _timeToResumeDrain = 0;
    }

    protected override void OnUpdate()
    {
        Main.LostDreams.LogError("Update penitence");
        if (Time.time >= _nextDrainTime)
        {
            PerformDrain();
            _nextDrainTime = Time.time + _config.DRAIN_DELAY;
        }
    }

    /// <summary>
    /// Start a timer based on flask health and reverse drain until then
    /// </summary>
    public void OnDrinkFlask(ref bool cancel)
    {
        if (!ShouldDrainHealth)
            return;

        Core.Logic.Penitent.Stats.Flask.Current--;
        cancel = true;

        float time = _config.FLASK_BASE + _config.FLASK_INCREASE * Core.Logic.Penitent.Stats.FlaskHealth.GetUpgrades();
        _timeToResumeDrain = Time.time + time;
        Main.LostDreams.Log($"Reversing health drain for {time} seconds");
    }

    private void PerformDrain()
    {
        if (!ShouldDrainHealth)
            return;

        Life life = Core.Logic.Penitent.Stats.Life;
        float amount = _config.DRAIN_BASE + _config.DRAIN_INCREASE * life.GetUpgrades();
        if (ShouldReverseDrain)
            amount *= -1;

        Main.LostDreams.Log("Draining player by " + amount);
        life.Current -= amount;
        if (life.Current <= 0)
            Core.Logic.Penitent.KillInstanteneously();
    }

    /// <summary>
    /// Apply thorns damage back to the enemy and reduce damage if contact
    /// </summary>
    private void PlayerTakeDamage(ref Hit hit)
    {
        if (!ShouldApplyThorns)
            return;

        IDamageable enemy = hit.AttackingEntity?.GetComponentInChildren<IDamageable>();
        if (enemy == null)
            return;

        Main.LostDreams.Log("Applying thorns damage");
        enemy.Damage(new Hit()
        {
            DamageAmount = _config.THORNS_AMOUNT,
            DamageElement = DamageArea.DamageElement.Contact,
            DamageType = DamageArea.DamageType.Normal,
            AttackingEntity = Core.Logic.Penitent.gameObject
        });

        if (hit.DamageElement == DamageArea.DamageElement.Contact)
        {
            Main.LostDreams.Log("Reducing contact damage");
            hit.DamageAmount = _config.CONTACT_AMOUNT;
        }
    }

    private void HitEnemy(ref Hit hit)
    {
        float healAmount = hit.AttackingEntity?.name == "Penitent(Clone)"
            ? hit.DamageAmount * ApplyHealthModifier(_config.HEAL_SWORD_BASE, _config.HEAL_SWORD_INCREASE)
            : ApplyHealthModifier(_config.HEAL_PRAYER_BASE, _config.HEAL_PRAYER_INCREASE);

        HealPlayer(healAmount);
    }

    private void KillEnemy()
    {
        float amount = ApplyHealthModifier(_config.HEAL_KILL_BASE, _config.HEAL_KILL_INCREASE);
        HealPlayer(amount);
    }

    private void HealPlayer(float amount)
    {
        if (!ShouldDrainHealth)
            return;

        Main.LostDreams.Log("Healing player by " + amount);
        Core.Logic.Penitent.Stats.Life.Current += amount;
    }

    private float ApplyHealthModifier(float baseAmount, float increaseAmount)
    {
        return baseAmount + increaseAmount * (Core.Logic.Penitent?.Stats.Life.GetUpgrades() ?? 0);
    }

    private bool ShouldPreventDrain()
    {
        return Core.Input.HasBlocker("ANY") || BLOCKED_SCENES.Contains(Core.LevelManager.currentLevel?.LevelName);
    }

    private static readonly string[] BLOCKED_SCENES =
    [
        "D07Z01S03",
        "D14Z01S01",
        "D14Z02S01",
        "D14Z03S01",
    ];
}

/// <summary>
/// When checking for ANY input, block everything unless only player logic
/// </summary>
[HarmonyPatch(typeof(InputManager), nameof(InputManager.HasBlocker))]
class Input_Block_Patch
{
    public static void Postfix(string name, List<string> ___inputBlockers, ref bool __result)
    {
        if (name != "ANY")
            return;

        __result = ___inputBlockers.Count > 1 || ___inputBlockers.Count == 1 && ___inputBlockers[0] != "PLAYER_LOGIC";
    }
}

/// <summary> Properties for PE501 </summary>
public class PE501Config
{
    /// <summary> Base damage multiplier to heal after sword hit </summary>
    public float HEAL_SWORD_BASE = 0.1f;
    /// <summary> Additional damage multiplier to heal after sword hit </summary>
    public float HEAL_SWORD_INCREASE = 0.015f;

    /// <summary> Base amount to heal after prayer hit </summary>
    public float HEAL_PRAYER_BASE = 3f;
    /// <summary> Additional amount to heal after prayer hit </summary>
    public float HEAL_PRAYER_INCREASE = 0.45f;

    /// <summary> Base amount to heal after killing enemy </summary>
    public float HEAL_KILL_BASE = 15f;
    /// <summary> Additional amount to heal after killing enemy </summary>
    public float HEAL_KILL_INCREASE = 2.5f;

    /// <summary> Base time that drinking a flask stops health drain </summary>
    public float FLASK_BASE = 10f;
    /// <summary> Additional time per level that drinking a flask stops health drain </summary>
    public float FLASK_INCREASE = 6f;

    /// <summary> Number of seconds between each health drain tick </summary>
    public float DRAIN_DELAY = 2f;
    /// <summary> Base amount of health lost every tick </summary>
    public float DRAIN_BASE = 4f;
    /// <summary> Additional amount per health upgrade of health lost every tick </summary>
    public float DRAIN_INCREASE = 0.75f;

    /// <summary> Damage applied to enemies when the player is hit </summary>
    public float THORNS_AMOUNT = 40f;
    /// <summary> Damage applied to the player in place of contact damage </summary>
    public float CONTACT_AMOUNT = 3f;
}
