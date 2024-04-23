using UnityEngine;

public static class EventTimeUIService
{
    private static EventTimeDisplayer _eventTimeDisplayer;

    // 스타트 시점에 EventTimeDisplayer 인스턴스를 가져오는 메소드
    public static void Load()
    {
        _eventTimeDisplayer = UIManager.Instance.EventTimeDisplayer;
    }

    public static void SetEventTime(EventTime eventTime)
    {
        _eventTimeDisplayer.SetEventTime(eventTime);
    }
}
