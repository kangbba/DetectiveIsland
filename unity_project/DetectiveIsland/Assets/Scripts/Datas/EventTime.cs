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

    [SerializeField] private string _date;
    [SerializeField] private int  _hour;
    [SerializeField] private int _minute;

    // 생성자 정의
    public EventTime(string date, int hour, int minute)
    {
        _date = date;
        _hour = hour;
        _minute = minute;
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

    // 입력된 EventTime보다 나중인지를 확인하는 메서드
    public bool IsLaterThan(EventTime eventTime)
    {
        // 연도 비교
        if (this.Year > eventTime.Year)
            return true;
        // 월 비교
        if (this.Month > eventTime.Month)
            return true;
        // 일 비교
        if (this.Day > eventTime.Day)
            return true;
        // 시간 비교
        if (this._hour > eventTime.Hour)
            return true;
        // 분 비교
        if (this._hour == eventTime.Hour && this._minute > eventTime.Minute)
            return true;
        
        return false;
    }
}