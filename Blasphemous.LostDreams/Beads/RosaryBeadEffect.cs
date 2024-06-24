using Blasphemous.Framework.Items;

namespace Blasphemous.LostDreams.Beads;

internal class RosaryBeadEffect : ModItemEffectOnEquip
{
    public bool IsEquipped { get; private set; }

    public RosaryBeadEffect()
    {
        Main.LostDreams.EventHandler.OnExitGame += RemoveEffect;
    }

    protected override void ApplyEffect()
    {
        IsEquipped = true;
    }

    protected override void RemoveEffect()
    {
        IsEquipped = false;
    }
}
