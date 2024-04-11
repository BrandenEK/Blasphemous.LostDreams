using Framework.FrameworkCore.Attributes;
using Framework.Managers;
using Gameplay.GameControllers.Entities;
using Gameplay.GameControllers.Penitent.Abilities;
using HarmonyLib;
using System.Collections.Generic;

namespace Blasphemous.LostDreams.Effects;

/// <summary>
/// Handles LD01
/// </summary>
public class HealthDrain
{
    private readonly Config _config;

    private bool _reverse = false;

    /// <summary>
    /// Should drain health if penitence is active and not resting at prie dieu or other input blocks
    /// </summary>
    public bool ShouldDrainHealth => Main.LostDreams.PenitenceHandler.IsActive("PE_LD01")
        && !Core.Input.HasBlocker("LD01");

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

        Main.LostDreams.TimeHandler.AddTicker("drain-tick", _config.LD01_DRAIN_DELAY, true, PerformDrain);
    }

    /// <summary>
    /// Start a timer based on flask health and reverse drain until then
    /// </summary>
    public void OnDrinkFlask()
    {
        float time = _config.LD01_FLASK_BASE + _config.LD01_FLASK_INCREASE * Core.Logic.Penitent.Stats.FlaskHealth.GetUpgrades();
        Main.LostDreams.Log($"Reversing health drain for {time} seconds");
        Main.LostDreams.TimeHandler.AddCountdown("drain-reverse", time, ResumeDrain);
        _reverse = true;
    }

    private void ResumeDrain()
    {
        Main.LostDreams.Log("Resuming health drain");
        _reverse = false;
    }

    private void PerformDrain()
    {
        if (!ShouldDrainHealth)
            return;

        Life life = Core.Logic.Penitent.Stats.Life;
        float amount = _config.LD01_DRAIN_BASE + _config.LD01_DRAIN_INCREASE * life.GetUpgrades();
        if (_reverse)
            amount *= -1;

        Main.LostDreams.Log("Draining player by " + amount);
        life.Current -= amount;
        if (life.Current <= 0)
            Core.Logic.Penitent.KillInstanteneously();
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
            ? hit.DamageAmount * ApplyHealthModifier(_config.LD01_HEAL_SWORD_BASE, _config.LD01_HEAL_SWORD_INCREASE)
            : ApplyHealthModifier(_config.LD01_HEAL_PRAYER_BASE, _config.LD01_HEAL_PRAYER_INCREASE);

        HealPlayer(healAmount);
    }

    private void KillEnemy()
    {
        float amount = ApplyHealthModifier(_config.LD01_HEAL_KILL_BASE, _config.LD01_HEAL_KILL_INCREASE);
        HealPlayer(amount);
    }

    private float ApplyHealthModifier(float baseAmount, float increaseAmount)
    {
        return baseAmount + increaseAmount * (Core.Logic.Penitent?.Stats.Life.GetUpgrades() ?? 0);
    }
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

// When checking for LD01 input, block everything unless only player logic
[HarmonyPatch(typeof(InputManager), nameof(InputManager.HasBlocker))]
class Input_Block_Patch
{
    public static void Postfix(string name, List<string> ___inputBlockers, ref bool __result)
    {
        if (name != "LD01")
            return;

        __result = ___inputBlockers.Count > 1 || ___inputBlockers.Count == 1 && ___inputBlockers[0] != "PLAYER_LOGIC";
    }
}
