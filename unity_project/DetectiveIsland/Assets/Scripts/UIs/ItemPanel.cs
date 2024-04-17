using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System;
using Aroka.Anim;
using Aroka.ArokaUtils;
using System.Collections;

public class ItemPanel : ArokaAnim
{
    [SerializeField] private ItemButton _itemBtnPrefab; // 아이템 버튼 프리팹
    [SerializeField] private Transform _itemBtnsParent; // 버튼이 생성될 부모 컨테이너
    [SerializeField] private Image _background; // 패널의 배경 이미지
    [SerializeField] private Image _selectedItemImg; // 선택된 아이템의 이미지를 보여줄 이미지 컴포넌트
    [SerializeField] private TextMeshProUGUI _selectedItemNameText; // 아이템 제목 텍스트
    [SerializeField] private TextMeshProUGUI _selectedItemDescText; // 아이템 설명 텍스트
    [SerializeField] private GameObject _exitBtn; // 아이템 설명 텍스트

    private ItemData _selectedItemData; // 현재 선택된 아이템 데이터

    public ItemData SelectedItemData { get => _selectedItemData; }

    // 이벤트를 사용하여 아이템 선택을 외부에 알림
    public static event Action<string> onItemDemandSelected;

    // 아이템이 선택되었을 때 호출할 메서드
    public void SelectItemDemand(string itemID)
    {
        onItemDemandSelected?.Invoke(itemID);
    }
    
    public void Initialize(List<ItemData> itemDatas, bool exitable)
    {
        _itemBtnsParent.DestroyAllChildren(); // Ensure all children are destroyed properly
        OnItemButtonClicked(itemDatas.Last().ItemID);
        CreateItemButtons(itemDatas);
        _exitBtn.SetActive(exitable);
    }

    public void SetOn(bool b, float totalTime){
        SetAnim(b, totalTime);
    }

    public IEnumerator AwaitItemBtnSelectedRoutine()
    {
        _selectedItemData = null;
        while (_selectedItemData == null)
        {
            yield return null;
        }
        Debug.Log($"Selected choice: {_selectedItemData.ItemNameForUser}");
    }
    

    public void OnClickedExitBtn(){
        SetOn(false, .1f);
    }
    public void CreateItemButtons(List<ItemData> itemDatas)
    {
        // 모든 자식 객체를 삭제
        _itemBtnsParent.DestroyAllChildren();

        // 버튼 생성
        for (int i = 0; i < itemDatas.Count; i++)
        {
            ItemData itemData = itemDatas[i];
            ItemButton itemBtn = Instantiate(_itemBtnPrefab, _itemBtnsParent);
            itemBtn.Initialize(itemData, OnItemButtonClicked); // 수정: 람다를 사용하지 않고 직접 메서드 참조

            // 버튼의 위치 설정
            RectTransform btnRectTransform = itemBtn.GetComponent<RectTransform>();
            float spacing = 200; // 각 버튼 사이의 간격을 200으로 설정
            float xPosition = spacing * i; // i번째 버튼의 x 위치
            btnRectTransform.anchoredPosition = new Vector2(xPosition, btnRectTransform.anchoredPosition.y);
        }
    }

    private void OnItemButtonClicked(string itemID)
    {
        ItemData itemData = ItemService.GetItemData(itemID);
        _selectedItemData = itemData;
        _selectedItemImg.sprite = itemData.ItemSprite; // Update the item description
        _selectedItemNameText.text = itemData.ItemNameForUser;
        _selectedItemDescText.text = itemData.ItemDescription; // Update the item description
    }
}
