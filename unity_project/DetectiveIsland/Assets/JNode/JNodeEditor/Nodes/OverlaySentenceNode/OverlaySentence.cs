using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OverlaySentence : Element
{
    private string _sentence;
    private float _sentenceTime;
    private float _afterDelayTime;

    public OverlaySentence(string sentence, float sentenceTime, float afterDelayTime)
    {
        _sentence = sentence;
        _sentenceTime = sentenceTime;
        _afterDelayTime = afterDelayTime;
    }

    public string Sentence { get => _sentence; }
    public float SentenceTime { get => _sentenceTime;}
    public float AfterDelayTime { get => _afterDelayTime; }
}
