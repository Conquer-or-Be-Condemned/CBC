
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;


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
    public static List<string> NameList_ENG = new List<string>();
    public static List<string> StoryList_ENG = new List<string>();
    public static List<string> InfoList_ENG = new List<string>();
    
    public static List<string> NameList_KOR = new List<string>();
    public static List<string> StoryList_KOR = new List<string>();
    public static List<string> InfoList_KOR = new List<string>();

    public static bool StageInit;
    
    //  각 스테이지의 웨이브 정보를 저장하는 클래스 타입
    [Header("Wave Info")]
    public static List<int> StageInfo = new List<int>();
    public static List<List<int>> WaveInfo = new List<List<int>>();

    [Header("Stage Rewards")] public static int[] StageRewards = { 200,200,200 };
    
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
    public void Show()
    {
        planetName.SetText("");
        planetStory.SetText("");
        planetInfo.SetText("");
        StartCoroutine(NameCoroutine());
    }
    
    private IEnumerator NameCoroutine()
    {
        planetName.SetText("");
        StringBuilder stringBuilder = new StringBuilder();

        if (GameManager.Language == 0)
        {
            for (var i = 0; i < NameList_ENG[GeneralManager.Instance.stageSelectManager.curSelectStage].Length; i++)
            {
                stringBuilder.Append(NameList_ENG[GeneralManager.Instance.stageSelectManager.curSelectStage][i]);
                planetName.text = stringBuilder.ToString();
                yield return new WaitForSeconds(0.05f);
            }
        }
        else if (GameManager.Language == 1)
        {
            for (var i = 0; i < NameList_KOR[GeneralManager.Instance.stageSelectManager.curSelectStage].Length; i++)
            {
                stringBuilder.Append(NameList_KOR[GeneralManager.Instance.stageSelectManager.curSelectStage][i]);
                planetName.text = stringBuilder.ToString();
                yield return new WaitForSeconds(0.05f);
            }
        }
        
        StartCoroutine(StoryCoroutine());
    }
    
    //  NameCoroutine 다음에 연달아 실행
    private IEnumerator StoryCoroutine()
    {
        planetStory.SetText("");

        string unknownStory_ENG = "Exception: Information is NULL.";
        string unknownStory_KOR = "예외 : 정보가 NULL입니다.";
        
        StringBuilder stringBuilder = new StringBuilder();

        if (GameManager.Language == 0)
        {
            if (DataManager.CurStage >= GeneralManager.Instance.stageSelectManager.curSelectStage + 1)
            {
                for (var i = 0; i < StoryList_ENG[GeneralManager.Instance.stageSelectManager.curSelectStage].Length; i++)
                {
                    stringBuilder.Append(StoryList_ENG[GeneralManager.Instance.stageSelectManager.curSelectStage][i]);
                    planetStory.text = stringBuilder.ToString();
                    yield return new WaitForSeconds(0.005f);
                }
            }
            else
            {
                for (var i = 0; i < unknownStory_ENG.Length; i++)
                {
                    stringBuilder.Append(unknownStory_ENG[i]);
                    planetStory.text = stringBuilder.ToString();
                    yield return new WaitForSeconds(0.005f);
                }
            }
            
        }
        else if (GameManager.Language == 1)
        {
            if (DataManager.CurStage >= GeneralManager.Instance.stageSelectManager.curSelectStage + 1)
            {
                for (var i = 0;
                     i < StoryList_KOR[GeneralManager.Instance.stageSelectManager.curSelectStage].Length;
                     i++)
                {
                    stringBuilder.Append(StoryList_KOR[GeneralManager.Instance.stageSelectManager.curSelectStage][i]);
                    planetStory.text = stringBuilder.ToString();
                    yield return new WaitForSeconds(0.005f);
                }
            }
            else
            {
                for (var i = 0; i < unknownStory_KOR.Length; i++)
                {
                    stringBuilder.Append(unknownStory_KOR[i]);
                    planetStory.text = stringBuilder.ToString();
                    yield return new WaitForSeconds(0.005f);
                }
            }
        }
        
        StartCoroutine(InfoCoroutine());
    }
    //  마찬가지로 연달아 실행
    private IEnumerator InfoCoroutine()
    {
        planetInfo.SetText("");
        
        StringBuilder stringBuilder = new StringBuilder();

        if (GameManager.Language == 0)
        {
            if (DataManager.CurStage >= GeneralManager.Instance.stageSelectManager.curSelectStage + 1)
            {
                for (int i = 0; i < InfoList_ENG[GeneralManager.Instance.stageSelectManager.curSelectStage].Length; i++)
                {
                    stringBuilder.Append(InfoList_ENG[GeneralManager.Instance.stageSelectManager.curSelectStage][i]);
                    planetInfo.text = stringBuilder.ToString();
                    yield return new WaitForSeconds(0.005f);
                }
            }
        }
        else if (GameManager.Language == 1)
        {
            if (DataManager.CurStage >= GeneralManager.Instance.stageSelectManager.curSelectStage + 1)
            {
                for (int i = 0; i < InfoList_KOR[GeneralManager.Instance.stageSelectManager.curSelectStage].Length; i++)
                {
                    stringBuilder.Append(InfoList_KOR[GeneralManager.Instance.stageSelectManager.curSelectStage][i]);
                    planetInfo.text = stringBuilder.ToString();
                    yield return new WaitForSeconds(0.005f);
                }
            }
        }
        
    }

    //  정보 저장을 위한 Method
    public static void SetPlanet()
    {
        NameList_ENG.Add("HJD-1029X2");
        NameList_ENG.Add("AJH-4001D2");
        NameList_ENG.Add("JHS-8854xD");
        NameList_ENG.Add("Ending");
        
        StoryList_ENG.Add("This planet will be our first destination. It has similar environment with the Earth" +
                      ", but has strong enemy forces defending it.\n\n" +
                      "  Our vanguard tried their best to conquer, " +
                      "but now we can't find traces of them anymore. " +
                      "Let's colonize this Planet.\nTake care and Focus Developer.....\nGood Luck.....");
        
        StoryList_ENG.Add("Congrats Clearing HJD-1029X2!!! This is our second destination. "+
                      "It has similar temperature with the moon. "+
                      "VERY hot during the daytime and VERY cold after the sunset. "
                      +"It snowed there last night. Watch out!!! Pretty Slippery out there.. "
                      +"Get Some thick clothes on and take care! We trust you... ");
        
        StoryList_ENG.Add("Finally....this is our final destination. "
                      + "We don't have much information about JHS-8854xD to tell you. "
                      + "We just checked that there are some flaming hot lava lakes. "
                      +"You better not fall into it." +"Oh, we just got a new information. "
                      +"The control-unit is in the middle of the map. So the monsters will" +
                      "come from every direction... Good Luck... ");
        
        StoryList_ENG.Add("");
        
        InfoList_ENG.Add("Average temperature: 15.6\u00b0C\nPlanet diameter: 12,564 km\nBiological Population: 145,235,520\nPlanet type: Earth-type planet");
        InfoList_ENG.Add("Average temperature: 0.4\u00b0C\nPlanet diameter: 3,515 km\nBiological Population: 5,558,421\nPlanet type: Moon-type Planet");
        InfoList_ENG.Add("Average temperature: 64.7\u00b0C\nPlanet diameter: 9,564 km\nBiological Population: 34,512\nPlanet type: Lava Planet");
        InfoList_ENG.Add("");
        
        // KOREAN

        NameList_KOR.Add("HJD-1029X2");
        NameList_KOR.Add("AJH-4001D2");
        NameList_KOR.Add("JHS-8854xD");
        NameList_KOR.Add("엔딩");

        StoryList_KOR.Add("이 행성은 우리의 첫 번째 목적지입니다.\n지구와 유사한 환경을 가지고 있지만,\n" +
                          "강력한 적군이 이 행성을 방어하고 있습니다.\n" +
                          "우리의 선발대가 최선을 다해 이 행성을\n정복하려 했지만,\n" +
                          "현재는 그들의 흔적을 더 이상 찾을 수 없습니다.\n" +
                          "이 행성을 식민지화합시다.\n조심하고 집중하세요, 개발자님...\n행운을 빕니다.....");

        StoryList_KOR.Add("HJD-1029X2를 클리어한 것을 축하드립니다!!!\n여기가 우리의 두 번째 목적지입니다.\n" +
                          "이곳은 달과 비슷한 온도를 가지고 있습니다.\n" +
                          "낮에는 매우 덥고, 해가 지고 나면 매우 춥습니다.\n" +
                          "어젯밤에는 눈이 내렸습니다.\n조심하세요! 꽤 미끄럽습니다...\n" +
                          "두꺼운 옷을 준비하시고 조심히 다녀오세요! \n우리는 당신을 믿습니다...\n");

        StoryList_KOR.Add("드디어... 여기가 우리의 마지막 목적지입니다.\n" +
                          "JHS-8854xD에 대해 알려드릴 정보가 많지 않습니다.\n" +
                          "단지, 뜨거운 용암 호수가 있다는 사실만 확인되었습니다. " +
                          "그곳에 빠지지 않도록 조심하세요.\n" +
                          "아, 방금 새로운 정보를 얻었습니다.\n" +
                          "제어 유닛이 지도의 중앙에 위치해 있습니다.\n따라서 몬스터들이 " +
                          "모든 방향에서 몰려올 것입니다...\n행운을 빕니다...");

        StoryList_KOR.Add("게임 엔딩입니다.");

        InfoList_KOR.Add("평균 온도: 15.6\u00b0C\n행성 직경: 12,564 km\n생물학적 인구: 145,235,520\n행성 유형: 지구형 행성");
        InfoList_KOR.Add("평균 온도: 0.4\u00b0C\n행성 직경: 3,515 km\n생물학적 인구: 5,558,421\n행성 유형: 달형 행성");
        InfoList_KOR.Add("평균 온도: 64.7\u00b0C\n행성 직경: 9,564 km\n생물학적 인구: 34,512\n행성 유형: 용암 행성");
        InfoList_KOR.Add("");

    }
    
    public static string GetCurStageName()
    {
        return NameList_ENG[SceneController.Instance.curSelectStage];
    }
    
    //  웨이브 저장을 위한 Method
    public static void SetStageInfo()
    {
        StageInfo.Add(5);
        StageInfo.Add(5);
        StageInfo.Add(5);
        
    }

    public static void SetWaveInfo()
    {
        //  Stage 1 - Wave 9개 (임시 3개)
        WaveInfo.Add(new List<int> {150,200,250,350,0,0,0,0,0});
        // WaveInfo.Add(new List<int> {10,10,10,1000,0,0,0,0,0});
        WaveInfo.Add(new List<int> {200,250,300,450,0,0,0,0,0,0});
        WaveInfo.Add(new List<int> {250,300,350,550,0,0,0,0,0,0});
    }
    
    public static int GetStageInfo()
    {
        return StageInfo[SceneController.Instance.curSelectStage];
    }

    public static int GetWaveInfo(int curWave)
    {
        return WaveInfo[SceneController.Instance.curSelectStage][curWave - 1];
    }

    public static int GetReward()
    {
        return StageRewards[SceneController.Instance.curSelectStage];
    }
}
