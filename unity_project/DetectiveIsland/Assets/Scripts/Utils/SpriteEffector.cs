using System.Collections;
using System.Collections.Generic;
using ArokaUtil;
using UnityEngine;

// SpriteEffector 클래스 정의
public class SpriteEffector : MonoBehaviour
{
    public void FadeIn(float totalTime)
    {
        Debug.Log("FadeIn 호출됨");
        transform.ArokaTr().SetSpriteRendererColor(Color.white.ModifiedAlpha(1f), totalTime);
    }
    public void FadeOut(float totalTime)
    {
        Debug.Log("FadeOut 호출됨");
        transform.ArokaTr().SetSpriteRendererColor(Color.white.ModifiedAlpha(0f), totalTime);
    }
    public void Red(float redStrengthPerone, float totalTime)
    {
        Debug.Log("FadeOut 호출됨");
        transform.ArokaTr().SetSpriteRendererColor(Color.red.ModifiedAlpha(redStrengthPerone), totalTime);
    }
}