using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SearchService;
using UnityEngine.UI;

/*
 *  InGame 내에서 Wave에 대한 정보와 보상 관련 정보를 다루는 스크립트입니다.
 *  GameManager에서 모든 일을 담당할 수 없기에 스크립트를 분리했습니다.
 */
public class InGameManager : MonoBehaviour
{
    public int curWave;
    public int maxWave;

    //  웨이브 중인지 판단하는 변수
    public bool isWave;
    
    //  Wave 진행 버튼
    public GameObject waveStart;
    public GameObject waveWrapper;
    public TMP_Text waveStartText;
    
    //  Wave 출력 Text
    public TMP_Text waveInfo;
    public GameObject waveInfoWrapper;
    
    
    //  게임 시작
    private void Start()
    {
        //  Wave를 위한 Static 소환
        StageInfoManager.SetWaveInfo();
        
        curWave = 1;
        maxWave = StageInfoManager.GetWaveInfo();
        isWave = false;
        
        //  필요한 오브젝트 검색
        waveStart = GameObject.Find("WaveStart");
        waveWrapper = GameObject.Find("WaveWrapper");
        waveStartText = GameObject.Find("WaveStartText").GetComponent<TMP_Text>();
        waveInfo = GameObject.Find("WaveInfo").GetComponent<TMP_Text>();
        waveInfoWrapper = GameObject.Find("WaveInfoWrapper");
        
        waveInfoWrapper.SetActive(false);
    }
    
    public void StartWave()
    {
        isWave = true;
        
        ShowInfo();
        HideButton();
    }
    
    private void CheckWaveClear(){
        //  TODO : 클리어 조건을 달성할 시 아래의 코드 실행
        isWave = false;

        curWave++;
        CheckStageClear();
    }

    private void CheckStageClear()
    {
        if (curWave >= maxWave)
        {
            Debug.Log("Stage Clear");
        }
        
        //  TODO : 이후 스테이지 클리어 화면과 함께 스테이지 선택 씬으로 넘어갈 것.
    }

    private void ShowInfo()
    {
        waveInfo.SetText("Wave " + curWave +" / " + maxWave);
        waveInfoWrapper.SetActive(true);
        StartCoroutine(ShowInfoCoroutine());
    }

    private IEnumerator ShowInfoCoroutine()
    {
        yield return new WaitForSeconds(3f);
        waveInfoWrapper.SetActive(false);
    }

    private void ShowButton()
    {
        //  TODO : 애니메이션
    }

    private void HideButton()
    {
        waveWrapper.SetActive(false);
    }

}
