using Blasphemous.LostDreams.Animation;
using UnityEngine;

namespace Blasphemous.LostDreams.Components;

/// <summary>
/// Component that plays animations on a SpriteRenderer
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class ModAnimator : MonoBehaviour
{
    private float _nextUpdateTime;
    private int _currentIdx;

    /// <summary>
    /// The animation to play
    /// </summary>
    public AnimationInfo Animation
    {
        get => _animation;
        set
        {
            _animation = value;
            if (value != null)
            {
                sr.sprite = _animation.Sprites[0];
                _nextUpdateTime = Time.time + _animation.SecondsPerFrame;
                _currentIdx = 0;
            }
        }
    }
    private AnimationInfo _animation = null;

    /// <summary>
    /// Whether the animation should start over once it reaches the end
    /// </summary>
    public bool Loop
    {
        get => _loop;
        set => _loop = value;
    }
    private bool _loop = true;

    /// <summary>
    /// The percent of the way through the animation
    /// </summary>
    public float NormalizedTime
    {
        get => 0;
        set
        {

        }
    }

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (_animation == null || Time.time < _nextUpdateTime)
            return;

        if (!_loop && _currentIdx == _animation.Sprites.Length - 1)
            return;

        if (++_currentIdx > _animation.Sprites.Length - 1)
            _currentIdx = 0;

        sr.sprite = _animation.Sprites[_currentIdx];
        _nextUpdateTime = Time.time + _animation.SecondsPerFrame;
    }

    private SpriteRenderer sr;
}
