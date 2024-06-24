using Blasphemous.Framework.Items;

namespace Blasphemous.LostDreams.Items;

internal class EquipEffect : ModItemEffectOnEquip
{
    public bool IsEquipped { get; private set; }

    public EquipEffect()
    {
        Main.LostDreams.EventHandler.OnExitGame += RemoveEffect;
    }

    protected virtual void OnEquip() { }
    protected virtual void OnUnequip() { }
    protected virtual void OnUpdate() { }

    protected sealed override void ApplyEffect()
    {
        IsEquipped = true;
        OnEquip();
    }

    protected sealed override void RemoveEffect()
    {
        IsEquipped = false;
        OnUnequip();
    }

    protected sealed override void Update()
    {
        if (IsEquipped)
            OnUpdate();
    }
}