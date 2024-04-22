
using UnityEngine;

public class Character : SpriteEffector
{
    [SerializeField] private CharacterData _characterData;

    public CharacterData CharacterData { get => _characterData; }


    // 캐릭터 초기화 메서드
    public void Initialize()
    {
        SetEmotion("Smile");
    }

    public void SetEmotion(string emotionID)
    {
        EmotionData emotionData = _characterData.GetEmotionData(emotionID);
        if(emotionData == null || emotionData.EmotionSprite == null){
            Debug.LogWarning("emotionData 가 없거나, emotionData에 해당하는 sprite 없음");
        }
        base.SetSprite(emotionData.EmotionSprite, 0);
    }
    public void SetOn(bool b, float totalTime)
    {
        if (b)
        {
            base.FadeIn(totalTime);
        }
        else
        {
            base.FadeOut(totalTime);
        }
    }  
    private void OnMouseDown()
    {
    }

    private void OnMouseEnter()
    {
    }

    private void OnMouseExit()
    {
    }
}