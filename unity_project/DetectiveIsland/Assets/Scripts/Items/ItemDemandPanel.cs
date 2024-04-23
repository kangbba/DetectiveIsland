using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ItemDemandPanel : ItemPanel
{
    [SerializeField] private Button _submitBtn;
    private ItemButton _confirmedItemButton; // 사용자가 선택한 아이템 버튼
    private Coroutine _awaitItemBtnSelectedCoroutine; // 코루틴 참조를 저장하기 위한 변수

    protected override void Start()
    {
        base.Start();
        _submitBtn.onClick.AddListener(OnClickedSubmitBtn);
    }

    private void OnClickedSubmitBtn()
    {
        _confirmedItemButton = _selectedItemBtn; // 사용자가 선택한 아이템을 확정
        if (_awaitItemBtnSelectedCoroutine != null)
        {
            StopCoroutine(_awaitItemBtnSelectedCoroutine); // 안전하게 코루틴 중단
            _awaitItemBtnSelectedCoroutine = null; // 참조 초기화
        }
    }

    // 이 메서드는 ItemData를 반환하는 코루틴으로 변경되었습니다.
    public IEnumerator AwaitItemDataSelectedRoutine()
    {
        _confirmedItemButton = null; // 초기화
        yield return StartCoroutine(WaitForConfirmation()); // 코루틴 시작하고 완료 대기

        if (_confirmedItemButton != null)
        {
            yield return _confirmedItemButton.ItemData; // 선택된 아이템 데이터 반환
        }
        else
        {
            yield return null; // 선택되지 않았을 경우 null 반환
        }
    }

    private IEnumerator WaitForConfirmation()
    {
        while (_confirmedItemButton == null) // 버튼이 클릭될 때까지 기다림
        {
            yield return null; // 다음 프레임까지 대기
        }
    }

    public void ShowPanelOn(bool isOn, float totalTime)
    {
        base.ShowPanel(isOn, totalTime);
    }
}
