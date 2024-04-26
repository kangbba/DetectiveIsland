using System.Linq;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private CharacterEmotion[] _characterEmotions; // 감정 상태 배열
    private CharacterEmotion _curCharacterEmotion; // 감정 상태 배열

    private string _characterID;
    public string CharacterID { get => _characterID; }

    public void Initialize(string characterID)
    {
        _characterID = characterID;
        foreach (var emotion in _characterEmotions)
        {
            emotion.SetOn(false, 0f); // 나머지 감정은 즉시 숨김
        }
    }

    public void FadeOutAndDestroy(float totalTime){
        foreach(var emotion in _characterEmotions)
        {
            emotion.SetOn(false, totalTime);
            Destroy(emotion.gameObject, totalTime);
        }
    }

    public void ChangeEmotion(string targetEmotionID, float fadeTime)
    {
        CharacterEmotion targetEmotion = GetCharacterEmotion(targetEmotionID);
        if(targetEmotion == null){
            return;
        }
        if(_curCharacterEmotion != null && _curCharacterEmotion.EmotionID == targetEmotionID){
            return;
        }
        if(_curCharacterEmotion != null){
            _curCharacterEmotion.SetOn(false, fadeTime);
        }
        _curCharacterEmotion = targetEmotion;
        _curCharacterEmotion.SetOn(true, fadeTime);
    }

    public CharacterEmotion GetCharacterEmotion(string emotionID){
        return _characterEmotions.FirstOrDefault(emotion => emotion.EmotionID == emotionID );
    }

    public void Exit(float totalTime){
        foreach (var emotion in _characterEmotions)
        {
            emotion.SetOn(false, totalTime); // 나머지 감정은 즉시 숨김
        }
    }


    public void StartTalking(){
        Debug.Log("로그3");
        _curCharacterEmotion.StartTalking(5f);
    }

    public void StopTalking(){
        Debug.Log("로그4");
        _curCharacterEmotion.StopTalking();
    }
}
