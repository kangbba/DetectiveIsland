using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ItemPanel : MonoBehaviour
{
    [SerializeField] protected ArokaAnimParent _arokaAnimParent;
    [SerializeField] protected ItemButton _itemBtnPrefab;
    [SerializeField] protected Transform _itemBtnsParent;
    [SerializeField] protected Image _background;
    [SerializeField] protected ItemContainer _itemContainer;
    protected List<ItemButton> _curItemBtns = new List<ItemButton>();
    protected ItemButton _selectedItemBtn;

    public ItemButton SelectedItemBtn { get => _selectedItemBtn; }

    protected virtual void Start()
    {
        ShowPanel(false, 0f);
    }

    public void Initialize(List<ItemData> itemDatas)
    {
        _selectedItemBtn = null;
        DestroyItemBtns();
        CreateItemButtons(itemDatas);
    }

    protected void CreateItemButtons(List<ItemData> itemDatas)
    {
        for (int i = 0; i < itemDatas.Count; i++)
        {
            ItemData itemData = itemDatas[i];
            ItemButton itemBtn = Instantiate(_itemBtnPrefab, _itemBtnsParent);
            itemBtn.Initialize(itemData, OnClickedItem, null, null);
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
        ShowPanel(true, 0f);
    }

    protected void ShowPanel(bool show, float totalTime)
    {
        _arokaAnimParent.SetOnAllChildren(show, totalTime);
    }

    protected void DestroyItemBtns()
    {
        foreach (var btn in _curItemBtns)
        {
            Destroy(btn.gameObject);
        }
        _curItemBtns.Clear();
    }

    protected virtual void OnClickedItem(string itemID)
    {
        if (_selectedItemBtn != null)
        {
            _selectedItemBtn.SetSelected(false);
        }
        _selectedItemBtn = _curItemBtns.FirstOrDefault(btn => btn.ItemData.ItemID == itemID);
        _itemContainer.Display(_selectedItemBtn.ItemData);
        _selectedItemBtn.SetSelected(true);
    }
}
