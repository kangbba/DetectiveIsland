using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class EventTime
{ 
    [SerializeField] private    string   _date;
    [SerializeField] private     int     _hour;
    [SerializeField] private     int     _minute;

    public int Year => int.Parse(Date.Split('-')[0]);
    public int Month => int.Parse(Date.Split('-')[1]);
    public int Day => int.Parse(Date.Split('-')[2]);

    public  string      Date        { get => _date; }
    public  int         Hour        { get => _hour; }
    public  int         Minute      { get => _minute; }

    // 생성자 정의
    public EventTime(string date, int hour, int minute)
    {
        _date = date;
        _hour = hour;
        _minute = minute;
    }
    public override int GetHashCode()
    {
        return _date.GetHashCode() ^ _hour.GetHashCode() ^ _minute.GetHashCode();
    }

    public override string ToString()
    {
        return $"{Date} {Hour} {Minute}";
    }
    // 시간의 전후 관계를 비교하여 enum 값으로 반환
}