using UnityEngine;

public static class AudioController
{
    private static GameObject _audioGameObject;
    private static AudioSource _audioSource;

    public static void Load(){
        _audioGameObject = new GameObject("AudioController");
        _audioSource = _audioGameObject.AddComponent<AudioSource>();
    }

    public static void PlaySound(string clipName, float volume)
    {
        AudioClip clip = Resources.Load<AudioClip>(clipName);
        if (clip != null)
        {
            _audioSource.clip = clip;
            _audioSource.volume = volume;
            _audioSource.Play();
        }
        else
        {
            Debug.LogWarning($"Audio clip not found: {clipName}");
        }
    }

    public static void StopSound()
    {
        _audioSource.Stop();
    }

    public static void PauseSound()
    {
        _audioSource.Pause();
    }
}
