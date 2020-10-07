using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

// thanks brackeys for the audiomanager tutorial:
//https://www.youtube.com/watch?v=6OT43pvUyfY

public class AudioManager : SingletonBase<AudioManager>
{
    public Sound[] sounds;  //array of sound clips

    // Start is called before the first frame update
    public override void Awake()
    {
        base.Awake();
        
        // fill in the info from the specified audio clips
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    private void Start()
    {
        AudioListener.volume = 0.5f;
        /* added entanglement for the jam 
        OozeEscape.Instance.OnChangeState += (OozeEscape.GameState state) =>
        {
            if (state == OozeEscape.GameState.MainMenu)
            {
                if (!sounds[0].source.isPlaying)
                {
                    StopAll();
                    Play("menu");
                }
            }
            else
            {
                if (!sounds[1].source.isPlaying)
                {
                    StopAll();
                    Play("game");
                }
            }
        };
        */
    }

    // find a song by name and play it
    public void Play (string name)
    {
        // select a sound from the array by name (using a selector lambda)
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            // return error if song not found
            Debug.LogError("can't find song with name " + name);
            return;
        }
        // play song
        s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogError("can't find song with name " + name);
            return;
        }
        s.source.Stop();
    }

    public void StopAll()
    {
        foreach (Sound s in sounds)
        {
            s.source.Stop();
        }
    }


}
