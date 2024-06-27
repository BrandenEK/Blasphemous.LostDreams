
namespace Blasphemous.LostDreams.Items.RosaryBeads;

internal class RosaryBeadList(Config cfg) : ItemList<RosaryBead>
{
    public RosaryBead RB501 { get; } = new RosaryBead(new RB501());
    public RosaryBead RB502 { get; } = new RosaryBead(new RB502(cfg.RB502));
    public RosaryBead RB503 { get; } = new RosaryBead(new RB503());

    public RosaryBead RB510 { get; } = new RosaryBead(new RB510(cfg.RB510));
    public RosaryBead RB511 { get; } = new RosaryBead(new RB511(cfg.RB511));
    public RosaryBead RB512 { get; } = new RosaryBead(new RB512(cfg.RB512));
    public RosaryBead RB513 { get; } = new RosaryBead(new RB513(cfg.RB513));
    public RosaryBead RB514 { get; } = new RosaryBead(new RB514(cfg.RB514));

    public RosaryBead RB551 { get; } = new RosaryBead(new RB551());
}
