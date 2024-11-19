using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class AudioManager : Singleton<AudioManager>
{
    // public static AudioManager instance;
    [Header("BGM")] public AudioClip[] bgmClips;
    public float bgmVolume;
    public int bgmChannels;
    AudioSource[] bgmPlayers;
    int bgmChannelIndex;
    [Header("SFX")] public AudioClip[] sfxClips;
    public float sfxVolume;
    public int sfxChannels;
    AudioSource[] sfxPlayers;
    int sfxChannelIndex;

    public enum Bgm
    {
        StartingScene,
        StageSelection,
        Stage1
    }

    public enum Sfx
    {
        fire
    }

    void Awake()
    {
        base.Awake();
        // AudioManager.Instance = this;
        Init();
    }

    void Init()
    {
        GameObject bgmObject = new GameObject("BGM");
        bgmObject.transform.parent = transform;
        bgmPlayers = new AudioSource[bgmChannels];
        for (int i = 0; i < bgmPlayers.Length; i++)
        {
            bgmPlayers[i] = bgmObject.AddComponent<AudioSource>();
            bgmPlayers[i].playOnAwake = false;
            bgmPlayers[i].volume = bgmVolume;
        }


        GameObject sfxObject = new GameObject("SFXPLayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[sfxChannels];

        for (int i = 0; i < sfxPlayers.Length; i++)
        {
            sfxPlayers[i] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[i].playOnAwake = false;
            sfxPlayers[i].volume = sfxVolume;
        }
    }
    
    //  TODO : 아예 브금을 안나오게 하는 Method 있으면 좋을 듯

    public void PlayBGM(Bgm bgm, bool isPlay)
    {
        Debug.Log(bgmPlayers.Length);
        for (int i = 0; i < bgmPlayers.Length; i++)
        {
            int loopIndex = (i + bgmChannelIndex) % bgmPlayers.Length;
            bgmChannelIndex = loopIndex;
            bgmPlayers[loopIndex].clip = bgmClips[(int)bgm];
            if (isPlay)
            {
                if (bgmPlayers[loopIndex].isPlaying)
                    continue;
                bgmPlayers[loopIndex].Play();
            }
            else
            {
                bgmPlayers[loopIndex].Stop();
            }
            Debug.Log(bgm);
            // break;
        }

        // if (isPlay)
        // {
        //     bgmPlayers[(int)bgm].Play();
        // }
        // else
        // {
        //     bgmPlayers[(int)bgm].Stop();
        // }
    }

    public void PlaySfx(Sfx sfx)
    {
        Debug.Log(sfxPlayers.Length);
        for (int i = 0; i < sfxPlayers.Length; i++)
        {
            int loopIndex = (i + sfxChannelIndex) % sfxPlayers.Length;
            // if (sfxPlayers[loopIndex].isPlaying)
            //     continue;
            sfxChannelIndex = loopIndex;
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx];
            sfxPlayers[loopIndex].Play();
            Debug.Log("BGM Play");
            break;
        }
    }
}