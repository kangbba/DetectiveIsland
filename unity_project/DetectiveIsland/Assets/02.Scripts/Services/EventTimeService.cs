using System;
using System.Collections.Generic;
using System.Linq;
using Aroka.ArokaUtils;
using UnityEngine;

public enum TimeRelation
{
    Past,    // 과거
    Same,    // 동일
    Future   // 미래
}
public static class EventPlanManager
{
    private static EventTime _curEventTime = null;
    private static EventRoadmap _eventRoadmap;

    public static EventTime CurEventTime => _curEventTime;

    public static void Load()
    {
        _eventRoadmap = ArokaUtils.LoadScriptableDatasFromFolder<EventRoadmap>("EventRoadmap").FirstOrDefault();
        if (_eventRoadmap == null)
        {
            Debug.LogError("EventRoadmap을 찾을 수 없습니다.");
            return;
        }

        var allEventTimes = _eventRoadmap.AllEventPlans.Select(plan => plan.EventTime).Distinct().OrderBy(eventTime => eventTime.Date).ThenBy(eventTime => eventTime.Hour).ThenBy(eventTime => eventTime.Minute).ToList();

        foreach (var eventTime in allEventTimes)
        {
            Debug.Log($"EventTime: {eventTime.Date} - {eventTime.Hour}:{eventTime.Minute}");
        }
    }

    public static EventPlan GetCurEventPlanOfPlace(EPlaceID placeID, int sectionIndex)
    {
      //  Debug.Log($"Checking event for PlaceID: {placeID}, SectionIndex: {sectionIndex}, EventTime: {eventTime}");

        foreach (var plan in _eventRoadmap.AllEventPlans)
        {
         //   Debug.Log($"Checking Plan - EventTime: {plan.EventTime}, PlaceID: {plan.PlaceID}, SectionIndex: {plan.PlaceSectionIndex}");
            if (plan.EventTime.Equals(CurEventTime) && plan.PlaceID == placeID && plan.PlaceSectionIndex == sectionIndex)
            {
                return plan;
            }
        }
        return null;
    }


    public static int GetYear(string date) => ParseDatePart(date, 0);

    public static int GetMonth(string date) => ParseDatePart(date, 1);

    public static int GetDay(string date) => ParseDatePart(date, 2);

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
    public static bool IsCurrentTimeEquals(EventTime eventTime)
    {
        return CompareTime(CurEventTime, eventTime) == TimeRelation.Same;
    }
    public static void TimePassesToNext(){
        SetCurEventTime(GetNextEventTime());
    }
    public static void SetCurEventTime(EventTime eventTime)
    {
        Debug.Log($"새로 설정된 EventTime : {eventTime.Date} - {eventTime.Hour}시 {eventTime.Minute}분");
        _curEventTime = eventTime;
        UIManager.SetEventTime(eventTime);
    }
    private static EventTime GetNextEventTime()
    {
        var futureEvents = _eventRoadmap.AllEventPlans.Select(plan => plan.EventTime).Where(eventTime => CompareTime(eventTime, _curEventTime) == TimeRelation.Future).Distinct().OrderBy(eventTime => eventTime.Date).ThenBy(eventTime => eventTime.Hour).ThenBy(eventTime => eventTime.Minute).ToList();
        return futureEvents.FirstOrDefault();
    }

    public static bool IsAllSolvedInEventTime(this EventTime eventTime){
        return GetEventPlans(eventTime).IsAllSolved();
    }
    private static List<EventPlan> GetEventPlans(EventTime eventTime){
        var properEvents = new List<EventPlan>();
        foreach(EventPlan eventPlan in _eventRoadmap.AllEventPlans){
            bool isIdentical = eventPlan.EventTime.IsEqual(eventTime);
            if(isIdentical){
                properEvents.Add(eventPlan);
            }
        }
        return properEvents;
    }

    private static bool IsAllSolved(this List<EventPlan> eventPlans){
        foreach(EventPlan eventPlan in eventPlans){
            if(!eventPlan.IsSolved){
                return false;
            }
        }
        return true;
    }

}
