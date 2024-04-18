


using TMPro;
using UnityEngine;
using UnityEngine.UI;

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