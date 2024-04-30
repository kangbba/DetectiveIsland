using System.Linq;
using Aroka.EaseUtils;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private CharacterEmotion[] _characterEmotions; // 감정 상태 배열
    private CharacterEmotion _curCharacterEmotion; // 감정 상태 배열

    private string _characterID;
    private string _positionID;
    public string CharacterID { get => _characterID; }
    public string PositionID { get => _positionID; }

    public void Initialize(string characterID, string positionID)
    {
        _positionID = positionID;
        SetPos(_positionID, 0f);
        
        _characterID = characterID;
        foreach (var emotion in _characterEmotions)
        {
            emotion.gameObject.SetActive(true);
            emotion.SetOn(false, 0f); // 나머지 감정은 즉시 숨김
        }
    }

    public void SetPos(string positionID, float totalTime){
        Vector3 targetLocalPos = CalculatePosition(positionID);
        transform.EaseLocalPos(targetLocalPos, totalTime); ;
    }

    public void FadeInCurrentEmotion(float totalTime){
        if(_curCharacterEmotion == null){
            Debug.LogWarning("_curCharacterEmotion NULL");  
            return;
        }
        _curCharacterEmotion.SetOn(true, totalTime);
    }
    public void FadeOutCurrentEmotion(float totalTime){
        if(_curCharacterEmotion == null){
            Debug.LogWarning("_curCharacterEmotion NULL");    
            return;
        }
        _curCharacterEmotion.SetOn(false, totalTime);
    }
    
    //Fade In 대신 SetEmotion 하면 원하는거 됩니다.    
    public void SetEmotion(string targetEmotionID, float fadeTime)
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
        FadeInCurrentEmotion(fadeTime);
    }

    public void FadeOutAndDestroy(float totalTime){
        FadeOutCurrentEmotion(totalTime);
        Destroy(gameObject, totalTime);
    }
    public CharacterEmotion GetCharacterEmotion(string emotionID){
        return _characterEmotions.FirstOrDefault(emotion => emotion.EmotionID == emotionID );
    }
    public void StartTalking(){
        _curCharacterEmotion.StartTalking(5f);
    }

    public void StopTalking(){
        _curCharacterEmotion.StopTalking();
    }
    private static Vector3 CalculatePosition(string positionID)
    {
        Vector3 newPosition = Vector3.zero;
        switch (positionID)
        {
            case "Left":
                newPosition = new Vector3(-8f, 0f, 0f);
                break;
            case "Middle":
                newPosition = new Vector3(0f, 0f, 0f);
                break;
            case "Right":
                newPosition = new Vector3(8f, 0f, 0f);
                break;
        }
        return newPosition;
    }
}
