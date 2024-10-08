using Blasphemous.ModdingAPI;

namespace Blasphemous.LostDreams.Items.RosaryBeads;

internal class RB504 : EffectOnEquip
{
    protected override void OnEquip()
    {
        // Remove this
        ModLog.Error("Equipping perpetva bead!");
    }
}
