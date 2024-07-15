
namespace Blasphemous.LostDreams.Npc;

public class NpcInfo
{
    public string Id { get; }
    public string Animation { get; }
    public float ColliderWidth { get; }
    public float ColliderHeight { get; }

    public NpcInfo(string id, string animation, float colliderWidth, float colliderHeight)
    {
        Id = Main.Validate(id, x => !string.IsNullOrEmpty(x));
        Animation = Main.Validate(animation, x => !string.IsNullOrEmpty(x));
        ColliderWidth = Main.Validate(colliderWidth, x => x > 0);
        ColliderHeight = Main.Validate(colliderHeight, x => x > 0);
    }
}
