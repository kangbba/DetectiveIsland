

using Aroka.Anim;
using UnityEngine;

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
