using UnityEngine;
using TMPro;

public enum DisplayFormat
{
    Standard,  // 기본 형식: "2024.05.01"
    English,   // 영어권 형식: "May 1, 2024"
    Debug      // 디버그 모드
}

public class EventTimeDisplayer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timeText;  // 텍스트 컴포넌트
    [SerializeField] private TextMeshProUGUI _debugText; // 디버그 텍스트 컴포넌트
    [SerializeField] private DisplayFormat _displayFormat = DisplayFormat.Standard;  // 기본 포맷 설정

    public void SetEventTime(EventTime eventTime)
    {
        if (_timeText == null)
        {
            Debug.LogError("Time TextMeshProUGUI component is not assigned.");
            return;
        }

        string formattedDate = FormatDate(eventTime);
        if (_displayFormat == DisplayFormat.Debug)
        {
            _timeText.text = formattedDate;  // 시간과 분 포함
            _debugText.gameObject.SetActive(true);  // 디버그 텍스트 활성화
        }
        else
        {
            _timeText.text = formattedDate;
            _debugText.gameObject.SetActive(false);  // 디버그 텍스트 비활성화
        }
    }

    private string FormatDate(EventTime eventTime)
    {
        string formattedDate;
        switch (_displayFormat)
        {
            case DisplayFormat.Standard:
            case DisplayFormat.English:
                formattedDate = FormatStandardOrEnglish(eventTime);
                break;
            case DisplayFormat.Debug:
                formattedDate = $"{EventTimeService.GetYear(eventTime.Date):0000}-{EventTimeService.GetMonth(eventTime.Date):00}-{EventTimeService.GetDay(eventTime.Date):00} {eventTime.Hour:00}:{eventTime.Minute:00}";
                break;
            default:
                formattedDate = eventTime.Date;  // Fallback if format is unrecognized
                break;
        }

        return formattedDate;
    }

    private string FormatStandardOrEnglish(EventTime eventTime)
    {
        if (_displayFormat == DisplayFormat.Standard)
        {
            return $"{EventTimeService.GetYear(eventTime.Date):0000}.{EventTimeService.GetMonth(eventTime.Date):00}.{EventTimeService.GetDay(eventTime.Date):00}";
        }
        else  // English format
        {
            int month = EventTimeService.GetMonth(eventTime.Date);
            int day = EventTimeService.GetDay(eventTime.Date);
            int year = EventTimeService.GetYear(eventTime.Date);
            string monthName = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);
            return $"{monthName} {day}, {year}";
        }
    }

    public void ChangeDisplayFormat(DisplayFormat newFormat)
    {
        _displayFormat = newFormat;
    }
}
