using Framework.Managers;
using Gameplay.GameControllers.Entities;
using HarmonyLib;
using Tools.Level.Interactables;
using UnityEngine;

namespace Blasphemous.LostDreams.Effects;

public class HealthDrain
{
    private float _currentDrainDelay = 0f;

    public bool ShouldDrainHealth => !IsUsingPrieDieu
        && Main.LostDreams.PenitenceHandler.IsActive("PE_LD01");

    public bool ShouldApplyThorns => Main.LostDreams.PenitenceHandler.IsActive("PE_LD01")
        || Main.LostDreams.ItemHandler.IsEquipped("RB551");

    public static bool IsUsingPrieDieu { get; set; }

    public HealthDrain()
    {
        Main.LostDreams.EventHandler.OnEnemyDamaged += (ref Hit hit) => HealPlayer(HIT_HEAL_AMOUNT);
        Main.LostDreams.EventHandler.OnEnemyKilled += () => HealPlayer(KILL_HEAL_AMOUNT);
        Main.LostDreams.EventHandler.OnPlayerDamaged += PlayerTakeDamage;
    }

    public void Update()
    {
        if (!ShouldDrainHealth || Core.Logic.Penitent == null)
        {
            _currentDrainDelay = DRAIN_DELAY;
            return;
        }

        _currentDrainDelay -= Time.deltaTime;
        if (_currentDrainDelay <= 0)
        {
            _currentDrainDelay = DRAIN_DELAY;
            Core.Logic.Penitent.Stats.Life.Current -= DRAIN_AMOUNT;
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
        });

        if (hit.DamageElement == DamageArea.DamageElement.Contact)
        {
            Main.LostDreams.Log("Reducing contact damage");
            hit.DamageAmount = CONTACT_AMOUNT;
        }
    }

    private const float HIT_HEAL_AMOUNT = 5f;
    private const float KILL_HEAL_AMOUNT = 15f;
    private const float DRAIN_DELAY = 2f;
    private const float DRAIN_AMOUNT = 2f;
    private const float THORNS_AMOUNT = 10f;
    private const float CONTACT_AMOUNT = 3f;
}

// Control flag for when a prie dieu is in use
[HarmonyPatch(typeof(PrieDieu), "OnUpdate")]
class PrieDieu_Update_Patch
{
    public static void Postfix(PrieDieu __instance) => HealthDrain.IsUsingPrieDieu = __instance.BeingUsed;
}
