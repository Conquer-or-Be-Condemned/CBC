using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SearchService;
using UnityEngine.UI;

/*
 *  InGame 내에서 Wave에 대한 정보와 보상 관련 정보를 다루는 스크립트입니다.
 *  GameManager에서 모든 일을 담당할 수 없기에 스크립트를 분리했습니다.
 */

public class InGameManager : MonoBehaviour
{
    [Header("Status")]
    public int curWave;
    public int maxWave;

    //  웨이브 중인지 판단하는 변수
    [Header("Wave")]
    public bool isWave;
    public bool spawnEnd;
    
    //  Wave 진행 버튼
    public GameObject waveStart;
    public GameObject startWrapper;
    
    //  Wave 출력 Text
    [Header("Wave Info")]
    public TMP_Text waveInfo;
    public GameObject waveWrapper;
    
    //  Wave 몬스터 수
    public int maxSpawn;
    public int dieSpawn;
    public int curSpawn;

    //  대화창 끝남을 확인
    private bool talkEnd;
    //  대화 중 움직임 차단
    public bool isTalking;
    //  현재 대화의 진전도
    private int talkIdx;
    
    private void Start()
    {
        Init();
        
        if (GameManager.isNewGame && GameManager.tutorialEnd)
        {
            //  진전도 초기화
            talkIdx = 1;
            
            isTalking = true;
            int curSceneId = 0;
            int idx = 0;

            //  현재 씬 분석
            switch (SceneController.NowScene)
            {
                case "MapText" :
                    curSceneId = 100;
                    break;
            }
            
            while (true)
            {
                string buff = TalkManager.GetTalk(curSceneId + talkIdx, idx);
                idx++;

                if (buff == null)
                {
                    isTalking = false;
                    break;
                }
                
                //  TODO : 출력
            }
            
            talkEnd = true;
            talkIdx++;
        }
        
        ShowButton();
    }

    private void Init()
    {
        //  Wave를 위한 Static 소환
        StageInfoManager.SetStageInfo();
        StageInfoManager.SetWaveInfo();
        
        curWave = 1;
        maxWave = StageInfoManager.GetStageInfo();
        isWave = false;
        
        //  필요한 오브젝트 검색
        waveStart = GameObject.Find("StartButton");
        startWrapper = GameObject.Find("WaveStart");
        waveInfo = GameObject.Find("WaveInfo").GetComponent<TMP_Text>();
        waveWrapper = GameObject.Find("WaveWrapper");
        
        waveWrapper.SetActive(false);
        
        talkEnd = false;
        isTalking = false;
    }

    private void FixedUpdate()
    {
        if (isWave)
        {
            CheckWaveClear();
        }
    }

    public void ListenMonsterDie()
    {
        dieSpawn++;
    }

    public bool AddAndCheckCurSpawn(int offset)
    {
        curSpawn += offset;
        
        if (curSpawn > StageInfoManager.GetWaveInfo(curWave))
        {
            spawnEnd = true;
            return true;
        }

        return false;
    }

    public void StartWave()
    {
        Debug.Log("Wave Start!!");
        isWave = true;
        spawnEnd = false;
        
        InitWave();
        ShowInfo();
        HideButton();
    }

    private void InitWave()
    {
        maxSpawn = StageInfoManager.GetWaveInfo(curWave);
        dieSpawn = 0;
        curSpawn = 0;
    }
    
    private void CheckWaveClear(){
        
        if (dieSpawn == curSpawn && spawnEnd)
        {
            isWave = false;
            ShowWaveClear();
            ShowButton();
    
            curWave++;
            CheckStageClear();
            
            //  Player 회복
            GameManager.Instance.player.GetComponent<PlayerInfo>().RecoverHp();
        }
    }
    
    private void CheckStageClear()
    {
        if (curWave > maxWave)
        {
            Debug.Log("Stage Clear");
            SceneController.ChangeScene("StageMenu");

            GameManager.InGame = false;
        }
        
        //  TODO : 이후 스테이지 클리어 화면과 함께 스테이지 선택 씬으로 넘어갈 것.
    }
    
    private void ShowInfo()
    {
        if (curWave == maxWave)
        {
            waveInfo.SetText("Final Wave");
        }
        else
        {
            waveInfo.SetText("Wave " + curWave);
        }
        
        waveWrapper.SetActive(true);
        
        StartCoroutine(ShowInfoCoroutine());
    }

    private void ShowWaveClear()
    {
        waveInfo.SetText("Wave Clear!");
        waveWrapper.SetActive(true);
        
        StartCoroutine(ShowInfoCoroutine());
    }
    
    //  Animation은 Alpha값 조정으로 임시 결정
    private IEnumerator ShowInfoCoroutine()
    {
        var alpha = 0f;
        
        while (true)
        {
            if (alpha >= 1f)
            {
                StartCoroutine(HideInfoCoroutine());
                yield break;
            }

            alpha += 0.005f;
            waveInfo.color = new Color(waveInfo.color.r, waveInfo.color.g, waveInfo.color.b, alpha);
            yield return new WaitForSeconds(0.01f);
        }
    }

    private IEnumerator HideInfoCoroutine()
    {
        var alpha = 1f;
        while (true)
        {
            if (alpha <= 0)
            {
                waveWrapper.SetActive(false);
                yield break;
            }

            alpha -= 0.005f;
            waveInfo.color = new Color(waveInfo.color.r, waveInfo.color.g, waveInfo.color.b, alpha);
            yield return new WaitForSeconds(0.02f);
        }
    }
    
    private void ShowButton()
    {
        waveStart.GetComponent<Button>().interactable = true;
        startWrapper.GetComponent<Animator>().SetBool("visible",true);
    }
    
    private void HideButton()
    {
        waveStart.GetComponent<Button>().interactable = false;
        startWrapper.GetComponent<Animator>().SetBool("visible",false);
    }
}
