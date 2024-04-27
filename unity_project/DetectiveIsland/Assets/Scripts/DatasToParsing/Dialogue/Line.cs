
[System.Serializable]
public class Line
{
    private string _emotionID;
    private string _sentence;


    public Line(string emotionID, string sentence)
    {
        this._emotionID = emotionID;
        this._sentence = sentence;
    }

    public string EmotionID { get => _emotionID; set => _emotionID = value; }
    public string Sentence { get => _sentence; set => _sentence = value; }
}