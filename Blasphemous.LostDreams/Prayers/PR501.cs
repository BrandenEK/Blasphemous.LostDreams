using Blasphemous.Framework.Items;

namespace Blasphemous.LostDreams.Prayers;

public class PR501 : ModItemEffectOnPrayerUse
{
    protected override float EffectTime { get; } = 0;

    protected override bool UsePrayerDurationModifier { get; } = false;

    protected override void ApplyEffect()
    {
        PerformSwap();
    }

    protected override void RemoveEffect() { }

    private void PerformSwap()
    {
        Main.LostDreams.Log("Swapping places with nearest enemy");
    }
}
