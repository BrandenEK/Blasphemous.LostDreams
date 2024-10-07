using Blasphemous.ModdingAPI;

namespace Blasphemous.LostDreams.Items.RosaryBeads;

internal class RB506 : EffectOnEquip
{
    protected override void OnEquip()
    {
        // Remove this
        ModLog.Error("Equipping health beams bead");
    }
}
