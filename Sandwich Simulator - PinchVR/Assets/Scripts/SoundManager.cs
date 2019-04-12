using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour {

    public Sound[] sounds;

	// Use this for initialization
	void Start ()
    {
        foreach (var sound in sounds)
        {
            sound.SetAudioSource(gameObject.AddComponent<AudioSource>());
            sound.GetAudioSource().clip = sound.clip;
            
        }
	}

    public void PlaySound(string name)
    {
        foreach (var sound in sounds)
        {
            if(sound.soundName == name)
            {
                sound.GetAudioSource().volume = sound.volume;
                sound.GetAudioSource().Play();
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
