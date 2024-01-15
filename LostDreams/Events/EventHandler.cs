using Gameplay.GameControllers.Entities;
using Gameplay.GameControllers.Penitent;

namespace LostDreams.Events;

internal class EventHandler
{
    public delegate void EventDelegate();

    public event EventDelegate OnPlayerKilled;
    public event EventDelegate OnEnemyKilled;
    public event EventDelegate OnPlayerDamaged;
    public event EventDelegate OnEnemyDamaged;

    public event EventDelegate OnUsePrieDieu;
    public event EventDelegate OnExitGame;

    public void KillEntity(Entity entity)
    {
        if (entity is Penitent)
            OnPlayerKilled?.Invoke();
        else
            OnEnemyKilled?.Invoke();
    }

    public void DamagePlayer()
    {
        OnPlayerDamaged?.Invoke();
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
