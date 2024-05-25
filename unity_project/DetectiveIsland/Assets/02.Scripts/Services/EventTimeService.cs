using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum TimeRelation
{
    Past,    // 과거
    Same,    // 동일
    Future   // 미래
}


public static class EventTimeService
{
    private static EventTime _curEventTime = null;

    public static EventTime CurEventTime { get => _curEventTime; }

    public static void Load()
    {
    }

    public static int GetYear(string date)
    {
        return ParseDatePart(date, 0);
    }

    public static int GetMonth(string date)
    {
        return ParseDatePart(date, 1);
    }

    public static int GetDay(string date)
    {
        return ParseDatePart(date, 2);
    }

    private static int ParseDatePart(string date, int index)
    {
        string[] dateParts = date.Split('-');
        if (dateParts.Length != 3)
        {
            Debug.LogError("Invalid date format provided.");
            return 0;
        }
        return int.Parse(dateParts[index]);
    }

    public static bool AreEqual(EventTime first, EventTime second)
    {
        return first.Date == second.Date && first.Hour == second.Hour && first.Minute == second.Minute;
    }

    public static TimeRelation CompareTime(EventTime first, EventTime second)
    {
        int yearFirst = GetYear(first.Date);
        int yearSecond = GetYear(second.Date);
        if (yearFirst != yearSecond)
            return yearFirst < yearSecond ? TimeRelation.Past : TimeRelation.Future;

        int monthFirst = GetMonth(first.Date);
        int monthSecond = GetMonth(second.Date);
        if (monthFirst != monthSecond)
            return monthFirst < monthSecond ? TimeRelation.Past : TimeRelation.Future;

        int dayFirst = GetDay(first.Date);
        int daySecond = GetDay(second.Date);
        if (dayFirst != daySecond)
            return dayFirst < daySecond ? TimeRelation.Past : TimeRelation.Future;

        int hourFirst = first.Hour;
        int hourSecond = second.Hour;
        if (hourFirst != hourSecond)
            return hourFirst < hourSecond ? TimeRelation.Past : TimeRelation.Future;

        int minuteFirst = first.Minute;
        int minuteSecond = second.Minute;
        if (minuteFirst != minuteSecond)
            return minuteFirst < minuteSecond ? TimeRelation.Past : TimeRelation.Future;

        return TimeRelation.Same;
    }



    public static void SetCurEventTime(EventTime eventTime)
    {
        Debug.Log($"새로 설정된 EventTime : {eventTime.Date} - {eventTime.Hour}시 {eventTime.Minute}분");
        _curEventTime = eventTime;
        UIManager.SetEventTime(eventTime);
    }

    public static List<EventPlan> EventTimeFilter(this List<EventPlan> eventPlans, EventTime inputTime, TimeRelation timeRelation)
    {
        return eventPlans.Where(plan => CompareTime(plan.EventTime, inputTime) == timeRelation && plan.EventTime.Date == inputTime.Date).ToList();
    }

    public static bool IsCurrentTimeEquals(EventTime eventTime){
        return CompareTime(CurEventTime, eventTime) == TimeRelation.Same;
    }
}