using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class WorldManager
{
    private static Transform _placePanel;
    private static Place _curPlace;
    public static Place CurPlace { get => _curPlace; }
    private static Transform _characterPanel;
    private static Transform _garbagePanel;
    private static List<Character> _curCharacters = new List<Character>();

    public static List<Character> CurCharacters { get => _curCharacters;  }
    public static Transform GarbagePanel { get => _garbagePanel; }

    public static void Load(){
        _placePanel = new GameObject("Place Panel").transform;
        _characterPanel = new GameObject("Character Panel").transform;
        _garbagePanel = new GameObject("Garbage Panel").transform;
    }

    public static void SetPlace(string _placeID){
        _curPlace = InstantiatePlace(_placeID);
    }
    public static void CurPlaceFadeIn(float totalTime){
        if(_curPlace == null){
            Debug.LogWarning("_curPlace is null");
            return;
        }
        _curPlace.FadeIn(totalTime);
    } 
    
    public static void CurPlaceFadeInFromStart(float totalTime){
        if(_curPlace == null){
            Debug.LogWarning("_curPlace is null");
            return;
        }
        _curPlace.FadeInFromStart(totalTime);
    }

    public static void CurPlaceFadeOut(float totalTime){
        if(_curPlace == null){
            Debug.LogWarning("_curPlace is null");
            return;
        }
        _curPlace.FadeOut(totalTime);
    }
    public static void CurPlaceFadeOutThenDestroy(float totalTime){
        if(_curPlace == null){
            Debug.LogWarning($"_curPlace 이 없음");
            return;
        }
        _curPlace.FadeOutAndDestroy(totalTime);
    }
    public static Place InstantiatePlace(string placeID){

        Place placePrefab = PlaceService.GetPlacePrefab(placeID);
        if(placePrefab  == null){
            Debug.LogWarning($"{placeID}에 해당하는 Place Prefab 찾을 수 없음");
        }
        Place instancedPlace = GameObject.Instantiate(placePrefab, _placePanel.transform);
        instancedPlace.transform.localPosition = Vector3.zero;
        instancedPlace.Initialize();
        instancedPlace.FadeOut(0f);
        return instancedPlace;
    }

    public static Character GetInstancedCharacter(string characterID){

        return _curCharacters.FirstOrDefault(character => character.CharacterID == characterID);

    }
    public static void InstantiateCharacterThenFadeIn(string characterID, string positionID, string initialEmotionID, float totalTime)
    {
        CharacterData characterData = CharacterService.GetCharacterData(characterID);

        if (characterData == null)
        {
            Debug.LogWarning("characterPlan not found: " + characterID);
            return;
        }

        Character characterInstance = GameObject.Instantiate(characterData.CharacterPrefab, _characterPanel.transform);
        characterInstance.Initialize(characterData.CharacterID, CalculatePosition(positionID) + Vector3.right * CurPlace.CurPagePlan.XPoint);
        characterInstance.SetEmotion(initialEmotionID, totalTime);
        _curCharacters.Add(characterInstance);
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

    public static void FadeOutCharacterThenDestroy(string characterID, float totalTime)
    {
        Character character = GetInstancedCharacter(characterID);
        if (character != null)
        {
            character.FadeOutAndDestroy(totalTime);
            _curCharacters.Remove(character);
        }
        else
        {
            Debug.LogWarning("Character to destroy not found: " + characterID);
        }
    }
    public static void AllCharacterFadeOutAndDestroy(float totalTime)
    {
        foreach (Character character in _curCharacters)
        {
            character.FadeOutAndDestroy(totalTime);
        }
        _curCharacters.Clear();
    }
    public static void AllCharacterFadeIn(float totalTime){

        foreach (Character character in _curCharacters)
        {
            character.FadeInCurrentEmotion(totalTime);
        }
    }
    public static void AllCharacterFadeOut(float totalTime){
        
        foreach (Character character in _curCharacters)
        {
            character.FadeOutCurrentEmotion(totalTime);
        }
    }

    public static void StartCharacterTalking(string characterID){
        Character character = GetInstancedCharacter(characterID);
        if(character == null){
            Debug.Log("해당 캐릭터가 없습니다");
            return;
        }
        character.StartTalking();
    }

    public static void StopCharacterTalking(string characterID){
        Character character = GetInstancedCharacter(characterID);
        if(character == null){
            Debug.Log("해당 캐릭터가 없습니다");
            return;
        }
        character.StopTalking();
    }

    public static void SetCharacterEmotion(string characterID, string emotionID, float totalTime){
        Character character = GetInstancedCharacter(characterID);
        if(character == null){
            Debug.Log("해당 캐릭터가 없습니다");
            return;
        }
        character.SetEmotion(emotionID, totalTime);
    }
}
