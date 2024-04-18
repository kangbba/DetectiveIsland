
using UnityEngine;

public class Character : SpriteEffector
{
    [SerializeField] private CharacterData _characterData;

    public CharacterData CharacterData { get => _characterData; }


    // 캐릭터 초기화 메서드
    public void Initialize()
    {
        SetEmotion("Smile");
        base.FadeOut(0f);
    }

    public void SetEmotion(string emotionID)
    {
        EmotionData emotionData = _characterData.GetEmotionData(emotionID);
        if(emotionData == null || emotionData.EmotionSprite == null){
            Debug.LogWarning("emotionData 가 없거나, emotionData에 해당하는 sprite 없음");
        }
        base.SetSprite(emotionData.EmotionSprite);
    }

    private void OnMouseDown()
    {
        Debug.Log("Character clicked: " + gameObject.name);
    }

    public void SetOn(bool b, float totalTime)
    {
        if (b)
        {
            base.FadeInFromStart(totalTime);
        }
        else
        {
            base.FadeOut(totalTime);
        }
    }
}