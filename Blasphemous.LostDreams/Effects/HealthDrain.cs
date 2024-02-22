using Framework.FrameworkCore.Attributes.Logic;
using Framework.Managers;
using Gameplay.GameControllers.Entities;
using Gameplay.GameControllers.Penitent.Abilities;
using HarmonyLib;
using Tools.Level.Interactables;
using UnityEngine;

namespace Blasphemous.LostDreams.Effects;

/// <summary>
/// Handles LD01
/// </summary>
public class HealthDrain
{
    private readonly Config _config;
    private readonly RawBonus _attackSpeedBonus;

    private float _currentDrainDelay = 0f;
    private float _currentSpeedDelay = 0f;

    /// <summary>
    /// Should drain health if penitence is active and not resting at prie dieu
    /// </summary>
    public bool ShouldDrainHealth => !IsUsingPrieDieu
        && Main.LostDreams.PenitenceHandler.IsActive("PE_LD01");

    /// <summary>
    /// Should apply thorns if penitence is active or bead is equipped
    /// </summary>
    public bool ShouldApplyThorns => Main.LostDreams.PenitenceHandler.IsActive("PE_LD01")
        || Main.LostDreams.ItemHandler.IsEquipped("RB551");

    internal static bool IsUsingPrieDieu { get; set; }

    internal HealthDrain(Config config)
    {
        _config = config;
        _attackSpeedBonus = new RawBonus(config.LD01_SPEED_AMOUNT);

        Main.LostDreams.EventHandler.OnEnemyDamaged += HitEnemy;
        Main.LostDreams.EventHandler.OnEnemyKilled += KillEnemy;
        Main.LostDreams.EventHandler.OnPlayerDamaged += PlayerTakeDamage;
    }

    /// <summary>
    /// Processes the health drain cycle and speed cycle
    /// </summary>
    public void Update()
    {
        if (!ShouldDrainHealth || Core.Logic.Penitent == null)
        {
            _currentDrainDelay = _config.LD01_DRAIN_DELAY;
            _currentSpeedDelay = 0f;
            return;
        }

        _currentDrainDelay -= Time.deltaTime;
        if (_currentSpeedDelay > 0)
            _currentSpeedDelay -= Time.deltaTime;

        if (_currentDrainDelay <= 0)
        {
            _currentDrainDelay = _config.LD01_DRAIN_DELAY;
            Core.Logic.Penitent.Stats.Life.Current -= _config.LD01_DRAIN_AMOUNT;
            if (Core.Logic.Penitent.Stats.Life.Current <= 0)
                Core.Logic.Penitent.KillInstanteneously();
        }

        if (_currentSpeedDelay < 0)
        {
            ChangeAttackSpeed(false);
        }
    }

    /// <summary>
    /// Adds or removes the attack speed bonus
    /// </summary>
    public void ChangeAttackSpeed(bool enable)
    {
        if (enable)
        {
            _currentSpeedDelay = _config.LD01_SPEED_LENGTH;
            Core.Logic.Penitent.Stats.AttackSpeed.AddRawBonus(_attackSpeedBonus);
        }
        else
        {
            _currentSpeedDelay = 0f;
            Core.Logic.Penitent.Stats.AttackSpeed.RemoveRawBonus(_attackSpeedBonus);
        }
    }

    /// <summary>
    /// Adds health to the player
    /// </summary>
    private void HealPlayer(float amount)
    {
        if (!ShouldDrainHealth)
            return;

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
        if (hit.AttackingEntity?.name == "Penitent(Clone)")
            HealPlayer(_config.LD01_HIT_HEAL_AMOUNT);
    }

    private void KillEnemy()
    {
        HealPlayer(_config.LD01_KILL_HEAL_AMOUNT);
    }
}

// Control flag for when a prie dieu is in use
[HarmonyPatch(typeof(PrieDieu), "OnUpdate")]
class PrieDieu_Update_Patch
{
    public static void Postfix(PrieDieu __instance) => HealthDrain.IsUsingPrieDieu = __instance.BeingUsed;
}

// When using a flask, increase attack speed until time limit or leave room
[HarmonyPatch(typeof(Healing), "Heal")]
class Heal_Start_Patch
{
    public static bool Prefix()
    {
        if (!Main.LostDreams.HealthDrain.ShouldDrainHealth)
            return true;

        Core.Logic.Penitent.Stats.Flask.Current--;
        Main.LostDreams.HealthDrain.ChangeAttackSpeed(true);
        return false;
    }
}
