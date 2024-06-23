using Framework.Managers;
using Gameplay.GameControllers.Entities;
using Gameplay.GameControllers.Penitent;
using Gameplay.GameControllers.Penitent.Abilities;

namespace Blasphemous.LostDreams.Events;

internal class EventHandler
{
    public delegate void EventDelegate();

    public delegate void StandardEvent();
    public delegate void HitEvent(ref Hit hit);
    public delegate void EntityEvent(Entity entity);
    public delegate void ParryEvent(Parry parry);
    public delegate void GuardSlideEvent(GuardSlide guadeSlide);

    public event StandardEvent OnUsePrieDieu;
    public event StandardEvent OnExitGame;

    public event HitEvent OnPlayerDamaged;
    public event HitEvent OnEnemyDamaged;

    public event StandardEvent OnPlayerKilled;
    public event StandardEvent OnEnemyKilled;

    public event ParryEvent OnParryStart;
    public event ParryEvent OnParryRiposte;
    public event GuardSlideEvent OnParryGuardSlide;
    public event ParryEvent OnParryFail;

    public void KillEntity(Entity entity)
    {
        if (entity is Penitent)
            OnPlayerKilled?.Invoke();
        else
            OnEnemyKilled?.Invoke();
    }

    public void DamagePlayer(ref Hit hit)
    {
        OnPlayerDamaged?.Invoke(ref hit);
    }

    public void DamageEnemy(ref Hit hit)
    {
        OnEnemyDamaged?.Invoke(ref hit);
    }

    public void UsePrieDieu()
    {
        OnUsePrieDieu?.Invoke();
    }

    public void Reset()
    {
        OnExitGame?.Invoke();
    }

    public void ParryStart(Parry parry)
    {
        OnParryStart?.Invoke(parry);
    }

    public void ParryRiposte(Parry parry)
    {
        if (parry.SuccessParry) { OnParryRiposte?.Invoke(parry); }
    }

    public void ParryGuardSlide(GuardSlide guardSlide)
    {
        OnParryGuardSlide?.Invoke(guardSlide);
    }

    public void ParryFail(Parry parry)
    {
        if (!parry.SuccessParry && !Core.Logic.Penitent.GuardSlide.Casting)
        { OnParryFail?.Invoke(parry); }
    }
}
