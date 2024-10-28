using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    public GameObject player;

    #region SINGLETON
    
    private static GameManager _instance;

    public static GameManager GetInstance()
    {
        if (_instance == null)
        {
            _instance = FindObjectOfType<GameManager>();
            if (_instance != null) return _instance;

            _instance = new GameManager().AddComponent<GameManager>();
            _instance.name = "GameManager";
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
        DontDestroyOnLoad(gameObject);
        
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
