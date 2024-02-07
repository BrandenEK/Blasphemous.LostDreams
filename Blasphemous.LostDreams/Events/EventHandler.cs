using Framework.Managers;
using Gameplay.GameControllers.Entities;
using Gameplay.GameControllers.Penitent;

namespace Blasphemous.LostDreams.Events;

internal class EventHandler
{
    public delegate void EventDelegate();

    public delegate void StandardEvent();
    public delegate void HitEvent(ref Hit hit);
    public delegate void EntityEvent(Entity entity);

    public event StandardEvent OnUsePrieDieu;
    public event StandardEvent OnExitGame;

    public event HitEvent OnPlayerDamaged;
    public event HitEvent OnEnemyDamaged;

    public event StandardEvent OnPlayerKilled;
    public event StandardEvent OnEnemyKilled;

    public void KillEntity(Entity entity)
    {
        if (entity is Penitent)
            OnPlayerKilled?.Invoke();
        else
            OnEnemyKilled?.Invoke();
    }

    public void DamagePlayer(ref Hit hit)
    {
        if (Core.Logic.Penitent.Status.Unattacable)
            return;

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
}
