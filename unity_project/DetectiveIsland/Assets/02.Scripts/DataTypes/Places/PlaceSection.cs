
using ArokaInspector.Attributes;
using Cysharp.Threading.Tasks;
using UnityEngine;

[System.Serializable]
public class PlaceSection
{
    [SerializeField] private float _sectionCenterX;

    [SerializeField] private bool _useEnterEvent;
    [ShowIf("_useEnterEvent")]
    [SerializeField] private EventPlan _eventPlan;
    public float SectionCenterX { get => _sectionCenterX; }
    public EventPlan EventPlan { get => _eventPlan;  }
    public bool UseEnterEvent { get => _useEnterEvent; }

    public async UniTask PlayEnterEvent(){
        if(!_useEnterEvent){
            Debug.Log("이벤트를 사용하지않는 section");
            return;
        }
        EventPlan eventPlanToPlay = _eventPlan;
        if (eventPlanToPlay == null)
        {
            Debug.Log("eventPlanToPlay null or time does not match");
            return;
        }
        Scenario scenario = EventService.LoadScenario(eventPlanToPlay.ScenarioFile);
        if(scenario == null){
            Debug.Log("해당 eventplan엔 시나리오가 없으므로 생략");
            return;
        }
        await EventProcessor.PlayEvent(scenario);
    }
}