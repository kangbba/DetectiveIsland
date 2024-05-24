
[System.Serializable]
public class Line
{
    private EEmotionID _emotionID;
    private string _sentence;

    public Line(EEmotionID emotionID, string sentence)
    {
        this._emotionID = emotionID;
        this._sentence = sentence;
    }

    public EEmotionID EmotionID { get => _emotionID;  }
    public string Sentence { get => _sentence; }
}