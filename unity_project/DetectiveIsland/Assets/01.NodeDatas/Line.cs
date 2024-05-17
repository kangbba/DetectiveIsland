
[System.Serializable]
public class Line
{
    private EChacterEmotion _emotionID;
    private string _sentence;

    public Line(EChacterEmotion emotionID, string sentence)
    {
        this._emotionID = emotionID;
        this._sentence = sentence;
    }

    public EChacterEmotion EmotionID { get => _emotionID;  }
    public string Sentence { get => _sentence; }
}