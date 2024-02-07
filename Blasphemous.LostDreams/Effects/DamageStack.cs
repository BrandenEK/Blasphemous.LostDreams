using Gameplay.GameControllers.Entities;
using System;

namespace Blasphemous.LostDreams.Effects;

public class DamageStack : IMultiplierEffect
{
    private int _charges;

    public float Multiplier => 1 + (MAX_MULTIPLIER - 1) * ((float)_charges / MAX_CHARGES);

    public DamageStack()
    {
        Main.LostDreams.EventHandler.OnPlayerDamaged += PlayerTakeDamage;
        Main.LostDreams.EventHandler.OnEnemyDamaged += EnemyTakeDamage;
        Main.LostDreams.EventHandler.OnEnemyKilled += IncreaseCharges;
        Main.LostDreams.EventHandler.OnExitGame += ResetCharges;
    }

    private void IncreaseCharges()
    {
        if (!Main.LostDreams.ItemHandler.IsEquipped("RB502"))
            return;

        _charges = Math.Min(_charges + 1, MAX_CHARGES);
        Main.LostDreams.Log($"RB502: Increasing damage stack to {_charges}");
    }

    private void ResetCharges()
    {
        _charges = 0;
        Main.LostDreams.Log("RB502: Resetting damage stack");
    }

    private void PlayerTakeDamage(ref Hit hit)
    {
        if (!Main.LostDreams.ItemHandler.IsEquipped("RB502"))
            return;

        hit.DamageAmount *= Main.LostDreams.DamageStack.Multiplier;
        ResetCharges();
    }

    private void EnemyTakeDamage(ref Hit hit)
    {
        if (!Main.LostDreams.ItemHandler.IsEquipped("RB502"))
            return;

        hit.DamageAmount *= Main.LostDreams.DamageStack.Multiplier;
    }

    private const int MAX_CHARGES = 20;
    private const float MAX_MULTIPLIER = 2.0f;
}
