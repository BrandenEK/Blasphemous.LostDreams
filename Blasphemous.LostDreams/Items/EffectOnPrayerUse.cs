using Blasphemous.Framework.Items;

namespace Blasphemous.LostDreams.Items;

internal class EffectOnPrayerUse : ModItemEffectOnPrayerUse
{
    public bool IsActive { get; private set; }

    public int FervourCost { get; } = 20; // these need custom

    protected override float EffectTime { get; } = 3; // these need custom

    protected override bool UsePrayerDurationModifier { get; } = true; // these need custom

    public EffectOnPrayerUse()
    {
        Main.LostDreams.EventHandler.OnExitGame += RemoveEffect;
    }

    protected virtual void OnActivate() { }
    protected virtual void OnDeactivate() { }
    protected virtual void OnUpdate() { }

    protected sealed override void ApplyEffect()
    {
        if (EffectTime > 0)
            IsActive = true;
        OnActivate();
    }

    protected sealed override void RemoveEffect()
    {
        IsActive = false;
        OnDeactivate();
    }

    protected sealed override void Update()
    {
        if (IsActive)
            OnUpdate();
    }
}
