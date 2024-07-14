using Framework.FrameworkCore.Attributes.Logic;
using Framework.Managers;
using Gameplay.GameControllers.Entities;

namespace Blasphemous.LostDreams.Items;

internal class EffectOnEquipStat(EntityStats.StatsTypes statType, float statValue) : EffectOnEquip
{
    private readonly EntityStats.StatsTypes _statType = statType;
    private readonly RawBonus _statBonus = new(statValue);

    protected override void OnEquip()
    {
        Core.Logic.Penitent.Stats.GetByType(_statType).AddRawBonus(_statBonus);
    }

    protected override void OnUnequip()
    {
        Core.Logic.Penitent.Stats.GetByType(_statType).RemoveRawBonus(_statBonus);
    }
}
