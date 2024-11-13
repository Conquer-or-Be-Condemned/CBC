using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

/*
 *  Scene을 String으로 인해 변하게 할 수 있게 하는 스크립트입니다.
 *  따로 Object를 만들지 않고 사용할 수 있도록, 모든 methods를 static으로 선언합니다.
 */
public class SceneController : MonoBehaviour
{
    #region SINGLETON
    private static SceneController _instance;

    public static SceneController GetInstance()
    {
        if (_instance == null)
        {
            _instance = FindObjectOfType<SceneController>();
            if (_instance != null) return _instance;

            _instance = new SceneController().AddComponent<SceneController>();
            _instance.name = "SceneController";
        }

        return _instance;
    }
    
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(this);
        }
    }
    #endregion
    
    
    //  딜레이가 필요한 경우를 위한 변수
    private float delay = 3f;

    //  현재 씬 정보
    public static String NowScene;
    //  로딩 씬 같은 경우 다음 씬을 알아야 이동할 수 있음
    public static String NextScene;
    public static bool StageInit;
    
    //  For Map Select
    public Button warpButton;
    private StageInfoManager stageMenu;
    
    //  스테이지 정보
    private String[] stageList = { "MapTest" };

    public void Start()
    {
        //  Main 씬이 담기게 됨.
        NowScene = SceneManager.GetActiveScene().name;
        StageInit = false;
        AudioManager.Instance.PlayBGM(AudioManager.Bgm.StartingScene,true);
    }

    public void FixedUpdate()
    {
        //  Stage 선택 창인지를 지속적으로 확인
        //  이는 버튼을 가져오기 위함
        if (NowScene == "StageMenu" && StageInit)
        {
            Debug.Log("Boom!");
            warpButton = GameObject.Find("ToGame").GetComponent<Button>();

            stageMenu = GameObject.Find("PlanetInfo").GetComponent<StageInfoManager>();

            if (warpButton == null)
            {
                Debug.LogError("버튼이 없습니다.");
            }

            if (stageMenu == null)
            {
                Debug.LogError("스테이지 애니메이션 오류 발생");
            }
            
            //  버튼과 게임 시작 함수를 연동
            warpButton.onClick.AddListener(GoToGame);
            StageInit = false;
        }
    }

    //  게임 시작만을 위한 메소드
    public void GoToGame()
    {
        StartCoroutine(PreGoToGameCoroutine());
    }

    //  게임 시작 전 로딩 창을 임의로 불러옴(Space 키를 사용하지 않게 하기 위함)
    private IEnumerator PreGoToGameCoroutine()
    {
        stageMenu.SetStageMenuHide();
        yield return new WaitForSeconds(1.1f);
        
        ChangeScene("Loading");
        StartCoroutine(GoToGameCoroutine());
    }

    //  로딩 창이 시작되고 나서 스테이지를 바꿈
    private IEnumerator GoToGameCoroutine()
    {
        yield return new WaitForSeconds(2.8f);
        ChangeScene(stageList[GameManager.CurStage - 1]);
        GameManager.InGameInit = true;
        AudioManager.Instance.PlayBGM(AudioManager.Bgm.Stage1,true);
    }

    //  현재는 사용하지 않는 코드
    // public void SetStageInitTrue()
    // {
    //     StageInit = true;
    // }

    //  Scene을 이동하는 전역 함수
    public static void ChangeScene(string sceneName)
    {
        
        Debug.Log("Go to " + sceneName);
        SceneManager.LoadScene(sceneName);
        NowScene = sceneName;

        if (NowScene == "StageMenu")
        {
            StageInit = true;
        }
    }

    //  프로그램 종료
    public static void ExitProgram()
    {
        Application.Quit();
    }

    //  다음 씬 정보를 저장하는 함수
    public static void SetNextScene(String name)
    {
        NextScene = name;
    }

    //  다음 씬을 불러오는 함수
    public static void LoadNextScene()
    {
        ChangeScene(NextScene);
    }

    //  현재는 사용하지 않는 코드
    //  딜레이를 주는 함수만 Instance를 만들어야 실행이 가능합니다.
    public void ChangeSceneWithDelay(string sceneName)
    {
        StartCoroutine(ChangeSceneCoroutine(sceneName));
    }

    private IEnumerator ChangeSceneCoroutine(string sceneName)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }
}
