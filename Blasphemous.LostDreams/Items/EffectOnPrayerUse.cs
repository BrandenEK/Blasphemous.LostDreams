using Blasphemous.Framework.Items;
using Blasphemous.LostDreams.Items.Prayers;

namespace Blasphemous.LostDreams.Items;

internal class EffectOnPrayerUse : ModItemEffectOnPrayerUse
{
    public bool IsActive { get; private set; }

    public int FervourCost { get; }
    protected sealed override float EffectTime { get; }
    protected sealed override bool UsePrayerDurationModifier { get; }

    public EffectOnPrayerUse(IPrayerConfig cfg)
    {
        FervourCost = cfg.FervourCost;
        EffectTime = cfg.EffectTime;
        UsePrayerDurationModifier = cfg.UsePrayerDurationModifier;
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
