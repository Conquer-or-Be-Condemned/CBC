using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TowerManager : MonoBehaviour
{
    public GameObject towerMenu;
    public TMP_Text towerInfo;
    
    //  혹시 모를 관리의 용이성을 위해 배열로 하지 않고 List로 구현
    [SerializeField] private List<GameObject> towerList;
    [SerializeField] private String towerTag = "Tower";
    [SerializeField] private int totalTowers;
    [SerializeField] private int activeTowers;
    
    //  Tower Menu 요소들
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private GameObject activateButton;
    [SerializeField] private TMP_Text activateText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text powerText;
    [SerializeField] private TMP_Text damageText;
    [SerializeField] private TMP_Text rmp;

    private RaycastHit2D hit;
    private Animator _animator;
    private bool isVisible;
    private DefaultTurret curTower;
    
    private void Start()
    {
        //  리스트 초기화
        towerList.Clear();

        GameObject[] towerObjects = GameObject.FindGameObjectsWithTag(towerTag);
        towerList.AddRange(towerObjects);
        
        totalTowers = towerList.Count;
        activeTowers = 0;
        
        //  만일 TowerMenu가 비어있다면 -> Object : MenuBox, Tag : TowerMenu
        if (towerMenu == null)
        {
            towerMenu = GameObject.FindGameObjectWithTag("TowerMenu");
            
            if (towerMenu == null)
            {
                Debug.LogError("Tower Menu가 존재하지 않습니다. MenuBox를 연결해주세요.");
            }
        }
    
        //  Tower Menu Animation 관련
        _animator = towerMenu.GetComponent<Animator>();
        isVisible = false;
        curTower = null;

        activateButton.GetComponent<Button>().onClick.AddListener(SetTowerActive);
    }

    private void FixedUpdate()
    {
        //  타워 관련 정보 수집
        FindActiveTower();
        SetUITowerInfo();
    }

    private void Update()
    {
        //  마우스 클릭
        ClickProcess();
    }

    private void FindActiveTower()
    {
        activeTowers = totalTowers;
        foreach (var e in towerList)
        {
            //  추후에 타워가 추가되면 판정 기준을 바꿔야 함.
            //  그리고 나중에 타워 코드랑 연계해서 타워 수가 바뀌면 Event 걸도록 하는 것도 괜찮을 듯
            // if (!e.GetComponent<CamoTurretLV1>().isActivated)
            // {
            //     activeTowers--;
            // }
        }
    }

    private void SetUITowerInfo()
    {
        towerInfo.SetText("Active Tower : " + activeTowers+" / "+ totalTowers);
    }
    
    //  UI 마우스 클릭 시! - RayCast2D
    private void ClickProcess()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            hit = Physics2D.Raycast(mousePoint, Vector2.zero);
            
            //  UI 클릭 감지    ->  이해하기 어려울거임
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };
            if (EventSystem.current.IsPointerOverGameObject())
            {
                //  Ui 크
                Debug.Log("UI is Clicked.");
            }
            
            //  만일 부모 오브젝트에 DefaultTurret 스크립트가 존재한다면
            else if (hit.collider!=null && 
                hit.collider.GetComponentInParent<DefaultTurret>() != null)
            {
                curTower = hit.collider.GetComponentInParent<DefaultTurret>();
                Debug.Log("Tower is detected.");
                SetTowerInfo();
                isVisible = true;
            }
            else
            {
                Debug.Log("Tower is not detected.");
                isVisible = false;
            }
            
            SetVisible();
        }
    }

    private void SetVisible()
    {
        _animator.SetBool("isVisible", isVisible);
    }

    private void SetTowerInfo()
    {
        if (curTower == null)
        {
            Debug.LogError("Tower is Null");
            return;
        }
        nameText.SetText(curTower.GetName());
        levelText.SetText("Lv " + curTower.GetLevel());
        powerText.SetText("Power : " + curTower.GetPower());

        if (curTower.isActivated)
        {
            activateText.SetText("Activate");
            activateText.color = Color.green;
        }
        else
        {
            activateText.SetText("Deactivate");
            activateText.color = Color.red;
        }
        
        //  Damage와 RPM은 메소드가 준비되지 않음.
    }

    public void SetTowerActive()
    {
        if (curTower == null)
        {
            Debug.LogError("Tower is Null");
            return;
        }

        if (curTower.isActivated)
        {
            curTower.DeactivateTurret();
        }
        else
        {
            curTower.ActivateTurret();
        }
        
        SetTowerInfo();
    }
}
