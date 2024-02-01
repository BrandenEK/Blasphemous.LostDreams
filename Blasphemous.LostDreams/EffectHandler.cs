using System.Collections.Generic;

namespace LostDreams;

internal class EffectHandler
{
    private readonly List<string> _activeEffects = new();

    public void Activate(string effect)
    {
        if (!_activeEffects.Contains(effect))
        {
            Main.Blasphemous.LostDreams.Log("Activating effect: " + effect);
            _activeEffects.Add(effect);
        }
    }

    public void Deactivate(string effect)
    {
        Main.Blasphemous.LostDreams.Log("Deactivating effect: " + effect);
        _activeEffects.Remove(effect);
    }

    public void Reset()
    {
        Main.Blasphemous.LostDreams.Log("Clearing all item effects");
        _activeEffects.Clear();
    }

    public bool IsActive(string effect)
    {
        return _activeEffects.Contains(effect);
    }
}
