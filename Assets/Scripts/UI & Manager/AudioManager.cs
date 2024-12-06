using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [Header("BGM")]
    public AudioClip[] bgmClips;
    public float bgmVolume;
    private AudioSource[] bgmPlayers;
    public int bgmChannelIndex;

    [Header("SFX")]
    public AudioClip[] sfxClips;
    public float sfxVolume;
    private AudioSource[] sfxPlayers;
    private Dictionary<string, AudioSource> activeSfx = new Dictionary<string, AudioSource>();
    public int sfxChannelIndex;
    //
    // [Header("Alert")]
    // public AudioClip[] alertClips;
    // public float alertVolume;
    // private AudioSource[] alertPlayers;
    // private int alertChannelIndex;

    public enum Bgm
    {
        StartingScene,
        StageSelection,
        Stage1,
        Stage2,
        Stage3,
        OminousSound,
        Ending,
        GameOver,
        Opening
    }

    public enum Sfx
    {
        Fire,
        BossTroopComing,
        BossStepSound,
        BossPunch,
        BossGrowl,
        BossWalkingAppears,
        MissileFinalDetect,
        MissileExplosion,
        MissileTargetDetected,
        MissileLaunch,
        PlayerBullet,
        MissileFlying,
        PlayerMine,
        TurretOff,
        TurretOn
    }

    public enum Alert
    {
        ControlUnitIsUnderAttack
    }

    void Awake()
    {
        base.Awake();
        Init();
    }

    void Init()
    {
        GameObject bgmObject = new GameObject("BGM");
        bgmObject.transform.parent = transform;
        bgmPlayers = new AudioSource[bgmChannelIndex];
        for (int i = 0; i < bgmPlayers.Length; i++)
        {
            bgmPlayers[i] = bgmObject.AddComponent<AudioSource>();
            bgmPlayers[i].playOnAwake = false;
            bgmPlayers[i].volume = bgmVolume;
        }


        GameObject sfxObject = new GameObject("SFXPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[sfxChannelIndex];

        for (int i = 0; i < sfxPlayers.Length; i++)
        {
            sfxPlayers[i] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[i].playOnAwake = false;
            sfxPlayers[i].volume = sfxVolume;
        }
    }
    

    // BGM 재생
    public void PlayBGM(Bgm bgm, bool isPlay)
    {
        for (int i = 0; i < bgmPlayers.Length; i++)
        {
            int loopIndex = (i + bgmChannelIndex) % bgmPlayers.Length;
            // bgmChannelIndex = loopIndex;
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
        }
    }

    // SFX 재생 (폴링 방식)
    public string PlaySfx(Sfx sfx)
    {
        int loopIndex = sfxChannelIndex % sfxPlayers.Length;
        sfxChannelIndex++;

        AudioSource source = sfxPlayers[loopIndex];
        source.clip = sfxClips[(int)sfx];
        source.Play();

        string id = System.Guid.NewGuid().ToString(); // 고유 ID 생성
        activeSfx[id] = source;

        return id;
    }

    // 특정 SFX 중지
    public void StopSfx(string id)
    {
        if (activeSfx.ContainsKey(id)&&activeSfx[id]!=null)
        {
            activeSfx[id].Stop();
            activeSfx.Remove(id);
        }
    }

    // 모든 SFX 중지
    public void StopAllSfx()
    {
        foreach (var source in activeSfx.Values)
        {
            source.Stop();
        }
        activeSfx.Clear();
    }

    // // Alert SFX 재생
    // public void PlayAlert(Alert alert, bool isPlay)
    // {
    //     int loopIndex = alertChannelIndex % alertPlayers.Length;
    //     alertChannelIndex++;
    //
    //     if (isPlay)
    //     {
    //         if (!alertPlayers[loopIndex].isPlaying)
    //         {
    //             alertPlayers[loopIndex].clip = alertClips[(int)alert];
    //             alertPlayers[loopIndex].Play();
    //         }
    //     }
    //     else
    //     {
    //         alertPlayers[loopIndex].Stop();
    //     }
    // }

    public void UIBgm(bool isPlay) // UI 창을 띄웠을 때 고음만 통과시켜 간지나게 함
    {
        AudioHighPassFilter bgmEffect = Camera.main.GetComponent<AudioHighPassFilter>();
        bgmEffect.enabled = isPlay;
    }

    public void ChangeBgmVolume(float vol)
    {
        bgmVolume = vol;
        for (int i = 0; i < bgmPlayers.Length; i++)
        {
            bgmPlayers[i].volume = bgmVolume;
        }
    }

    public void ChangeSfxVolume(float vol)
    {
        sfxVolume = vol;

        for (int i = 0; i < sfxPlayers.Length; i++)
        {
            sfxPlayers[i].volume = sfxVolume;
        }
    }
}
