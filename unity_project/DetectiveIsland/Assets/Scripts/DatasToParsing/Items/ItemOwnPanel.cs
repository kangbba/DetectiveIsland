
using Aroka.Anim;
using UnityEngine;

public class ItemOwnPanel : ArokaAnimParent
{
    [SerializeField] private ItemContainer itemContainer; // 아이템 정보 컨테이너

    public void OpenPanel(ItemData itemData)
    {
        itemContainer.Display(itemData); // 아이템 정보 표시
        base.SetOnAllChildren(true, 0.1f); // 패널 비활성화
    }
    public void ClosePanel()
    {
        base.SetOnAllChildren(false, 0.1f); // 패널 비활성화
    }
}
