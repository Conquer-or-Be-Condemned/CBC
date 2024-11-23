using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

/*
 *  Stage 선택 창에서 Stage에 대한 정보를 출력하고 저장하는 스크립트입니다.
 */

public class StageInfoManager : MonoBehaviour
{
    
    //  UI의 정보들
    [Header("UI Instance")]
    [SerializeField] private TMP_Text planetName;
    [SerializeField] private TMP_Text planetStory;
    [SerializeField] private TMP_Text planetInfo;
    [SerializeField] private GameObject planetSet;
    [SerializeField] private Animator animator;
    
    public GameObject warpButton;
    
    //  정보 저장을 위한 List
    [Header("Planet Info List")]
    public static List<string> NameList = new List<string>();
    public static List<string> StoryList= new List<string>();
    public static List<string> InfoList = new List<string>();

    public static bool StageInit;
    
    //  각 스테이지의 웨이브 정보를 저장하는 클래스 타입
    [Header("Wave Info")]
    public static List<int> StageInfo = new List<int>();
    public static List<List<int>> WaveInfo = new List<List<int>>();
    
    //  현재 Display되고 있는 스테이지
    private void Awake()
    {
        AudioManager.Instance.PlayBGM(AudioManager.Bgm.StageSelection,true);
        
        //  Static 초기화는 GameManager에서 진행합니다.

        if (planetName == null) planetName = GameObject.Find("PlanetName").GetComponent<TMP_Text>();
        if (planetStory == null) planetStory = GameObject.Find("PlanetStory").GetComponent<TMP_Text>();
        if (planetInfo == null) planetInfo = GameObject.Find("PlanetInfo").GetComponent<TMP_Text>();
        if (planetSet == null) planetSet = GameObject.Find("PlanetSet");
        if (warpButton == null) warpButton = GameObject.Find("WarpButton");

        if (warpButton == null) Debug.LogError("이거 오류뜨는건 어쩔 수 없습니다. 신경 쓰지마세요.");
        animator = planetSet.GetComponent<Animator>();
        
        warpButton.SetActive(false);
    }

    private void Start()
    {
        //  모든 정보를 공백으로 초기화
        planetName.SetText("");
        planetStory.SetText("");
        planetInfo.SetText("");
        
        StageInit = false; 
        StartCoroutine(WaitShowCoroutine());
    }

    //  Stage 정보 메뉴를 숨기는 함수
    public void SetStageMenuHide()
    {
        animator.SetBool("isWarp", true);
        warpButton.SetActive(false);
    }

    //  씬 전환 직후 애니메이션과 워프 버튼에 딜레이 부여
    private IEnumerator WaitShowCoroutine()
    {
        yield return new WaitForSeconds(1.05f);
        
        warpButton.SetActive(true);
        StageInit = true;
        Show();
    }

    //  String Builder를 사용하기 위한 코루틴
    private void Show()
    {
        StartCoroutine(NameCoroutine());
    }
    
    private IEnumerator NameCoroutine()
    {
        StringBuilder stringBuilder = new StringBuilder();

        for (var i = 0; i < NameList[GameManager.CurStage-1].Length; i++)
        {
            stringBuilder.Append(NameList[GameManager.CurStage - 1][i]);
            planetName.text = stringBuilder.ToString();
            yield return new WaitForSeconds(0.05f);
        }
        StartCoroutine(StoryCoroutine());
    }
    
    //  NameCoroutine 다음에 연달아 실행
    private IEnumerator StoryCoroutine()
    {
        StringBuilder stringBuilder = new StringBuilder();

        for (var i = 0; i < StoryList[GameManager.CurStage-1].Length; i++)
        {
            stringBuilder.Append(StoryList[GameManager.CurStage - 1][i]);
            planetStory.text = stringBuilder.ToString();
            yield return new WaitForSeconds(0.005f);
        }
        StartCoroutine(InfoCoroutine());
    }
    //  마찬가지로 연달아 실행
    private IEnumerator InfoCoroutine()
    {
        StringBuilder stringBuilder = new StringBuilder();

        for (int i = 0; i < InfoList[GameManager.CurStage-1].Length; i++)
        {
            stringBuilder.Append(InfoList[GameManager.CurStage - 1][i]);
            planetInfo.text = stringBuilder.ToString();
            yield return new WaitForSeconds(0.005f);
        }
    }

    //  정보 저장을 위한 Method
    public static void SetPlanet()
    {
        NameList.Add("HJD-1029X2");
        
        StoryList.Add("  This planet will be our first destination. It has similar environment with Earth" +
                      ", but has strong enemy forces defending it.\n\n" +
                      "  Our vanguard tried their best to conquer, " +
                      "but now we can't find traces of them anymore. " +
                      "Let's colonize this Planet.\nTake care and Focus Developer.....\nGood Luck.....");
        
        InfoList.Add("Average temperature: 15.6\u00b0C\nPlanet diameter: 12,564 km\nBiological Population: 145,235,520\nPlanet type: Earth-type planet");
    }
    
    public static string GetCurStageName()
    {
        return NameList[GameManager.CurStage - 1];
    }
    
    //  웨이브 저장을 위한 Method
    public static void SetStageInfo()
    {
        StageInfo.Add(3);
    }

    public static void SetWaveInfo()
    {
        //  Stage 1 - Wave 9개 (임시 3개)
        WaveInfo.Add(new List<int> {30,30,30,30,30,30,30,30,30});
    }
    
    public static int GetStageInfo()
    {
        return StageInfo[GameManager.CurStage - 1];
    }

    public static int GetWaveInfo(int curWave)
    {
        return WaveInfo[GameManager.CurStage - 1][curWave - 1];
    }
}
