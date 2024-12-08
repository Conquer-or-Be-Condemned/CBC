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
        TurretOn,
        WolfComing,
        DragonComing,
        HorseComing,
        HorseMoving
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
    public void PlaySFXOnce(Sfx sfx, bool isPlay)
    {
        for (int i = 0; i < sfxPlayers.Length; i++)
        {
            int loopIndex = (i + sfxChannelIndex) % sfxPlayers.Length;
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx];
            sfxPlayers[loopIndex].volume = sfxVolume/10f;
            Debug.Log(sfxPlayers[loopIndex].volume);
            if (isPlay)
            {
                sfxPlayers[loopIndex].Play();
            }
            else
            {
                sfxPlayers[loopIndex].Stop();
            }
        }
    }
    // SFX 재생 (폴링 방식)
    public string PlaySfx(Sfx sfx)
    {
        int loopIndex = sfxChannelIndex % sfxPlayers.Length;
        sfxChannelIndex++;
        if (sfxChannelIndex >= 1000) sfxChannelIndex = 100;
        AudioSource source = sfxPlayers[loopIndex];
        foreach (var src in activeSfx.Values)//먼저 들어온 sfx 볼륨 감소
        {
            src.volume -= 0.1f;
        }
        activeSfx.Clear();
        //초기화
        source.volume = sfxVolume;
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
