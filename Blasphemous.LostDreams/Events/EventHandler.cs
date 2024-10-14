using Gameplay.GameControllers.Entities;
using Gameplay.GameControllers.Penitent;
using Gameplay.GameControllers.Penitent.Attack;

namespace Blasphemous.LostDreams.Events;

internal class EventHandler
{
    public delegate void EventDelegate();

    public delegate void StandardEvent();
    public delegate void HitEvent(ref Hit hit);
    public delegate void EntityEvent(Entity entity);
    public delegate void CancellableEvent(ref bool cancel);
    public delegate void PenitentSwordAttackEvent(PenitentSword.AttackType attackType);

    public event StandardEvent OnUsePrieDieu;
    public event StandardEvent OnExitGame;

    public event HitEvent OnPlayerDamaged;
    public event HitEvent OnEnemyDamaged;

    public event StandardEvent OnPlayerKilled;
    public event StandardEvent OnEnemyKilled;

    public event CancellableEvent OnUseFlask;

    public event PenitentSwordAttackEvent OnSwordAttack;

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

    public bool UseFlask()
    {
        bool cancel = false;
        OnUseFlask?.Invoke(ref cancel);
        return cancel;
    }

    public void SwordAttack(PenitentSword.AttackType attackType)
    {
        OnSwordAttack?.Invoke(attackType);
    }
}
