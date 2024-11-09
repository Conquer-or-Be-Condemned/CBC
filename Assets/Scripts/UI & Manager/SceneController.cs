using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    
    
    private float delay = 3f;

    //  씬 정보
    public static String NowScene;
    public static String NextScene;
    public static bool StageInit;
    
    //  For Map Select
    public Button warpButton;

    private String[] stageList = { "MapTest" };

    public void Start()
    {
        NowScene = SceneManager.GetActiveScene().name;
        StageInit = false;
    }

    public void FixedUpdate()
    {
        if (NowScene == "StageMenu" && StageInit)
        {
            Debug.Log("Boom!");
            warpButton = GameObject.Find("ToGame").GetComponent<Button>();

            if (warpButton == null)
            {
                Debug.Log("버튼이 없습니다.");
            }
            
            warpButton.onClick.AddListener(GoToGame);
            StageInit = false;
        }
    }

    public void GoToGame()
    {
        ChangeScene("Loading");
        StartCoroutine(GoToGameCoroutine());
    }

    private IEnumerator GoToGameCoroutine()
    {
        yield return new WaitForSeconds(2.8f);
        ChangeScene(stageList[GameManager.CurStage - 1]);
        GameManager.InGameInit = true;
    }

    public void SetStageInitTrue()
    {
        StageInit = true;
    }

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

    public static void ExitProgram()
    {
        Application.Quit();
    }

    public static void SetNextScene(String name)
    {
        NextScene = name;
    }

    public static void LoadNextScene()
    {
        ChangeScene(NextScene);
    }

    //  딜레이를 주는 함수만 Instance를 만들어야 실행이 가능합니다.
    public void ChangeSceneWithDelay(string sceneName)
    {
        StartCoroutine(ChangeSceneCoroutine(sceneName));
    }
    
    // public void ChangeSceneWithDelayAndNext(string sceneName, string nextName, float time)
    // {
    //     StartCoroutine(ChangeSceneCoroutine(sceneName,time));
    // }

    private IEnumerator ChangeSceneCoroutine(string sceneName)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }
}
