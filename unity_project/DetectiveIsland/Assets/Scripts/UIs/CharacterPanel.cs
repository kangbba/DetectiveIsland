using System.Collections.Generic;
using Aroka.Anim;
using UnityEngine;

public class CharacterPanel : MonoBehaviour
{
    [SerializeField] private Transform _characterParent;
    private List<Character> _curCharacters = new List<Character>();

    public void RepositionCharacter(string characterID, string positionID)
    {
        if (positionID == "Exit")
        {
            HideCharacter(characterID);
        }
        else
        {
            ShowCharacter(characterID, positionID); // EnterCharacter 호출
        }
    }

    private void ShowCharacter(string characterID, string positionID)
    {
        // 리스트에서 캐릭터를 찾음
        Character characterInstance = GetCharacter(characterID);

        if (characterInstance != null)
        {
            // 이미 해당 캐릭터가 활성화되어 있는 경우 위치만 변경
            Vector3 newPosition = CalculatePosition(positionID);
            characterInstance.transform.localPosition = newPosition;
        }
        else
        {
            // 캐릭터를 가져와야 함
            Character characterPrefab = CharacterService.GetCharacter(characterID);

            if (characterPrefab != null)
            {
                // 캐릭터를 인스턴스화하고 리스트에 추가
                Vector3 newPosition = CalculatePosition(positionID);
                characterInstance = Instantiate(characterPrefab, _characterParent);
                _curCharacters.Add(characterInstance);
                characterInstance.transform.localPosition = newPosition;
                characterInstance.Initialize();
                characterInstance.SetOn(true, 1f);
            }
            else
            {
                Debug.LogWarning("Character not found: " + characterID);
            }
        }
    }
    private void HideCharacter(string characterID)
    {
        // 리스트에서 캐릭터를 찾음
        Character characterInstance = GetCharacter(characterID);
        if (characterInstance != null)
        {
            // 캐릭터를 비활성화
            characterInstance.SetOn(false, 1f);

            // 리스트에서 캐릭터를 제거
            _curCharacters.Remove(characterInstance);

            // 어떤 처리를 추가하고 싶다면 여기에 추가
        }
        else
        {
            Debug.LogWarning("Character not found or already exited: " + characterID);
        }
    }


    public void ShowCharacters(float totalTime)
    {
        foreach (Character character in _curCharacters)
        {
            character.SetOn(true, totalTime);
        }
    }
    public void ClearCharacters(float totalTime)
    {
        foreach (Character character in _curCharacters)
        {
            character.SetOn(false, totalTime);
            Destroy(character.gameObject, totalTime);
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
                newPosition = new Vector3(-12f, 0f, 0f); // 예시 위치, 필요에 따라 수정
                break;
            case "Middle":
                newPosition = new Vector3(0f, 0f, 0f); // 예시 위치, 필요에 따라 수정
                break;
            case "Right":
                newPosition = new Vector3(12f, 0f, 0f); // 예시 위치, 필요에 따라 수정
                break;
            // 필요에 따라 다른 위치 추가
        }

        return newPosition;
    }

}
