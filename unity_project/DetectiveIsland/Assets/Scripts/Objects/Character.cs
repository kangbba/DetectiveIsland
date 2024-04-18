
using UnityEngine;

public class Character : SpriteEffector
{
    [SerializeField] private CharacterData _characterData;

    public CharacterData CharacterData { get => _characterData; }


    // 캐릭터 초기화 메서드
    public void Initialize()
    {
        base.FadeOut(0f);
        SetEmotion("Smile");
        
        ////
        ///
    }

    public void SetEmotion(string emotionID)
    {
        EmotionData emotionData = _characterData.GetEmotionData(emotionID);

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