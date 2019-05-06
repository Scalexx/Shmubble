using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public static AudioManager instance;

    public Sound[] player;

    public Sound[] boss;

    public Sound[] environment;

    public Sound[] ambience;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in player)
        {
            s.source = gameObject.transform.GetChild(0).gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.outputAudioMixerGroup = s.mixer;

            s.source.priority = s.priority;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }

        foreach (Sound s in boss)
        {
            s.source = gameObject.transform.GetChild(1).gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.outputAudioMixerGroup = s.mixer;

            s.source.priority = s.priority;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }

        foreach (Sound s in environment)
        {
            s.source = gameObject.transform.GetChild(2).gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.outputAudioMixerGroup = s.mixer;

            s.source.priority = s.priority;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }

        foreach (Sound s in ambience)
        {
            s.source = gameObject.transform.GetChild(3).gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.outputAudioMixerGroup = s.mixer;

            s.source.priority = s.priority;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    public void PlayPlayerSound(string name)
    {
        Sound s = Array.Find(player, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + "not found in Player list!");
            return;
        }

        s.source.PlayOneShot(s.source.clip);
    }

    public void PlayBossSound(string name)
    {
        Sound s = Array.Find(boss, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + "not found in Boss list!");
            PlayEnvironmentSound(name);
            return;
        }

        s.source.PlayOneShot(s.source.clip);
    }

    public void PlayEnvironmentSound(string name)
    {
        Sound s = Array.Find(environment, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + "not found in Environment list!");
            return;
        }

        s.source.PlayOneShot(s.source.clip);
    }

    public void PlayAmbientSound(string name)
    {
        Sound s = Array.Find(ambience, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + "not found in Ambience list!");
            return;
        }

        s.source.Play();
    }

    public void StopPlaying (string name)
    {
        Sound s = Array.Find(player, sound => sound.name == name);
        Sound o = Array.Find(boss, sound => sound.name == name);
        Sound u = Array.Find(environment, sound => sound.name == name);
        Sound n = Array.Find(ambience, sound => sound.name == name);

        if (s != null || o != null || u != null || n != null)
        {
            s.source.Stop();
        }
        else
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
    }

    public void PauseSounds ()
    {
        foreach (Sound s in player)
        {
            if (s.source.isPlaying)
            {
                s.paused = true;
                s.source.Pause();
            }
        }

        foreach (Sound s in boss)
        {
            if (s.source.isPlaying)
            {
                s.paused = true;
                s.source.Pause();
            }
        }

        foreach (Sound s in environment)
        {
            if (s.source.isPlaying)
            {
                s.paused = true;
                s.source.Pause();
            }
        }

        foreach (Sound s in ambience)
        {
            if (s.source.isPlaying)
            {
                s.paused = true;
                s.source.Pause();
            }
        }
    }

    public void UnpauseSounds ()
    {
        foreach (Sound s in player)
        {
            if (s.paused)
            {
                s.paused = false;
                s.source.UnPause();
            }
        }

        foreach (Sound s in boss)
        {
            if (s.paused)
            {
                s.paused = false;
                s.source.UnPause();
            }
        }

        foreach (Sound s in environment)
        {
            if (s.paused)
            {
                s.paused = false;
                s.source.UnPause();
            }
        }

        foreach (Sound s in ambience)
        {
            if (s.paused)
            {
                s.paused = false;
                s.source.UnPause();
            }
        }
    }

    public void StopAllSounds()
    {
        foreach (Sound s in player)
        {
            s.source.Stop();
        }

        foreach (Sound s in boss)
        {
            s.source.Stop();
        }

        foreach (Sound s in environment)
        {
            s.source.Stop();
        }

        foreach (Sound s in ambience)
        {
            s.source.Stop();
        }
    }
}

// no bugs plz
