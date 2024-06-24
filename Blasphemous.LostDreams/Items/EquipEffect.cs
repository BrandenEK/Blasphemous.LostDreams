using Blasphemous.Framework.Items;

namespace Blasphemous.LostDreams.Items;

internal class EquipEffect : ModItemEffectOnEquip
{
    public bool IsEquipped { get; private set; }

    public EquipEffect()
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