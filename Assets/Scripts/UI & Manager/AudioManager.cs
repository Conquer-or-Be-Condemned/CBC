using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [Header("BGM")]
    public AudioClip[] bgmClips;
    public float bgmVolume;
    private AudioSource[] _bgmPlayers;
    private int _bgmChannelIndex;

    [Header("SFX")]
    public AudioClip[] sfxClips;
    public float sfxVolume;
    private AudioSource[] _sfxPlayers;
    private Dictionary<string, AudioSource> _activeSfx = new Dictionary<string, AudioSource>();

    private GameObject _sfxObject;
    private GameObject _bgmObject;
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
        HorseMoving,
        SpaceShipHover,
        SpaceShipPassing
    }
    
    void Awake()
    {
        base.Awake();
        _bgmChannelIndex = bgmClips.Length;
        Init();
    }

    void Init()
    {
        //BGM Initialization
        _bgmObject = new GameObject("BGM");
        _bgmObject.transform.parent = transform;
        _bgmPlayers = new AudioSource[_bgmChannelIndex];
        for (int i = 0; i < _bgmPlayers.Length; i++)
        {
            _bgmPlayers[i] = _bgmObject.AddComponent<AudioSource>();
            _bgmPlayers[i].playOnAwake = false;
            _bgmPlayers[i].volume = bgmVolume;
        }
        //SFX Initialization
        _sfxObject = new GameObject("SFXPlayer");
        _sfxObject.transform.parent = transform;
    }
    

    // BGM 재생
    public void PlayBGM(Bgm bgm, bool isPlay)
    {
        for (int i = 0; i < _bgmPlayers.Length; i++)
        {
            int loopIndex = (i + _bgmChannelIndex) % _bgmPlayers.Length;
            _bgmPlayers[loopIndex].clip = bgmClips[(int)bgm];
            if (isPlay)
            {
                if (_bgmPlayers[loopIndex].isPlaying)
                    continue;
                _bgmPlayers[loopIndex].Play();
            }
            else
            {
                _bgmPlayers[loopIndex].Stop();
            }
        }
    }

    // SFX 재생 (폴링 방식)
    public string PlaySfx(Sfx sfx)
    {
        // foreach (var src in _activeSfx.Values)//먼저 들어온 sfx 볼륨 감소
        // {
        //     src.volume -= 0.005f;
        // }
        //초기화
        AudioSource source = _sfxObject.AddComponent<AudioSource>();
        source.dopplerLevel = 0.0f;
        source.reverbZoneMix = 0.0f;
        source.volume = sfxVolume;
        source.clip = sfxClips[(int)sfx];
        // if (sfx == Sfx.MissileTargetDetected) source.volume =sfxVolume/2f;
        if (sfx == Sfx.MissileFinalDetect) source.loop = true;
        source.Play();

        string id = System.Guid.NewGuid().ToString(); // 고유 ID 생성
        _activeSfx[id] = source;
        StartCoroutine(RemoveSfxWhenFinished(id, source));
        return id;
    }
    // public string PlaySfx(Sfx sfx,float volume)//볼륨을 커스텀 가능
    // {
    //     foreach (var src in _activeSfx.Values)//먼저 들어온 sfx 볼륨 감소
    //     {
    //         src.volume -= 0.03f;
    //     }
    //     //초기화
    //     AudioSource source = _sfxObject.AddComponent<AudioSource>();
    //     source.dopplerLevel = 0.0f;
    //     source.reverbZoneMix = 0.0f;
    //     source.volume = volume;
    //     source.clip = sfxClips[(int)sfx];
    //     source.Play();
    //
    //     string id = System.Guid.NewGuid().ToString(); // 고유 ID 생성
    //     _activeSfx[id] = source;
    //     StartCoroutine(RemoveSfxWhenFinished(id, source));
    //     return id;
    // }
    //
    // 특정 SFX 중지
    public void StopSfx(string id)
    {
        if (_activeSfx.ContainsKey(id))
        {
            _activeSfx[id].Stop(); 
            _activeSfx.Remove(id);
        }
    }

    // 모든 SFX 중지
    public void StopAllSfx()
    {
        foreach (var source in _activeSfx.Values)
        {
            source.Stop();
        }
        _activeSfx.Clear();
    }
    
    public void ChangeBgmVolume(float vol)
    {
        bgmVolume = vol;
        for (int i = 0; i < _bgmPlayers.Length; i++)
        {
            _bgmPlayers[i].volume = bgmVolume;
        }
    }

    public void ChangeSfxVolume(float vol)
    {
        foreach (var source in _activeSfx.Values)
            source.volume = vol;
        sfxVolume = vol;
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
        if (_activeSfx.ContainsKey(id))
        {
            _activeSfx.Remove(id);
            Destroy(source); // AudioSource 컴포넌트 제거
            // Debug.Log($"SFX with ID {id} has finished and removed from activeSfx.");
        }
    }
}
