using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*
 *  Player와 관련된 UI에 대한 스크립트입니다.
 *  HpBar의 Tag는 정해져있습니다.(HpBar)
 */
public class UIPlayerHp : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject [] cells;
    [SerializeField] private int maxCell;
    [SerializeField] private int curCell;
    
    private void Start()
    {

        if (cells.Length == 0)
        {
            cells = GameObject.FindGameObjectsWithTag("HpBar");
        }
        
        maxCell = cells.Length;
    }

    private void FixedUpdate()
    {
        //  플레이어와 UI 연결 -> 시간 차 오류로 인해 Start에 넣지 않음
        if (GameManager.Instance.player == null)
        {
            GameManager.Instance.player.GetComponent<PlayerInfo>().onHpChange.AddListener(SetUIPlayerHp);
        }
    }


    //  Player의 체력이 변동되었을 때만 호출
    public void SetUIPlayerHp(int curHp, int maxHp)
    {
        float cellRatio = 1f - (curHp / (float)maxHp);
        
        int cellNum = (int)(cells.Length * cellRatio);
        curCell = maxCell - cellNum;

        for (int i = 0; i < maxCell; i++)
        {
            cells[i].SetActive(true);
        }

        for (int i = 0; i < cellNum; i++)
        {
            cells[i].SetActive(false);
        }
        
        // Debug.Log(cellNum);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        
    }
}


