﻿using UnityEngine;
using UnityEngine.Audio;
using System;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    //Singleton
    private static AudioManager instance = null;
    public static AudioManager Instance { get { return instance; } }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public bool playOnStart;

    //lists to hold the audio/music clips
    public List<Audio> audioClip;
    public List<Audio> musicClip;

    //reference to current music playing (used to stop it)
    Audio musicPlaying;
    //reference to musicPlaying's position in the musicClip array
    private int musicPlayingIndex;

    //bools to set if the musicList or current music should loop;
    public bool loopMusicList;
    public bool loopCurrentMusic;

    private void Start()
    {
        //assign each audio/music a source (not best practice but should be fine for this game)
        foreach (Audio a in audioClip)
        {
            a.Init(gameObject.AddComponent<AudioSource>());
        }
        foreach (Audio m in musicClip)
        {
            m.Init(gameObject.AddComponent<AudioSource>());
        }

        if (playOnStart)
        {
            ToggleMusic();
        }
    }

    //reference to the playMusic coroutine function (used to stop it)
    Coroutine playMusicCoroutine = null;

    //a coroutine function that plays from the musicClip array.
    public IEnumerator PlayMusic(int index = 0)
    {
        //if index not outofbounds it's safe to run
        if(!(index > musicClip.Count-1 || index < 0))
        {
            musicPlayingIndex = index;
            do
            {
                musicPlaying = musicClip[musicPlayingIndex];
                musicPlaying.Source.Play();
                //wait til the end of the clip
                yield return new WaitForSeconds(musicPlaying.clip.length);

                //if not looping current music, get next musicClip index
                if (!loopCurrentMusic)
                {
                    musicPlayingIndex = musicPlayingIndex == musicClip.Count - 1 ? 0 : musicPlayingIndex + 1;
                }
            }
            while (loopMusicList);
        }
    }

    //a function to stop the music playing, and the looping coroutine
    public void StopMusic()
    {
        musicPlaying.Source.Stop();
        StopCoroutine(playMusicCoroutine);
        playMusicCoroutine = null;
    }

    //a function to toggle the music on and off.
    public void ToggleMusic()
    {
        if (playMusicCoroutine != null)
        {
            Debug.Log("stop");
            StopMusic();
        }
        else
        {
            Debug.Log("start");
            playMusicCoroutine = StartCoroutine(PlayMusic(musicPlayingIndex));
        }
    }

    //a function to get an audio clip by it's name.
    private Audio AudioClipByName(string name)
    {
        Audio a = Array.Find(audioClip.ToArray(), sound => sound.name == name);
        if (a == null)
        {
            Debug.Log("Can't find sound with name: " + name);
        }
        return a;
    }

    //a function to get a music clip by it's name.
    private Audio MusicClipByName(string name)
    {
        Audio a = Array.Find(musicClip.ToArray(), sound => sound.name == name);
        if (a == null)
        {
            Debug.Log("Can't find music with name: " + name);
        }
        return a;
    }

    //function that starts playing a clip by name
    public void PlayAudioClip(string name)
    {
        Audio a = AudioClipByName(name);
        a.Source.Play();
    }

    //function that starts playing music by name
    public void PlayMusicClip(string name)
    {
        Audio m = MusicClipByName(name);
        musicPlayingIndex = musicClip.IndexOf(m);
        StopMusic();
        ToggleMusic();
    }

}
