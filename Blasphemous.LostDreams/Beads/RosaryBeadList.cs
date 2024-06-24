using System.Collections.Generic;

namespace Blasphemous.LostDreams.Beads;

internal class RosaryBeadList
{
    public IEnumerable<RosaryBead> Items { get; }

    public RosaryBead RB501 { get; }
    public RosaryBead RB502 { get; }

    public RosaryBeadList(Config cfg)
    {
        Items = new RosaryBead[]
        {
            RB501 = new(new RB501()),
            RB502 = new(new RB502(cfg.RB502)),
        };
    }
}
