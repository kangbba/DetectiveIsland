using System.Collections;
using System.Collections.Generic;
using Aroka.ArokaUtils;
using Aroka.EaseUtils;
using UnityEngine;

// SpriteEffector 클래스 정의
public class SpriteEffector : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    public SpriteRenderer SpriteRenderer { get => _spriteRenderer; }

    protected void Initialize(){
        if(GetComponent<SpriteRenderer>() == null){
             gameObject.AddComponent<SpriteRenderer>();
        }
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }
    protected void SetSprite(Sprite sprite){
        _spriteRenderer.sprite = sprite;
    }
    protected void FadeIn(float totalTime)
    {
        _spriteRenderer.EaseSpriteRendererColor(Color.white.ModifiedAlpha(1f), totalTime);
    }
    protected void FadeInFromStart(float totalTime)
    {
        FadeOut(0f);
        FadeIn(totalTime);
    }
    protected void FadeOut(float totalTime)
    {
        Debug.Log("FadeOut 호출됨");
        _spriteRenderer.EaseSpriteRendererColor(Color.white.ModifiedAlpha(0f), totalTime);
    }
    protected void Red(float redStrengthPerone, float totalTime)
    {
        Debug.Log("FadeOut 호출됨");
        _spriteRenderer.EaseSpriteRendererColor(Color.red.ModifiedAlpha(redStrengthPerone), totalTime);
    }
    protected void RedRestore(float totalTime)
    {
        Red(0, totalTime);
    }

}