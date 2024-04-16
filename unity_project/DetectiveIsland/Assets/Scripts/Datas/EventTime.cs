using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class EventTime
{ 
    // 연도, 월, 일 속성 추가
    public int Year => int.Parse(Date.Split('-')[0]);
    public int Month => int.Parse(Date.Split('-')[1]);
    public int Day => int.Parse(Date.Split('-')[2]);

    public string Date { get => _date; }
    public int Hour { get => _hour; }
    public int Minute { get => _minute; }
    public int Sec { get => _sec; }

    [SerializeField] private string _date;
    [SerializeField] private int  _hour;
    [SerializeField] private int _minute;
    private int _sec;

    // 생성자 정의
    public EventTime(string date, int hour, int minute)
    {
        _date = date;
        _hour = hour;
        _minute = minute;
    }

    public override string ToString()
    {
        return $"{Year}-{Month:00}-{Day:00} {Hour:00}:{Minute:00}:{Sec:00}";
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
    // 날짜가 동일한지 비교하는 메서드
    public bool IsSameDate(EventTime other)
    {
        return IsSameDate(other.Date) && _hour == other.Hour && _minute == other.Minute;
    }
    public bool IsSameDate(string otherDate)
    {
        // 두 날짜를 '-'로 분리하여 연, 월, 일을 비교
        string[] thisDateParts = _date.Split('-');
        string[] otherDateParts = otherDate.Split('-');

        if (thisDateParts.Length == 3 && otherDateParts.Length == 3)
        {
            return thisDateParts[0] == otherDateParts[0] && // 연도 비교
                   thisDateParts[1] == otherDateParts[1] && // 월 비교
                   thisDateParts[2] == otherDateParts[2];   // 일 비교
        }
        Debug.LogError("IsSameDay Error");
        return default;
    }

  
    public bool IsPastThan(EventTime eventTime)
    {
        if (Year < eventTime.Year) return true;
        if (Year > eventTime.Year) return false;
        if (Month < eventTime.Month) return true;
        if (Month > eventTime.Month) return false;
        if (Day < eventTime.Day) return true;
        if (Day > eventTime.Day) return false;
        if (Hour < eventTime.Hour) return true;
        if (Hour > eventTime.Hour) return false;
        if (Minute < eventTime.Minute) return true;
        if (Minute > eventTime.Minute) return false;
        if (Sec < eventTime.Sec) return true;
        if (Sec > eventTime.Sec) return false;
        return false; // 모든 항목이 동일한 경우
    }


    public void AddSeconds(){
        _sec = 30;
    }

}