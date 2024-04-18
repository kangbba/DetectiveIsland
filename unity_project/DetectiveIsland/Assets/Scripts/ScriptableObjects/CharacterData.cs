using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class EmotionData
{
    [SerializeField] private string emotionID;
    [SerializeField] private Sprite emotionSprite;

    public string EmotionID => emotionID;
    public Sprite EmotionSprite => emotionSprite;
}

[System.Serializable]
public class CharacterData
{
    [SerializeField] private string _characterID;
    [SerializeField] private string _characterNameForUser;
    [SerializeField] private Color _characterColor;
    [SerializeField] private EmotionData[] _emotionDatas; // Array of emotions

    public string CharacterID => _characterID;
    public string CharacterNameForUser => _characterNameForUser;
    public Color CharacterColor => _characterColor;

    // Method to retrieve specific EmotionData
    public EmotionData GetEmotionData(string emotionID)
    {
        return _emotionDatas.FirstOrDefault(ed => ed.EmotionID == emotionID);
    }
}