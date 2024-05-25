using ArokaInspector.Attributes;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlaceSection : MonoBehaviour
{
    private bool _isAlreadyViewed;

    [SerializeField] private bool _useEnterEvent;
    [ShowIf("_useEnterEvent")]
    [SerializeField] private TextAsset _scenarioFile;

    public float SectionCenterX { get => transform.position.x; }
    public TextAsset ScenarioFile => _scenarioFile;
    public bool UseEnterEvent => _useEnterEvent;

    public async UniTask PlayEnterEvent()
    {
        if (_isAlreadyViewed)
        {
            Debug.Log("이미 열람한 이벤트");
            return;
        }
        if (!_useEnterEvent)
        {
            Debug.Log("이벤트를 사용하지 않는 section");
            return;
        }
        if (_scenarioFile == null)
        {
            Debug.Log("_useEnterEvent 임에도 _scenarioFile null");
            return;
        }
        Scenario scenario = EventService.LoadScenario(_scenarioFile);
        if (scenario == null)
        {
            Debug.Log("_scenarioFile 존재하지만 scenario 로드 실패");
            return;
        }
        _isAlreadyViewed = true;
        await EventProcessor.PlayEvent(scenario);
    }
}
