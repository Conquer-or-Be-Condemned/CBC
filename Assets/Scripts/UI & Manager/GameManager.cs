using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

/*
 *  게임 전체를 관할하는 GameManager입니다.
 *  오브젝트로써의 역할도 필요하지만 static으로 선언해야 하는 method도 많으니 참고바랍니다.
 */
public class GameManager : MonoBehaviour
{
    public GameObject player;
    //  현재 플레이 가능한 스테이지 정보
    public static int CurStage;
    //  인게임 상태인지 확인
    public static bool InGame;
    //  인게임에서 필요한 모든 초기화가 가능한지 확인(true : 초기화 안됨, false : 초기화 됨)
    public static bool InGameInit;
    //  로딩을 스킵할 수 있는지 확인
    public static bool LoadingSkip;

    #region SINGLETON
    
    private static GameManager _instance;

    public static GameManager GetInstance()
    {
        if (_instance == null)
        {
            _instance = FindObjectOfType<GameManager>();
            if (_instance != null) return _instance;
            else
            {
                Debug.LogError("GameManager가 존재하지 않습니다.");
            }
            // _instance = new GameManager().AddComponent<GameManager>();
            // _instance.name = "GameManager";
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
        //  GM이 존재하지만 this가 아닌 경우 -> this를 삭제
        else if (_instance != this)
        {
            Destroy(this);
        }
    }
    #endregion

    private void Start()
    {
        //  다음 씬에서도 동일하게 유지하기 위함
        DontDestroyOnLoad(this.gameObject);
        
        //  게임 시작 시 초기화 목록
        CurStage = 1;
        InGame = false;
        InGameInit = false;
    }

    private void Update()
    {
        //  지속해서 현재 씬이 Loading인지 확인 (또한 스킵이 가능한지 확인)
        if (SceneController.NowScene == "Loading" && LoadingSkip)
        {
            CheckSpaceKey();    
        }

        //  인게임인지 확인
        if (InGame)
        {
            //  플레이어 재검색
            if (player == null)
            {
                player = GameObject.FindGameObjectWithTag("Player");
                if (player == null)
                {
                    Debug.LogError("No player found");
                }
            }
        }
    }

    //  Loading 창에서 Space 입력 받기
    private void CheckSpaceKey()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Press!");
            if (SceneController.NowScene == "Loading")
            {
                SceneController.LoadNextScene();
            }
            else return;
        }
    }
    
}
