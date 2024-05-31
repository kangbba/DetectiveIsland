using Aroka.ArokaUtils;
using Aroka.EaseUtils;
using UnityEngine;
using UnityEngine.UI;

public class ArokaEffector : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Image _img;

    private enum ERendererType { None, SpriteRenderer, Image }

    private ERendererType RendererType 
    {
        get 
        {
            if (_spriteRenderer != null)
                return ERendererType.SpriteRenderer;
            if (_img != null)
                return ERendererType.Image;
            return ERendererType.None;
        }
    }

    public SpriteRenderer SpriteRenderer => _spriteRenderer;

    public void FadeIn(float totalTime)
    {
        Debug.Log(RendererType);
        switch (RendererType)
        {
            case ERendererType.SpriteRenderer:
                _spriteRenderer.EaseSpriteColor(Color.white.ModifiedAlpha(1f), totalTime);
                break;
            case ERendererType.Image:
                _img.transform.EaseColor(Color.white.ModifiedAlpha(1f), totalTime);
                break;
        }
    }

    public void FadeOut(float totalTime)
    {
        switch (RendererType)
        {
            case ERendererType.SpriteRenderer:
                _spriteRenderer.EaseSpriteColor(Color.white.ModifiedAlpha(0f), totalTime);
                break;
            case ERendererType.Image:
                _img.transform.EaseColor(Color.white.ModifiedAlpha(0f), totalTime);
                break;
        }
    }

    public void FadeInFromStart(float totalTime)
    {
        FadeOut(0f);
        FadeIn(totalTime);
    }

    public void FadeInFromBlack(float totalTime)
    {
        BlackOut(0f);
        FadeIn(totalTime);
    }

    public void FadeOutAndDestroy(float totalTime)
    {
        FadeOut(totalTime);
        Destroy(gameObject, totalTime);
    }

    public void BlackOut(float totalTime)
    {
        switch (RendererType)
        {
            case ERendererType.SpriteRenderer:
                _spriteRenderer.EaseSpriteColor(Color.black, totalTime);
                break;
            case ERendererType.Image:
                _img.transform.EaseColor(Color.black, totalTime);
                break;
        }
    }

    public void BlackOutAndDestroy(float totalTime)
    {
        BlackOut(totalTime);
        Destroy(gameObject, totalTime);
    }
}
