using System.Collections.Generic;
using System.Linq;

namespace Blasphemous.LostDreams.Items;

internal class ItemList<T>
{
    public IEnumerable<T> Items
    {
        get
        {
            return GetType()
                .GetProperties()
                .Where(p => p.PropertyType == typeof(T))
                .Select(p => (T)p.GetValue(this, null));
        }
    }
}
