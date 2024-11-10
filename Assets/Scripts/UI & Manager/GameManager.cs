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
    public static int CurStage;
    public static bool InGame;
    public static bool InGameInit;
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

        CurStage = 1;
        InGame = false;
        InGameInit = false;
    }

    private void Update()
    {
        if (SceneController.NowScene == "Loading" && LoadingSkip)
        {
            CheckSpaceKey();    
        }

        if (InGame)
        {
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
