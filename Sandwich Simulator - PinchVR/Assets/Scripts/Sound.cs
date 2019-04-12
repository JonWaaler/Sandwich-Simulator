using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{

    public string soundName = "";
    [Range(0, 1)]
    public float volume = .9f;
    public AudioClip clip;
    private AudioSource audioSource;

    public AudioSource GetAudioSource()
    {
        return audioSource;
    }

    public void SetAudioSource(AudioSource source)
    {
        audioSource = source;
    }
}
