using System.Collections.Generic;
using Aroka.Anim;
using UnityEngine;

public class CharacterPanel : MonoBehaviour
{
    [SerializeField] private Transform _characterParent;
    private List<Character> _curCharacters = new List<Character>();

    public void MakeCharacter(string characterID, string positionID, float totalTime){

        // 캐릭터를 가져와야 함
        Character characterPrefab = CharacterService.GetCharacter(characterID);

        if (characterPrefab== null)
        {
            Debug.LogWarning("Character not found: " + characterID);
            return;
        }
        // 캐릭터를 인스턴스화하고 리스트에 추가
        Vector3 newPosition = CalculatePosition(positionID);
        Character characterInstance = Instantiate(characterPrefab, _characterParent);
        _curCharacters.Add(characterInstance);
        characterInstance.transform.localPosition = newPosition;
        characterInstance.Initialize();
        characterInstance.SetOn(false, 0f);
        characterInstance.SetOn(true, totalTime);
    }

    public void DestroyCharacter(string characterID, float totalTime)
    {
        // 리스트에서 캐릭터를 찾음
        Character characterInstance = GetCharacter(characterID);
        if (characterInstance != null)
        {
            // 캐릭터를 비활성화
            characterInstance.SetOn(false, totalTime);

            // 리스트에서 캐릭터를 제거
            _curCharacters.Remove(characterInstance);

            // 어떤 처리를 추가하고 싶다면 여기에 추가
        }
        else
        {
            Debug.LogWarning("Character not found or already exited: " + characterID);
        }
    }
    
    public void DestroyAllCharacters(float totalTime)
    {
        foreach (Character character in _curCharacters)
        {
            DestroyCharacter(character.CharacterData.CharacterID, totalTime);
        }
    }
    private Character GetCharacter(string characterID)
    {
        return _curCharacters.Find(character => character.CharacterData.CharacterID == characterID);
    }

    private Vector3 CalculatePosition(string positionID)
    {
        Vector3 newPosition = Vector3.zero;

        // positionID에 따라 위치 설정
        switch (positionID)
        {
            case "Left":
                newPosition = new Vector3(-8f, 0f, 0f); // 예시 위치, 필요에 따라 수정
                break;
            case "Middle":
                newPosition = new Vector3(0f, 0f, 0f); // 예시 위치, 필요에 따라 수정
                break;
            case "Right":
                newPosition = new Vector3(8f, 0f, 0f); // 예시 위치, 필요에 따라 수정
                break;
            // 필요에 따라 다른 위치 추가
        }

        return newPosition;
    }

}
