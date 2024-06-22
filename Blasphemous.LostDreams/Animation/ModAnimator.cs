using UnityEngine;

namespace Blasphemous.LostDreams.Animation;

[RequireComponent(typeof(SpriteRenderer))]
public class ModAnimator : MonoBehaviour
{
    private AnimationInfo _animation;
    private float _nextUpdateTime;
    private int _currentIdx;

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

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (_animation == null || Time.time < _nextUpdateTime)
            return;

        if (++_currentIdx >= _animation.Sprites.Length - 1)
            _currentIdx = 0;

        sr.sprite = _animation.Sprites[_currentIdx];
    }

    private SpriteRenderer sr;
}
