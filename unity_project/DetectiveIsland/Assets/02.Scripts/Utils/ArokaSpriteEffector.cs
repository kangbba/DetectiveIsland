using Aroka.ArokaUtils;
using Aroka.EaseUtils;
using UnityEngine;

public class ArokaSpriteEffector : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;

    public SpriteRenderer SpriteRenderer { get => _spriteRenderer;}

    public void Initialize(SpriteRenderer spriteRenderer)
    {
        _spriteRenderer = spriteRenderer;
    }

    public void FadeIn(float totalTime)
    {
        _spriteRenderer.EaseSpriteColor(Color.white.ModifiedAlpha(1f), totalTime);
    }

    public void FadeOut(float totalTime)
    {
        _spriteRenderer.EaseSpriteColor(Color.white.ModifiedAlpha(0f), totalTime);
    }

    public void FadeInFromStart(float totalTime)
    {
        FadeOut(0f);
        FadeIn(totalTime);
    }

    public void FadeOutAndDestroy(float totalTime)
    {
        _spriteRenderer.EaseSpriteColor(_spriteRenderer.color.ModifiedAlpha(0f), totalTime);
    }
}
