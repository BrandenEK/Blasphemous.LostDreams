using UnityEngine;

namespace Blasphemous.LostDreams.Components;

/// <summary>
/// Component that destroys itself when the animation is finished
/// </summary>
[RequireComponent(typeof(ModAnimator))]
public class ModVanisher : MonoBehaviour
{
    private void Start()
    {
        _anim = GetComponent<ModAnimator>();
    }

    private void Update()
    {
        if (_anim.NormalizedTime < 1)
            return;

        Main.LostDreams.Log($"Vanishing: {name}");
        Destroy(gameObject);
    }

    private ModAnimator _anim;
}
