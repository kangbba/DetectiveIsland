using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System;
using Aroka.Anim;
using Aroka.ArokaUtils;
using System.Collections;
using System.Threading;

public class ItemPanel : ArokaAnim
{
    [SerializeField] private ItemButton _itemBtnPrefab; // 아이템 버튼 프리팹
    [SerializeField] private Transform _itemBtnsParent; // 버튼이 생성될 부모 컨테이너
    [SerializeField] private Image _background; // 패널의 배경 이미지
    [SerializeField] private ItemContainer _itemContainer; // 아이템 정보를 표시할 컨테이너
    [SerializeField] private Button _enterBtn;
    [SerializeField] private Button _exitBtn;
    [SerializeField] private Button _confirmBtn;
    private List<ItemButton> _curItemBtns = new List<ItemButton>();

    private ItemButton _cursoredItemBtn; // 현재 선택된 아이템 데이터
    private ItemButton _confirmedItemBtn; // 현재 선택확정 아이템 데이터

    public ItemData ConfirmedItemData { get => _confirmedItemBtn == null ? null : _confirmedItemBtn.ItemData; }

    private void Start(){
        _enterBtn.onClick.AddListener(OnClickedEnterBtn);
        _exitBtn.onClick.AddListener(OnClickedExitBtn);
        _confirmBtn.onClick.AddListener(OnClickedConfirmBtn);
    }
    // 아이템이 선택되었을 때 호출할 메서드
    public void Initialize(List<ItemData> itemDatas, bool exitable)
    {
        _itemBtnsParent.DestroyAllChildren(); // Ensure all children are destroyed properly
        CreateItemButtons(itemDatas);
        bool isItemExist = itemDatas != null && itemDatas.Count > 0;
        if(isItemExist){
            _itemContainer.Display(itemDatas.Last());
        }
        else{
            _itemContainer.Display(null);
        }
        _confirmBtn.gameObject.SetActive(false);
        _exitBtn.gameObject.SetActive(exitable);
    }

    public void SetOn(bool b, float totalTime){
        _enterBtn.gameObject.SetActive(!b);
        SetAnim(b, totalTime);
    }

    public IEnumerator AwaitItemBtnSelectedRoutine()
    {
        _confirmedItemBtn = null;
        while (_confirmedItemBtn == null)
        {
            yield return null;
        }
        yield return _confirmedItemBtn.ItemData;
    }
    public void CreateItemButtons(List<ItemData> itemDatas)
    {
        // 모든 자식 객체를 삭제
        for(int i = _curItemBtns.Count - 1 ; i>= 0 ; i--){
            Destroy(_curItemBtns[i].gameObject);
        }
        _curItemBtns.Clear();
        _cursoredItemBtn = null;
        _confirmedItemBtn = null;
        // 버튼 생성
        for (int i = 0; i < itemDatas.Count; i++)
        {
            ItemData itemData = itemDatas[i];
            ItemButton itemBtn = Instantiate(_itemBtnPrefab, _itemBtnsParent);
            itemBtn.Initialize(itemData, OnClickedItem, null , null); // 수정: 람다를 사용하지 않고 직접 메서드 참조
            _curItemBtns.Add(itemBtn);
            // 버튼의 위치 설정
            RectTransform btnRectTransform = itemBtn.GetComponent<RectTransform>();
            float spacing = 200; // 각 버튼 사이의 간격을 200으로 설정
            float xPosition = spacing * i; // i번째 버튼의 x 위치
            btnRectTransform.anchoredPosition = new Vector2(xPosition, btnRectTransform.anchoredPosition.y);
        }

    }
    private void OnClickedItem(string itemID){
        if(_cursoredItemBtn != null){
            _cursoredItemBtn.SetCursored(false);
        }
        _cursoredItemBtn = _curItemBtns.FirstOrDefault(btn => btn.ItemData.ItemID == itemID);
        _itemContainer.Display(_cursoredItemBtn.ItemData);
        _cursoredItemBtn.SetCursored(true);
        _confirmBtn.gameObject.SetActive(true);
    }
    private void OnClickedEnterBtn(){
        Initialize(ItemService.GetOwnItemDatas(), true);
        SetOn(true, .1f);
    }
    private void OnClickedExitBtn(){
        SetOn(false, .1f);
    }
    private void OnClickedConfirmBtn(){
        if(_cursoredItemBtn == null){
            Debug.LogError("먼저 cursor 하세요");
            return;
        }
        _confirmBtn.gameObject.SetActive(false);
        _confirmedItemBtn = _cursoredItemBtn;
    }
}
