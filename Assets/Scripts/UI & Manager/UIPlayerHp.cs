using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*
 *  Player와 관련된 UI에 대한 스크립트입니다.
 *  HpBar의 Tag는 정해져있습니다.(HpBar)
 */
public class UIPlayerHp : MonoBehaviour
{
    [SerializeField] private GameObject [] cells;
    [SerializeField] private int maxCell;
    [SerializeField] private int curCell;
    
    [SerializeField] private GameObject player;
    
    private void Awake()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
            Debug.Log("UI - Player Object가 연결되었습니다.");
        }

        if (cells.Length == 0)
        {
            cells = GameObject.FindGameObjectsWithTag("HpBar");
        }
        
        //  플레이어와 UI 연결
        player.GetComponent<PlayerInfo>().onHpChange.AddListener(SetUIPlayerHp);
        
        maxCell = cells.Length;
    }
    

    //  Player의 체력이 변동되었을 때만 호출
    public void SetUIPlayerHp(int curHp, int maxHp)
    {
        float cellRatio = 1f - curHp / (float)maxHp;
        
        int cellNum = (int)(cells.Length*cellRatio);
        curCell = maxCell - cellNum;
        
        for (int i = 0; i < cellNum; i++)
        {
            cells[i].SetActive(false);
        }
        
        // Debug.Log(cellNum);
    }
}
