using System.Collections;
using System.Collections.Generic;
using Aroka.Anim;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
}


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

    public void SetOn(bool b, float totalTime){
        if(b){
            base.FadeInFromStart(totalTime);
        }
        else{
            base.FadeOut(totalTime);
        }
    }
}


[System.Serializable] // Unity 인스펙터에 표시될 수 있도록 Serializable 특성 추가
public class ItemContainer
{
    [SerializeField] private TextMeshProUGUI itemNameText; // 아이템 이름을 표시할 텍스트 컴포넌트
    [SerializeField] private TextMeshProUGUI itemDescriptionText; // 아이템 설명을 표시할 텍스트 컴포넌트
    [SerializeField] private Image itemImage; // 아이템 이미지를 표시할 이미지 컴포넌트

    // 아이템 데이터를 기반으로 UI 컴포넌트를 업데이트하는 메서드
    public void Display(ItemData itemData)
    {
        if (itemNameText != null) 
            itemNameText.text = itemData.ItemNameForUser;

        if (itemDescriptionText != null) 
            itemDescriptionText.text = itemData.ItemDescription;

        if (itemImage != null) 
            itemImage.sprite = itemData.ItemSprite;
    }
    
}

public class ItemOwnPanel : ArokaAnim
{
    [SerializeField] private ItemContainer itemContainer; // 아이템 정보 컨테이너

    public void ShowItem(ItemData itemData)
    {
        itemContainer.Display(itemData); // 아이템 정보 표시
        SetOn(true, 0.1f); // 패널 활성화
    }

    public void ClosePanel()
    {
        SetOn(false, 0.1f); // 패널 비활성화
    }

    // SetOn 함수 정의: 패널의 활성화 및 비활성화를 처리
    public void SetOn(bool isOn, float duration)
    {
        base.SetAnim(isOn, duration);
    }
}
