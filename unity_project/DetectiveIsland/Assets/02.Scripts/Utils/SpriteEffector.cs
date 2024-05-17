using System.Collections;
using System.Collections.Generic;
using Aroka.ArokaUtils;
using Aroka.EaseUtils;
using UnityEngine;

public static class SpriteEffector
{
    public static void FadeIn(this SpriteRenderer spriteRend, float totalTime)
    {
        spriteRend.EaseSpriteColor(Color.white.ModifiedAlpha(1f), totalTime);
    }
    public static void FadeOut(this SpriteRenderer spriteRend, float totalTime)
    {
        spriteRend.EaseSpriteColor(Color.white.ModifiedAlpha(0f), totalTime);
    }
    public static void FadeInFromStart(this SpriteRenderer spriteRend, float totalTime)
    {
        FadeOut(spriteRend, 0f);
        FadeIn(spriteRend, totalTime);
    }
    public static void BeRed(this SpriteRenderer spriteRend,float redStrengthPerone, float totalTime)
    {
        Debug.Log("FadeOut 호출됨");
         spriteRend.EaseSpriteColor(Color.red.ModifiedAlpha(redStrengthPerone), totalTime);
    }
    public static void BeGray(this SpriteRenderer spriteRend,float redStrengthPerone, float totalTime)
    {
        Debug.Log("FadeOut 호출됨");
         spriteRend.EaseSpriteColor(Color.gray.ModifiedAlpha(redStrengthPerone), totalTime);
    }

}