using Blasphemous.LostDreams.Effects;
using Framework.Managers;
using Gameplay.GameControllers.Entities;
using Gameplay.GameControllers.Penitent;

namespace Blasphemous.LostDreams.Swords;

public class HE502 : IToggleEffect
{
    private readonly Config _config;

    public bool IsActive => Main.LostDreams.ItemHandler.IsEquipped("HE502");

    public HE502(Config config)
    {
        Main.LostDreams.EventHandler.OnEnemyDamaged += OnEnemyDamaged;
        _config = config;
    }

    private void OnEnemyDamaged(ref Hit hit)
    {
        if (!IsActive || hit.AttackingEntity?.name != "Penitent(Clone)")
            return;

        Penitent p = Core.Logic.Penitent;
        var anim = p.Animator.GetCurrentAnimatorStateInfo(0);

        bool usingSwordArt = p.LungeAttack.IsUsingAbility || p.VerticalAttack.IsUsingAbility
            || anim.IsName("Charged Attack") || anim.IsName("ComboFinisherUp") || anim.IsName("ComboFinisherDown");

        Main.LostDreams.Log($"HE502: Using sword art [{usingSwordArt}]");
        hit.DamageAmount *= usingSwordArt ? _config.SWORDART_MULTIPLIER : _config.STANDARD_MULTIPLIER;
    }

    public class Config
    {
        public float STANDARD_MULTIPLIER { get; set; } = 0.8f;
        public float SWORDART_MULTIPLIER { get; set; } = 1.5f;
    }
}
