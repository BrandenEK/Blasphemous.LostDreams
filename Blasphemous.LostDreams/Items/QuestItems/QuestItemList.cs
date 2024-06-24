
namespace Blasphemous.LostDreams.Items.QuestItems;

internal class QuestItemList(Config cfg) : ItemList<QuestItem>
{
    public QuestItem QI502 { get; } = new QuestItem(new QI502());
}
