
namespace Blasphemous.LostDreams.Items.Penitences;

internal class PenitenceList(Config cfg) : ItemList<Penitence>
{
    public Penitence PE501 { get; } = new PE501(cfg.PE501);
}
