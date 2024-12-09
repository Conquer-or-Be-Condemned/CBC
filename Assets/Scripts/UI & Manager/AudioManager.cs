using System.Collections;
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

    private GameObject sfxObject;
    private GameObject bgmObject;
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
        WolfComing2,
        WolfBark,
        WolfBite,
        WolfSpawn,
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
        bgmObject = new GameObject("BGM");
        bgmObject.transform.parent = transform;
        bgmPlayers = new AudioSource[bgmChannelIndex];
        for (int i = 0; i < bgmPlayers.Length; i++)
        {
            bgmPlayers[i] = bgmObject.AddComponent<AudioSource>();
            bgmPlayers[i].playOnAwake = false;
            bgmPlayers[i].volume = bgmVolume;
        }


        sfxObject = new GameObject("SFXPlayer");
        sfxObject.transform.parent = transform;
        // sfxPlayers = new AudioSource[sfxChannelIndex];
        //
        // for (int i = 0; i < sfxPlayers.Length; i++)
        // {
        //     sfxPlayers[i] = sfxObject.AddComponent<AudioSource>();
        //     sfxPlayers[i].playOnAwake = false;
        //     sfxPlayers[i].volume = sfxVolume;
        // }
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
    // public void PlaySFXOnce(Sfx sfx, bool isPlay)
    // {
    //     for (int i = 0; i < sfxPlayers.Length; i++)
    //     {
    //         int loopIndex = (i + sfxChannelIndex) % sfxPlayers.Length;
    //         sfxPlayers[loopIndex].clip = sfxClips[(int)sfx];
    //         sfxPlayers[loopIndex].volume = sfxVolume/10f;
    //         Debug.Log(sfxPlayers[loopIndex].volume);
    //         if (isPlay)
    //         {
    //             sfxPlayers[loopIndex].Play();
    //         }
    //         else
    //         {
    //             sfxPlayers[loopIndex].Stop();
    //         }
    //     }
    // }
    // SFX 재생 (폴링 방식)
    public string PlaySfx(Sfx sfx)
    {
        foreach (var src in activeSfx.Values)//먼저 들어온 sfx 볼륨 감소
        {
            src.volume -= 0.01f;
        }
        //초기화
        AudioSource source = sfxObject.AddComponent<AudioSource>();
        source.volume = sfxVolume;
        source.clip = sfxClips[(int)sfx];
        source.Play();

        string id = System.Guid.NewGuid().ToString(); // 고유 ID 생성
        activeSfx[id] = source;
        StartCoroutine(RemoveSfxWhenFinished(id, source));
        return id;
    }
    public string PlaySfx(Sfx sfx,float volume)//볼륨을 커스텀 가능
    {
        foreach (var src in activeSfx.Values)//먼저 들어온 sfx 볼륨 감소
        {
            src.volume -= 0.01f;
        }
        //초기화
        AudioSource source = sfxObject.AddComponent<AudioSource>();
        source.volume = volume;
        source.clip = sfxClips[(int)sfx];
        source.Play();

        string id = System.Guid.NewGuid().ToString(); // 고유 ID 생성
        activeSfx[id] = source;
        StartCoroutine(RemoveSfxWhenFinished(id, source));
        return id;
    }
    // 특정 SFX 중지
    public void StopSfx(string id)
    {
        if (activeSfx.ContainsKey(id))
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
    public void UIBgm(bool isPlay) // UI 창을 띄웠을 때 고음만 통과시켜 간지나게 함
    {
        AudioHighPassFilter bgmEffect = Camera.main.GetComponent<AudioHighPassFilter>();
        bgmEffect.enabled = isPlay;
    }
// 재생 종료 후 activeSfx에서 제거
    private IEnumerator RemoveSfxWhenFinished(string id, AudioSource source)
    {
        // AudioSource 재생이 끝날 때까지 대기
        yield return new WaitUntil(() => !source.isPlaying);

        // activeSfx에서 제거
        if (activeSfx.ContainsKey(id))
        {
            activeSfx.Remove(id);
            Destroy(source); // AudioSource 컴포넌트 제거
            Debug.Log($"SFX with ID {id} has finished and removed from activeSfx.");
        }
    }
}
