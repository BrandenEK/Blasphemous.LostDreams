using Gameplay.GameControllers.Entities;
using System;

namespace Blasphemous.LostDreams.Effects;

/// <summary>
/// Reset when getting hit, increase charges when killing
/// </summary>
internal class DamageStack : IMultiplierEffect
{
    private readonly RB502Config _config;

    private int _currentCharges;

    public float Multiplier => 1 + (_config.MAX_MULTIPLIER - 1) * ((float)_currentCharges / _config.MAX_CHARGES);

    public DamageStack(RB502Config config)
    {
        _config = config;

        Main.LostDreams.EventHandler.OnPlayerDamaged += PlayerTakeDamage;
        Main.LostDreams.EventHandler.OnEnemyDamaged += EnemyTakeDamage;
        Main.LostDreams.EventHandler.OnEnemyKilled += IncreaseCharges;
        Main.LostDreams.EventHandler.OnExitGame += ResetCharges;
    }

    private void IncreaseCharges()
    {
        if (!Main.LostDreams.ItemHandler.IsEquipped("RB502"))
            return;

        _currentCharges = Math.Min(_currentCharges + 1, _config.MAX_CHARGES);
        Main.LostDreams.Log($"RB502: Increasing damage stack to {_currentCharges}");
    }

    private void ResetCharges()
    {
        _currentCharges = 0;
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
}

/// <summary> Properties for RB502 </summary>
public class RB502Config
{
    /// <summary> The total number of charges you can go up to </summary>
    public int MAX_CHARGES = 20;
    /// <summary> The multiplier added to damage when you have maximum charges </summary>
    public float MAX_MULTIPLIER = 2.0f;
}
