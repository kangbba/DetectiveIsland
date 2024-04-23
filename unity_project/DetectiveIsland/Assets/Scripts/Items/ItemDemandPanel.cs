using System.Collections;
using Aroka.Anim;
using UnityEngine;
using UnityEngine.UI;

public enum DemandExitType{
    Correct,
    Discorrect,
    Cancelled,
}
public class ItemDemandPanel : ItemPanel
{
    [SerializeField] private Button _submitBtn;
    [SerializeField] private Button _exitBtn;
    private ItemButton _confirmedItemButton; // 사용자가 선택한 아이템 버튼
    private Coroutine _awaitItemBtnSelectedCoroutine; // 코루틴 참조를 저장하기 위한 변수

    private bool _isDemandCancel = false;

    protected override void Start()
    {
        base.Start();
        _submitBtn.onClick.AddListener(OnClickedSubmitBtn);
        _exitBtn.onClick.AddListener(OnClickedExitBtn);
    }

    public void OnClickedExitBtn(){

        base.OpenPanel(false, .3f);
        _isDemandCancel = true;
        
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
        _isDemandCancel = false;
        _confirmedItemButton = null; // 초기화

        while (_confirmedItemButton == null && !_isDemandCancel) // 버튼이 클릭될 때까지 기다림
        {
            yield return null; // 다음 프레임까지 대기
        }

        if (_confirmedItemButton != null)
        {
            yield return _confirmedItemButton.ItemData; // 선택된 아이템 데이터 반환
        }
        else
        {
            Debug.Log("_isDemandCancel로 Exit한 경우");
            yield return null; // 선택되지 않았을 경우 null 반환
        }
    }
    public override void OpenPanel(bool isOn, float totalTime){
        base.OpenPanel(isOn, totalTime);
    }

    private void Update(){
        if(_isOpen){
            _submitBtn.GetComponent<ArokaAnim>().SetAnim(_selectedItemBtn != null, .3f);
        }
    }
}
