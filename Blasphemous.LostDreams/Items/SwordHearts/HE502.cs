using Blasphemous.ModdingAPI;
using Framework.Managers;
using Gameplay.GameControllers.Entities;
using Gameplay.GameControllers.Penitent;

namespace Blasphemous.LostDreams.Items.SwordHearts;

internal class HE502 : EffectOnEquip
{
    private readonly HE502Config _config;

    public HE502(HE502Config config)
    {
        _config = config;

        Main.LostDreams.EventHandler.OnEnemyDamaged += OnEnemyDamaged;
    }

    private void OnEnemyDamaged(ref Hit hit)
    {
        if (!IsEquipped || hit.AttackingEntity?.name != "Penitent(Clone)")
            return;

        Penitent p = Core.Logic.Penitent;
        var anim = p.Animator.GetCurrentAnimatorStateInfo(0);

        bool usingSwordArt = p.LungeAttack.IsUsingAbility || p.VerticalAttack.IsUsingAbility
            || anim.IsName("Charged Attack") || anim.IsName("ComboFinisherUp") || anim.IsName("ComboFinisherDown");

        ModLog.Info($"HE502: Using sword art [{usingSwordArt}]");
        hit.DamageAmount *= usingSwordArt ? _config.SWORDART_MULTIPLIER : _config.STANDARD_MULTIPLIER;
    }
}

/// <summary> Properties for HE501 </summary>
public class HE502Config
{
    /// <summary> Damage multiplier for regular attacks </summary>
    public float STANDARD_MULTIPLIER { get; set; } = 0.7f;
    /// <summary> Damage multiplier for special attacks </summary>
    public float SWORDART_MULTIPLIER { get; set; } = 1.4f;
}
