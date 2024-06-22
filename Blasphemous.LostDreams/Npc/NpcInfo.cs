
namespace Blasphemous.LostDreams.Npc;

public class NpcInfo
{
    public string Id { get; }
    public string Animation { get; }
    public float ColliderWidth { get; }
    public float ColliderHeight { get; }

    public NpcInfo(string id, string animation, float colliderWidth, float colliderHeight)
    {
        Id = id;
        Animation = animation;
        ColliderWidth = colliderWidth;
        ColliderHeight = colliderHeight;
    }
}
