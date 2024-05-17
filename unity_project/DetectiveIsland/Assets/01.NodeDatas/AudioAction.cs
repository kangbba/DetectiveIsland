using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EAudioFileID
{
    None,
    Yeah,
    Pow,
    Ah,
}

public class AudioAction : Element
{
    private readonly string _audioFileID;

    public AudioAction(string audioFileID)
    {
        _audioFileID = audioFileID;
    }

    public string AudioFileID { get => _audioFileID; }
}
