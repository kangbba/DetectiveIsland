using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TimeRelation
{
    Past,    // 과거
    Same,    // 동일
    Future   // 미래
}

[System.Serializable]
public class EventTime
{ 
    [SerializeField] private string _date;
    [SerializeField] private int  _hour;
    [SerializeField] private int _minute;

    // 연도, 월, 일 속성 추가
    public int Year => int.Parse(Date.Split('-')[0]);
    public int Month => int.Parse(Date.Split('-')[1]);
    public int Day => int.Parse(Date.Split('-')[2]);

    public string Date { get => _date; }
    public int Hour { get => _hour; }
    public int Minute { get => _minute; }

    // 생성자 정의
    public EventTime(string date, int hour, int minute)
    {
        _date = date;
        _hour = hour;
        _minute = minute;
    }

    public override string ToString()
    {
        return $"{Year}-{Month:00}-{Day:00} {Hour:00}:{Minute:00}";
    }
    // 내용 비교를 위한 Equals 메서드 재정의
    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        EventTime other = (EventTime)obj;
        return _date == other.Date && _hour == other.Hour && _minute == other.Minute;
    }
    public override int GetHashCode()
    {
        return _date.GetHashCode() ^ _hour.GetHashCode() ^ _minute.GetHashCode();
    }
// 시간의 전후 관계를 비교하여 enum 값으로 반환
    public TimeRelation CompareDate(int year, int month, int day)
    {
        // 연도 비교
        if (Year != year)
            return Year < year ? TimeRelation.Past : TimeRelation.Future;
        // 월 비교
        if (Month != month)
            return Month < month ? TimeRelation.Past : TimeRelation.Future;
        // 일 비교
        if (Day != day)
            return Day < day ? TimeRelation.Past : TimeRelation.Future;

        return TimeRelation.Same; // 모든 항목이 같을 경우
    }
    // 시간의 전후 관계를 비교하여 enum 값으로 반환
    public TimeRelation CompareTime(EventTime eventTime)
    {
        // 연도 비교
        if (Year != eventTime.Year)
            return Year < eventTime.Year ? TimeRelation.Past : TimeRelation.Future;
        // 월 비교
        if (Month != eventTime.Month)
            return Month < eventTime.Month ? TimeRelation.Past : TimeRelation.Future;
        // 일 비교
        if (Day != eventTime.Day)
            return Day < eventTime.Day ? TimeRelation.Past : TimeRelation.Future;
        // 시간 비교
        if (Hour != eventTime.Hour)
            return Hour < eventTime.Hour ? TimeRelation.Past : TimeRelation.Future;
        // 분 비교
        if (Minute != eventTime.Minute)
            return Minute < eventTime.Minute ? TimeRelation.Past : TimeRelation.Future;

        return TimeRelation.Same; // 모든 항목이 같을 경우
    }

}