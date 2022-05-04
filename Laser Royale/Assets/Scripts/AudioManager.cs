using UnityEngine;
using System.Collections.Generic;
using System;

public class AudioManager : MonoBehaviour
{
    public List<Sound> sounds;

    public static AudioManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("You are trying to create an AudioManager but one already exists.");

            Destroy(this);
            return;
        }

        DontDestroyOnLoad(this);

        foreach (Sound sound in sounds)
        {
            SoundConstructorHelper(sound);
        }


    }

    private void SoundConstructorHelper(Sound sound)
    {
        sound.source = gameObject.AddComponent<AudioSource>();
        sound.source.clip = sound.clip;

        sound.source.volume = sound.volume;
        sound.source.pitch = sound.pitch;

        sound.source.loop = sound.loop;

        sound.source.outputAudioMixerGroup = sound.output;
    }

    private void Start()
    {
        Play("BackgroundMusic");
    }

    //Play
    public void Play(string name)
    {
        Sound s = Array.Find(sounds.ToArray(), sound => sound.name == name);
        if (s != null)
        {
            s.source.Play();
        }
        else
        {
            Debug.LogWarning($"Sound {name} not found in sounds. Check that name is spelled correctly.");
        }
    }

}
