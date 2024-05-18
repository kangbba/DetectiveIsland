using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ItemPanel : MonoBehaviour
{
    [SerializeField] protected ItemButton _itemBtnPrefab;
    [SerializeField] protected Transform _itemBtnsParent;
    [SerializeField] protected Image _background;
    [SerializeField] protected ItemContainer _itemContainer;
    protected List<ItemButton> _curItemBtns = new List<ItemButton>();
    protected ItemButton _selectedItemBtn;
    protected bool _isOpen = false;

    public ItemButton SelectedItemBtn { get => _selectedItemBtn; }

    protected virtual void Initialize(List<ItemData> itemDatas)
    {
        _selectedItemBtn = null;
        DestroyItemBtns();
        CreateItemButtons(itemDatas);
    }


    protected virtual void OnClickedItem(EItemID itemID)
    {
        SetSelected(itemID);
    }

    private void CreateItemButtons(List<ItemData> itemDatas)
    {
        for (int i = 0; i < itemDatas.Count; i++)
        {
            ItemData itemData = itemDatas[i];
            ItemButton itemBtn = Instantiate(_itemBtnPrefab, _itemBtnsParent);
            itemBtn.Initialize(itemData, OnClickedItem);
            _curItemBtns.Add(itemBtn);

            RectTransform btnRectTransform = itemBtn.GetComponent<RectTransform>();
            float spacing = 200;
            float xPosition = spacing * i;
            btnRectTransform.anchoredPosition = new Vector2(xPosition, btnRectTransform.anchoredPosition.y);
        }

        if (itemDatas.Count > 0)
        {
            _itemContainer.Display(itemDatas.Last());
        }
    }
    private void DestroyItemBtns()
    {
        foreach (var btn in _curItemBtns)
        {
            Destroy(btn.gameObject);
        }
        _curItemBtns.Clear();
    }

    private void SetSelected(EItemID itemID)
    {
        ItemButton foundItemBtn = _curItemBtns.FirstOrDefault(btn => btn.ItemData.ItemID == itemID);
        if (foundItemBtn == null)
        {
            Debug.LogError("해당 ID의 버튼 찾을 수 없습니다");
            return;
        }
        Debug.Log($"{itemID} Selected");
        if (_selectedItemBtn != null)
        {
            _selectedItemBtn.SetSelectedImg(false);
        }
        _selectedItemBtn = foundItemBtn;
        _itemContainer.Display(_selectedItemBtn.ItemData);
        _selectedItemBtn.SetSelectedImg(true);
    }
}
