
namespace Blasphemous.LostDreams.Items.RosaryBeads;

internal class RosaryBeadList(Config cfg) : ItemList<RosaryBead>
{
    public RosaryBead RB501 { get; } = new RosaryBead(new RB501());
    public RosaryBead RB502 { get; } = new RosaryBead(new RB502(cfg.RB502));
    public RosaryBead RB503 { get; } = new RosaryBead(new RB503());

    public RosaryBead RB506 { get; } = new RosaryBead(new RB506(cfg.RB506));

    public RosaryBead RB551 { get; } = new RosaryBead(new RB551());
}
