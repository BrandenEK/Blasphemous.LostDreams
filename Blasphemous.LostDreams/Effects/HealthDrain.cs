using Framework.FrameworkCore.Attributes.Logic;
using Framework.Managers;
using Gameplay.GameControllers.Entities;
using Gameplay.GameControllers.Penitent.Abilities;
using HarmonyLib;
using Tools.Level.Interactables;
using UnityEngine;

namespace Blasphemous.LostDreams.Effects;

public class HealthDrain
{
    private const float HIT_HEAL_AMOUNT = 5f;
    private const float KILL_HEAL_AMOUNT = 15f;
    private const float DRAIN_DELAY = 2f;
    private const float DRAIN_AMOUNT = 2f;
    private const float THORNS_AMOUNT = 10f;
    private const float CONTACT_AMOUNT = 3f;
    private const float SPEED_LENGTH = 30f;
    private const float SPEED_AMOUNT = 30f;

    private float _currentDrainDelay = 0f;
    private float _currentSpeedDelay = 0f;

    private readonly RawBonus _attackSpeedBonus = new(SPEED_AMOUNT);

    public bool ShouldDrainHealth => !IsUsingPrieDieu
        && Main.LostDreams.PenitenceHandler.IsActive("PE_LD01");

    public bool ShouldApplyThorns => Main.LostDreams.PenitenceHandler.IsActive("PE_LD01")
        || Main.LostDreams.ItemHandler.IsEquipped("RB551");

    public static bool IsUsingPrieDieu { get; set; }

    public HealthDrain()
    {
        Main.LostDreams.EventHandler.OnEnemyDamaged += HitEnemy;
        Main.LostDreams.EventHandler.OnEnemyKilled += KillEnemy;
        Main.LostDreams.EventHandler.OnPlayerDamaged += PlayerTakeDamage;
    }

    public void Update()
    {
        if (!ShouldDrainHealth || Core.Logic.Penitent == null)
        {
            _currentDrainDelay = DRAIN_DELAY;
            _currentSpeedDelay = 0f;
            return;
        }

        _currentDrainDelay -= Time.deltaTime;
        if (_currentSpeedDelay > 0)
            _currentSpeedDelay -= Time.deltaTime;

        if (_currentDrainDelay <= 0)
        {
            _currentDrainDelay = DRAIN_DELAY;
            Core.Logic.Penitent.Stats.Life.Current -= DRAIN_AMOUNT;
            if (Core.Logic.Penitent.Stats.Life.Current <= 0)
                Core.Logic.Penitent.KillInstanteneously();
        }

        if (_currentSpeedDelay < 0)
        {
            ChangeAttackSpeed(false);
        }
    }

    public void ChangeAttackSpeed(bool enable)
    {
        if (enable)
        {
            _currentSpeedDelay = SPEED_LENGTH;
            Core.Logic.Penitent.Stats.AttackSpeed.AddRawBonus(_attackSpeedBonus);
        }
        else
        {
            _currentSpeedDelay = 0f;
            Core.Logic.Penitent.Stats.AttackSpeed.RemoveRawBonus(_attackSpeedBonus);
        }
    }

    private void HealPlayer(float amount)
    {
        if (!ShouldDrainHealth)
            return;

        Core.Logic.Penitent.Stats.Life.Current += amount;
    }

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
            DamageAmount = THORNS_AMOUNT,
            DamageElement = DamageArea.DamageElement.Contact,
            DamageType = DamageArea.DamageType.Normal,
            AttackingEntity = Core.Logic.Penitent.gameObject
        });

        if (hit.DamageElement == DamageArea.DamageElement.Contact)
        {
            Main.LostDreams.Log("Reducing contact damage");
            hit.DamageAmount = CONTACT_AMOUNT;
        }
    }

    private void HitEnemy(ref Hit hit)
    {
        if (hit.AttackingEntity?.name == "Penitent(Clone)")
            HealPlayer(HIT_HEAL_AMOUNT);
    }

    private void KillEnemy()
    {
        HealPlayer(KILL_HEAL_AMOUNT);
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
