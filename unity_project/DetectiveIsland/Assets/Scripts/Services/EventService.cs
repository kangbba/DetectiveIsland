using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class EventPlan
{
    [SerializeField] private EventTime _eventTime;
    [SerializeField] private string _placeID;
    [SerializeField] private TextAsset _scenarioFile; // JSON 파일 경로 또는 이름

    public EventPlan(EventTime eventTime, string placeID, TextAsset scenarioFile)
    {
        _eventTime = eventTime;
        _placeID = placeID;
        _scenarioFile = scenarioFile;
    }

    public string PlaceID { get => _placeID; }
    public TextAsset ScenarioFile { get => _scenarioFile; }
    public EventTime EventTime { get => _eventTime;  }
}

public static class EventService
{
    private static EventRoadMap _eventRoadmap;

    public static List<EventPlan> EventPlans => _eventRoadmap.EventPlans;
    private static EventTime _curEventTime = null;
    public static EventTime CurEventTime { get => _curEventTime; }

    public static void Initialize()
    {       
         _eventRoadmap = Resources.Load<EventRoadMap>("EventRoadMap/MainEventRoadMap");
        if (_eventRoadmap == null)
        {
            Debug.LogError($"Failed to load EventRoadMap from Resources folder with filename: {"EventRoadMap"}");
        }
    }
    public static void SetCurEventTime(string date, int hour, int minute)
    {
        _curEventTime = new EventTime(date, hour, minute);
    }

    public static void SetCurEventTime(EventTime eventTime)
    {
        Debug.Log($"새로 설정된 EventTime : {eventTime.Date} - {eventTime.Hour}시 {eventTime.Minute}분");
        _curEventTime = new EventTime(eventTime.Date, eventTime.Hour, eventTime.Minute);
    }
    public static EventPlan GetNextEventPlan(EventPlan currentEventPlan)
    {
        var eventPlans = _eventRoadmap.EventPlans; // 전체 이벤트 플랜 목록을 가져옵니다.
        int currentIndex = eventPlans.IndexOf(currentEventPlan); // 현재 이벤트 플랜의 인덱스를 찾습니다.

        if (currentIndex == -1)
        {
            Debug.LogError("The current event plan is not found in the list.");
            return null; // 현재 이벤트 플랜이 목록에 없는 경우
        }

        int nextIndex = currentIndex + 1; // 다음 이벤트 플랜의 인덱스
        if (nextIndex < eventPlans.Count) // 다음 인덱스가 리스트 범위 내인지 확인
        {
            return eventPlans[nextIndex]; // 다음 이벤트 플랜 반환
        }

        Debug.Log("There is no next event plan. Reached the end of the list.");
        return null; // 리스트의 끝이면 null 반환
    }


    // 입력된 EventTime 기준으로 이미 완료된 데일리 이벤트 플랜을 필터링하는 확장 메서드
    public static List<EventPlan> EventTimeFilter(this List<EventPlan> eventPlans, EventTime inputTime, TimeRelation timeRelation)
    {
        return eventPlans.Where(plan => plan.EventTime.CompareTime(inputTime) == timeRelation && plan.EventTime.Date == inputTime.Date).ToList();
    }

    public static EventPlan GetFirstEventPlan(){
        return _eventRoadmap.EventPlans[0];
    }

    public static List<EventPlan> GetEventPlansByDate(string date)
    {
        int year = int.Parse(date.Split('-')[0]);
        int month = int.Parse(date.Split('-')[1]);
        int day = int.Parse(date.Split('-')[2]);
        
        List<EventPlan> eventPlans = new List<EventPlan>();
        foreach (EventPlan plan in _eventRoadmap.EventPlans)
        {
            if (plan.EventTime.CompareDate(year, month, day) == TimeRelation.Same)
            {
                eventPlans.Add(plan);
            }
        }
        return eventPlans;
    }
    public static List<EventPlan> GetEventPlans(EventTime eventTime)
    {
        List<EventPlan> eventPlans = new List<EventPlan>();
        foreach (EventPlan plan in _eventRoadmap.EventPlans)
        {
            if (plan.EventTime.Equals(eventTime))
            {
                eventPlans.Add(plan);
            }
        }
        return eventPlans;
    }

    public static EventPlan GetEventPlan(EventTime eventTime, string placeID)
    {
        foreach (EventPlan plan in _eventRoadmap.EventPlans)
        {
            if (plan.EventTime.Equals(eventTime) && plan.PlaceID == placeID)
            {
                return plan;
            }
        }
        return null;
    }
}