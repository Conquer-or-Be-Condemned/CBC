using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TowerManager : MonoBehaviour
{
    public TMP_Text towerInfo;
    
    //  혹시 모를 관리의 용이성을 위해 배열로 하지 않고 List로 구현
    [SerializeField] private List<GameObject> towerList;
    [SerializeField] private String towerTag = "Tower";
    [SerializeField] private int totalTowers;
    [SerializeField] private int activeTowers;

    private RaycastHit2D hit;
    private void Start()
    {
        //  리스트 초기화
        towerList.Clear();

        GameObject[] towerObjects = GameObject.FindGameObjectsWithTag(towerTag);
        towerList.AddRange(towerObjects);
        
        totalTowers = towerList.Count;
        activeTowers = 0;
    }

    private void FixedUpdate()
    {
        //  기존 기능
        FindActiveTower();
        SetUITowerInfo();
    }

    private void FindActiveTower()
    {
        activeTowers = totalTowers;
        foreach (var e in towerList)
        {
            //  추후에 타워가 추가되면 판정 기준을 바꿔야 함.
            //  그리고 나중에 타워 코드랑 연계해서 타워 수가 바뀌면 Event 걸도록 하는 것도 괜찮을 듯
            if (!e.GetComponent<CamoTurretLV1>().isActivated)
            {
                activeTowers--;
            }
        }
    }

    private void SetUITowerInfo()
    {
        towerInfo.SetText("Active Tower : " + activeTowers+" / "+ totalTowers);
    }
    
    //  UI 마우스 클릭 시! - RayCast2D로 짬
    private void ClickProcess()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            hit = Physics2D.Raycast(mousePoint, Vector2.zero);

            // if (hit.collider.GetComponent<())
            // {
            // }
        }
    }

    private void CheckSelected()
    {
        
    }
}
