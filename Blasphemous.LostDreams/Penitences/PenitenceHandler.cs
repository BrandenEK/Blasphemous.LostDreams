using System.Collections.Generic;

namespace Blasphemous.LostDreams.Penitences;

internal class PenitenceHandler
{
    private readonly List<string> _active = new();

    public void Activate(string penitence)
    {
        if (!_active.Contains(penitence))
        {
            Main.LostDreams.Log("Activating penitence: " + penitence);
            _active.Add(penitence);
        }
    }

    public void Deactivate(string penitence)
    {
        Main.LostDreams.Log("Deactivating penitence: " + penitence);
        _active.Remove(penitence);
    }

    public void Reset()
    {
        Main.LostDreams.Log("Clearing all active penitences");
        _active.Clear();
    }

    public bool IsActive(string penitence)
    {
        return _active.Contains(penitence);
    }
}
