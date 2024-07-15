
namespace Blasphemous.LostDreams.Items.Prayers;

internal class PrayerList(Config cfg) : ItemList<Prayer>
{
    public Prayer PR501 { get; } = new Prayer(new PR501(cfg));
}
