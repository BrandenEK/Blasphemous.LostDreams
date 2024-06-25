using Blasphemous.Framework.Items;

namespace Blasphemous.LostDreams.Items;

internal class EffectOnAcquire : ModItemEffectOnAcquire
{
    public bool IsAcquired { get; private set; }

    public EffectOnAcquire()
    {
        Main.LostDreams.EventHandler.OnExitGame += RemoveEffect;
    }

    protected virtual void OnAcquire() { }
    protected virtual void OnUpdate() { }

    protected sealed override void ApplyEffect()
    {
        IsAcquired = true;
        OnAcquire();
    }

    protected sealed override void RemoveEffect()
    {
        IsAcquired = false;
    }

    protected sealed override void Update()
    {
        if (IsAcquired)
            OnUpdate();
    }

    protected override bool ActivateOnce { get; } = false;
}
