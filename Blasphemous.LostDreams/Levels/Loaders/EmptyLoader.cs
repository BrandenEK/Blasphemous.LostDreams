using Blasphemous.Framework.Levels.Loaders;
using System.Collections;
using UnityEngine;

namespace Blasphemous.LostDreams.Levels.Loaders;

/// <summary>
/// Just creates a new GameObject
/// </summary>
internal class EmptyLoader(string name) : ILoader
{
    public GameObject Result { get; private set; }

    public IEnumerator Apply()
    {
        Result = new GameObject(name);
        yield break;
    }
}
