using Framework.FrameworkCore.Attributes.Logic;
using Framework.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blasphemous.LostDreams.Items.RosaryBeads;

internal class RB511 : EffectOnEquip
{

    private RawBonus _flaskCountBonus;

    public RB511(RB511Config config)
    {
        _flaskCountBonus = new(config.FLASK_AMOUNT_BUFF);
    }

    protected override void OnEquip()
    {
        Core.Logic.Penitent.Stats.Flask.AddRawBonus(_flaskCountBonus);
    }

    protected override void OnUnequip()
    {
        Core.Logic.Penitent.Stats.Flask.RemoveRawBonus(_flaskCountBonus);
    }
}
/// <summary> 
/// Properties for RB511 
/// </summary>
public class RB511Config
{
    public int FLASK_AMOUNT_BUFF = 1;
}