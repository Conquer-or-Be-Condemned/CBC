using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using Unity.Properties;
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
    [Header("Status")] public int curWave;
    public int maxWave;
    public int curSceneId;

    //  웨이브 중인지 판단하는 변수
    [Header("Wave")] public bool isWave;
    public bool spawnEnd;

    //  Wave 진행 버튼
    public GameObject waveStart;
    public GameObject startWrapper;
    public GameObject waveStartText;

    //  Wave 출력 Text
    [Header("Wave Info")] public TMP_Text waveInfo;
    public GameObject waveWrapper;

    //  Wave 몬스터 수
    public int maxSpawn;
    public int dieSpawn;
    public int curSpawn;

    [Header("Talk Management")]
    //  대화창 끝남을 확인
    private bool talkEnd;

    //  대화 중 움직임 차단
    public bool isTalking;

    //  현재 대화의 진전도
    private int talkIdx;

    [Header("Talk UIs")] public GameObject talkWrapper;
    public GameObject talkBox;
    public TMP_Text talkText;

    [Header("Pause and Setting")] public GameObject pauseSet;
    public bool pauseVisible;
    public GameObject settings;
    public bool settingVisible;
    public GameObject operationKey;
    public bool operationKeyVisible;
    public GameObject blind;

    private void Start()
    {

        //  Pause, Settings
        pauseVisible = false;
        settingVisible = false;

        //  Wave를 위한 Static 호출
        StageInfoManager.SetStageInfo();
        StageInfoManager.SetWaveInfo();

        curWave = 1;
        maxWave = StageInfoManager.GetStageInfo();
        isWave = false;

        talkWrapper.GetComponent<Animator>().SetBool("isShow", true);

        talkEnd = false;
        isTalking = false;

        if (GameManager.IsNewGame && !GameManager.TutorialEnd)
        {
            //  진전도 초기화
            talkIdx = 1;

            StartCoroutine(TalkProcess());
        }
        else
        {
            ShowButton();
        }
    }

    //  대화 중에는 스킵 불가능
    private void Update()
    {
        if (!isTalking)
        {
            CheckKeyBoardInput();
        }
    }

    //  Blind를 통해 다른 UI 클릭을 방지한다.
    private void CheckKeyBoardInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (settingVisible)
            {
                settingVisible = false;
                settings.SetActive(settingVisible);
            }
            else if (operationKeyVisible)
            {
                operationKeyVisible = false;
                operationKey.SetActive(operationKeyVisible);
            }
            else
            {
                pauseVisible = !pauseVisible;
                pauseSet.SetActive(pauseVisible);
                blind.SetActive(pauseVisible);

                if (pauseVisible)
                {
                    Time.timeScale = 0f;
                }
                else
                {
                    Time.timeScale = 1f;
                }
            }
        }
    }
    
    public void ShowSettings()
    {
        settingVisible = true;
        settings.SetActive(settingVisible);
    }

    public void ShowOperationKey()
    {
        operationKeyVisible = true;
        operationKey.SetActive(operationKeyVisible);
    }

    private IEnumerator TalkProcess()
    {
        CheckSceneId();

        isTalking = true;
        int idx = 0;

        talkWrapper.SetActive(true);

        while (true)
        {
            string buff = TalkManager.GetTalk(curSceneId + talkIdx, idx);

            if (buff == null)
            {
                break;
            }

            //  이 코루틴이 종료될 때까지 대기후 idx++ (연산 오류 발생 방지)
            yield return StartCoroutine(TalkCoroutine(buff));
            idx++;

            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        }

        talkIdx++;
        talkEnd = true;
        isTalking = false;

        talkWrapper.GetComponent<Animator>().SetBool("isShow", false);

        ShowButton();
    }

    private IEnumerator TalkCoroutine(string buff)
    {
        talkText.text = "";
        StringBuilder stringBuilder = new StringBuilder();

        for (int i = 0; i < buff.Length; i++)
        {
            stringBuilder.Append(buff[i]);
            talkText.text = stringBuilder.ToString();
            yield return new WaitForSeconds(0.005f);
        }
    }

    private void CheckSceneId()
    {
        //  현재 씬 분석
        int cnt = 1;
        foreach(var e in SceneController.stageList)
        {
            if (SceneController.NowScene == e)
            {
                curSceneId = 100 * cnt;
                break;
            }

            cnt++;
        }
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
        //  오작동 방지
        if (Input.GetKeyDown(KeyCode.Space))
        {
            return;
        }
        
        Debug.Log("Wave Start");
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

    private void CheckWaveClear()
    {
        if (dieSpawn == curSpawn && spawnEnd)
        {
            isWave = false;
            ShowWaveClear();

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

        StartCoroutine(ShowInfoCoroutine(false));
    }

    private void ShowWaveClear()
    {
        waveInfo.SetText("Wave Clear");
        waveWrapper.SetActive(true);

        StartCoroutine(ShowInfoCoroutine(true));
    }

    //  Animation은 Alpha값 조정으로 임시 결정
    private IEnumerator ShowInfoCoroutine(bool isClear)
    {
        var alpha = 0f;

        while (true)
        {
            if (alpha >= 1f)
            {
                StartCoroutine(HideInfoCoroutine());
                break;
            }

            alpha += 0.005f;
            waveInfo.color = new Color(waveInfo.color.r, waveInfo.color.g, waveInfo.color.b, alpha);
            yield return new WaitForSeconds(0.006f);
        }

        //  빠른 버튼 클릭으로 인한 버그 방지
        if (isClear)
        {
            ShowButton();
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
            yield return new WaitForSeconds(0.004f);
        }
    }

    private void ShowButton()
    {
        waveStart.GetComponent<Button>().interactable = true;
        startWrapper.GetComponent<Animator>().SetBool("visible", true);

        StartCoroutine(StartTextCoroutine());
    }

    private IEnumerator StartTextCoroutine()
    {
        while (true)
        {
            if (!waveStart.GetComponent<Button>().interactable)
            {
                Debug.LogError("ENGGGGGG");
                yield break;
            }
            
            waveStartText.GetComponent<Animator>().SetBool("big", true);
            yield return new WaitForSeconds(1f);
            
            waveStartText.GetComponent<Animator>().SetBool("big", false);
            yield return new WaitForSeconds(1f);
        }
    }

    private void HideButton()
    {
        waveStart.GetComponent<Button>().interactable = false;
        startWrapper.GetComponent<Animator>().SetBool("visible", false);
    }
}