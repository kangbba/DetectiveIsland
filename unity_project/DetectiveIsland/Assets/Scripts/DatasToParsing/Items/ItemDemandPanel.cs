using System.Collections;
using Aroka.Anim;
using Aroka.ArokaUtils;
using Cysharp.Threading.Tasks;
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
    }

    // 이 메서드는 ItemData를 반환하는 코루틴으로 변경되었습니다.
    public async UniTask<ItemData> AwaitItemDataSelectedTask()
    {
        _isDemandCancel = false;
        _confirmedItemButton = null; // 초기화

        while (_confirmedItemButton == null && !_isDemandCancel) // 버튼이 클릭될 때까지 기다림
        {
            await UniTask.Yield(); // 다음 프레임까지 대기
        }

        if (_confirmedItemButton != null)
        {
            return _confirmedItemButton.ItemData; // 선택된 아이템 데이터 반환
        }
        else
        {
            return null;
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
