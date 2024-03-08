using Framework.Managers;
using Gameplay.GameControllers.Entities;
using Gameplay.GameControllers.Penitent.Abilities;
using HarmonyLib;
using System.Linq;
using UnityEngine;

namespace Blasphemous.LostDreams.Effects;

/// <summary>
/// Handles LD01
/// </summary>
public class HealthDrain
{
    private readonly Config _config;

    private float _currentDrainDelay = 0f;
    private bool _pauseDrain = false;

    /// <summary>
    /// Should drain health if penitence is active and not resting at prie dieu
    /// </summary>
    public bool ShouldDrainHealth => Main.LostDreams.PenitenceHandler.IsActive("PE_LD01")
        && !DRAIN_BLOCKS.Any(Core.Input.HasBlocker);

    /// <summary>
    /// Should apply thorns if penitence is active or bead is equipped
    /// </summary>
    public bool ShouldApplyThorns => Main.LostDreams.PenitenceHandler.IsActive("PE_LD01")
        || Main.LostDreams.ItemHandler.IsEquipped("RB551");

    internal HealthDrain(Config config)
    {
        _config = config;

        Main.LostDreams.EventHandler.OnEnemyDamaged += HitEnemy;
        Main.LostDreams.EventHandler.OnEnemyKilled += KillEnemy;
        Main.LostDreams.EventHandler.OnPlayerDamaged += PlayerTakeDamage;
        Main.LostDreams.EventHandler.OnExitGame += ResumeDrain;
    }

    /// <summary>
    /// Processes the health drain cycle and speed cycle
    /// </summary>
    public void Update()
    {
        // Reset health drain
        if (_pauseDrain || !ShouldDrainHealth || Core.Logic.Penitent == null)
        {
            _currentDrainDelay = _config.LD01_DRAIN_DELAY;
            return;
        }

        // Decrease and process health drain
        _currentDrainDelay -= Time.deltaTime;
        if (_currentDrainDelay <= 0)
        {
            _currentDrainDelay = _config.LD01_DRAIN_DELAY;
            Core.Logic.Penitent.Stats.Life.Current -= _config.LD01_DRAIN_AMOUNT;
            if (Core.Logic.Penitent.Stats.Life.Current <= 0)
                Core.Logic.Penitent.KillInstanteneously();
        }
    }

    /// <summary>
    /// Start a timer based on flask health and pause drain until then
    /// </summary>
    public void OnDrinkFlask()
    {
        float time = _config.LD01_FLASK_BASE + _config.LD01_FLASK_INCREASE * Core.Logic.Penitent.Stats.FlaskHealth.GetUpgrades();
        Main.LostDreams.Log($"Pausing health drain for {time} seconds");
        Main.LostDreams.TimeHandler.AddTimer("drain-pause", time, false, ResumeDrain);
        _pauseDrain = true;
    }

    private void ResumeDrain()
    {
        Main.LostDreams.Log("Resuming health drain");
        _pauseDrain = false;
    }

    /// <summary>
    /// Adds health to the player
    /// </summary>
    private void HealPlayer(float amount)
    {
        if (!ShouldDrainHealth)
            return;

        Main.LostDreams.Log("Healing player by " + amount);
        Core.Logic.Penitent.Stats.Life.Current += amount;
    }

    /// <summary>
    /// Apply thorns damage back to the enemy and reduce damage if contact
    /// </summary>
    private void PlayerTakeDamage(ref Hit hit)
    {
        if (!Main.LostDreams.HealthDrain.ShouldApplyThorns)
            return;

        IDamageable enemy = hit.AttackingEntity?.GetComponentInChildren<IDamageable>();
        if (enemy == null)
            return;

        Main.LostDreams.Log("Applying thorns damage");
        enemy.Damage(new Hit()
        {
            DamageAmount = _config.LD01_THORNS_AMOUNT,
            DamageElement = DamageArea.DamageElement.Contact,
            DamageType = DamageArea.DamageType.Normal,
            AttackingEntity = Core.Logic.Penitent.gameObject
        });

        if (hit.DamageElement == DamageArea.DamageElement.Contact)
        {
            Main.LostDreams.Log("Reducing contact damage");
            hit.DamageAmount = _config.LD01_CONTACT_AMOUNT;
        }
    }

    private void HitEnemy(ref Hit hit)
    {
        float healAmount = hit.AttackingEntity?.name == "Penitent(Clone)"
            ? hit.DamageAmount * _config.LD01_SWORD_HEAL_PERCENT
            : _config.LD01_PRAYER_HEAL_AMOUNT;

        HealPlayer(healAmount);
    }

    private void KillEnemy()
    {
        HealPlayer(_config.LD01_KILL_HEAL_AMOUNT);
    }

    private static readonly string[] DRAIN_BLOCKS =
    {
        "DIALOG", "CINEMATIC", "INTERACTABLE", "POP_UP"
    };
}

// When using a flask, perform special action instead of heal
[HarmonyPatch(typeof(Healing), "Heal")]
class Heal_Start_Patch
{
    public static bool Prefix()
    {
        if (!Main.LostDreams.HealthDrain.ShouldDrainHealth)
            return true;

        Core.Logic.Penitent.Stats.Flask.Current--;
        Main.LostDreams.HealthDrain.OnDrinkFlask();
        return false;
    }
}
