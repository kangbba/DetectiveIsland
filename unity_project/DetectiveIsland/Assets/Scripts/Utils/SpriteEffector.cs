using System.Collections;
using System.Collections.Generic;
using Aroka.ArokaUtils;
using Aroka.EaseUtils;
using UnityEngine;

public class SpriteEffector : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;

    protected void SetSprite(Sprite sprite, int order){
        if(_spriteRenderer == null){
            return;
        }
        _spriteRenderer.sprite = sprite;
        _spriteRenderer.sortingOrder = order;
        
    }
    protected void FadeIn(float totalTime)
    {
        _spriteRenderer.EaseSpriteColor(Color.white.ModifiedAlpha(1f), totalTime);
    }
    protected void FadeInFromStart(float totalTime)
    {
        FadeOut(0f);
        FadeIn(totalTime);
    }
    protected void FadeOut(float totalTime)
    {
         _spriteRenderer.EaseSpriteColor(Color.white.ModifiedAlpha(0f), totalTime);
    }
    protected void Red(float redStrengthPerone, float totalTime)
    {
        Debug.Log("FadeOut 호출됨");
         _spriteRenderer.EaseSpriteColor(Color.red.ModifiedAlpha(0.3f), totalTime);
    }
    protected void RedRestore(float totalTime)
    {
        Red(0, totalTime);
    }

}