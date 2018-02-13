using UnityEngine;
using UnityEngine.Audio;
using System;
using System.Collections;

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
    public Audio[] audioClip;
    public Audio[] musicClip;
    Audio musicPlaying;
    private int musicPlayingIndex;
    public bool loopMusicList;
    public bool loopCurrentMusic;

    private void Start()
    {
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

    Coroutine music = null;
    public IEnumerator PlayMusic(int index = 0)
    {
        if(!(index > musicClip.Length-1))
        {
            musicPlayingIndex = index;
            do
            {
                musicPlaying = musicClip[musicPlayingIndex];
                musicPlaying.Source.Play();
                yield return new WaitForSeconds(musicPlaying.clip.length);
                if (!loopCurrentMusic)
                {
                    musicPlayingIndex = musicPlayingIndex == musicClip.Length - 1 ? 0 : musicPlayingIndex + 1;
                }
            }
            while (loopMusicList);
        }
    }

    public void StopMusic()
    {
        musicPlaying.Source.Stop();
        StopCoroutine(music);
        music = null;
    }

    public void ToggleMusic()
    {
        if (music != null)
        {
            Debug.Log("stop");
            StopMusic();
        }
        else
        {
            Debug.Log("start");
            music = StartCoroutine(PlayMusic());
        }
    }

    private Audio AudioClipByName(string name)
    {
        Audio a = Array.Find(audioClip, sound => sound.name == name);
        if (a == null)
        {
            Debug.Log("Can't find sound with name: " + name);
        }
        return a;
    }

    private Audio MusicClipByName(string name)
    {
        Audio a = Array.Find(musicClip, sound => sound.name == name);
        if (a == null)
        {
            Debug.Log("Can't find music with name: " + name);
        }
        return a;
    }

    public void PlayAudioClip(string name)
    {
        Audio a = AudioClipByName(name);
        a.Source.Play();
    }
    public void PlayMusicClip(string name)
    {
        Audio m = MusicClipByName(name);
        m.Source.Play();
    }

}
