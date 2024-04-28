using Blasphemous.LostDreams.Effects;
using Framework.Managers;
using Gameplay.GameControllers.Entities;
using Gameplay.GameControllers.Penitent;

namespace Blasphemous.LostDreams.Swords;

internal class HE502 : IToggleEffect
{
    public bool IsActive => Main.LostDreams.ItemHandler.IsEquipped("HE502");

    public HE502()
    {
        Main.LostDreams.EventHandler.OnEnemyDamaged += OnEnemyDamaged;
    }

    private void OnEnemyDamaged(ref Hit hit)
    {
        if (!IsActive || hit.AttackingEntity?.name != "Penitent(Clone)")
            return;

        Penitent p = Core.Logic.Penitent;
        var anim = p.Animator.GetCurrentAnimatorStateInfo(0);

        bool usingSwordArt = p.LungeAttack.IsUsingAbility || p.VerticalAttack.IsUsingAbility
            || anim.IsName("Charged Attack") || anim.IsName("ComboFinisherUp") || anim.IsName("ComboFinisherDown");

        Main.LostDreams.Log($"HE502: Using sword art {usingSwordArt}");
        hit.DamageAmount *= usingSwordArt ? 1.5f : 0.8f;
    }
}
