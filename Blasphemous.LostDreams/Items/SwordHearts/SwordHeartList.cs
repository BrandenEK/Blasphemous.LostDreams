
namespace Blasphemous.LostDreams.Items.SwordHearts;

internal class SwordHeartList(Config cfg) : ItemList<SwordHeart>
{
    public SwordHeart HE501 { get; } = new SwordHeart(new HE501(cfg.HE501));
    public SwordHeart HE502 { get; } = new SwordHeart(new HE502(cfg.HE502));
}
