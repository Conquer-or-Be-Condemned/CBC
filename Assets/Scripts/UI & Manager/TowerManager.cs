using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using TMPro.Examples;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

/*
 *  타워 메뉴와 타워 전체를 관리하는 스크립트입니다.
 *  인 게임 내에서 UI의 핵심적인 기능들을 사용할 수 있게 해주니,
 *  코드 수정에 각별히 조심해야 합니다.
 */

public class TowerManager : MonoBehaviour
{
    public GameObject towerMenu;
    public TMP_Text towerInfo;
    
    //  혹시 모를 관리의 용이성을 위해 배열로 하지 않고 List로 구현
    [SerializeField] private List<GameObject> towerList = new List<GameObject>();
    [SerializeField] private string towerTag = "Tower";
    [SerializeField] private int totalTowers;
    [SerializeField] private int activeTowers;
    
    //  CU
    [SerializeField] private ControlUnitStatus controlUnit;
    
    //  AlertManager
    [SerializeField] private AlertManager alertManager;
    
    //  Tower Menu 요소들
    [SerializeField] private float defaultFontSize;
    [SerializeField] private float missileFontSize;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private GameObject activateButton;
    [SerializeField] private TMP_Text activateText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text powerText;
    [SerializeField] private TMP_Text damageText;
    [SerializeField] private TMP_Text rpm;

    //  클릭과 관련된 인스턴스
    private RaycastHit2D hit;
    private Animator _animator;
    private bool isVisible;
    private DefaultCanonTurret curCanonTower;
    private DefaultMissileTurret curMissileTower;
    private Transform curTower;
    
    //  게임 시작 시 초기화
    private void InGame()
    {
        //  리스트 초기화 및 타워 관련 정보 초기화
        towerList.Clear();
        GameObject[] towerObjects = GameObject.FindGameObjectsWithTag(towerTag);
        towerList.AddRange(towerObjects);
        totalTowers = towerList.Count;
        activeTowers = 0;
    
        //  Tower Menu Animation 관련
        _animator = towerMenu.GetComponent<Animator>();
        isVisible = false;
        curCanonTower = null;
        curTower = null;

        //  타워 메뉴의 Activate 버튼 연결
        activateButton.GetComponent<Button>().onClick.AddListener(SetTowerActive);
        
        //  Cursor 변경
        GameManager.Instance.GetComponent<CursorManager>().SetInGameCursor();
        
        //  자동 찾기 목록
        if (controlUnit == null)
        {
            controlUnit = GameObject.Find("ControlUnit").GetComponent<ControlUnitStatus>();
        }
        if (alertManager == null)
        {
            alertManager = GameObject.Find("AlertManager").GetComponent<AlertManager>();
        }
        if (towerMenu == null)
        {
            towerMenu = GameObject.FindGameObjectWithTag("TowerMenu");
            
            if (towerMenu == null)
            {
                Debug.LogError("Tower Menu가 존재하지 않습니다. MenuBox를 연결해주세요.");
            }
        }

        //  게임 시작
        GameManager.InGame = true;
    }
    
    private void Awake()
    {
        InGame();
        
        //  초기화 완료
        GameManager.InGameInit = false;

        //  메뉴 내의 폰트 사이즈    ->  현재는 의미 없음
        defaultFontSize = 14f;
        missileFontSize = 14f;
        
        //  메뉴의 기본 위치
        menuOriginalPos = towerMenu.GetComponent<RectTransform>().anchoredPosition;
    }

    private void FixedUpdate()
    {
         // 타워 관련 정보 수집
        if (GameManager.InGame)
        {
            FindActiveTower();
            // SetUITowerInfo();
        }
    }

    private void Update()
    {
        //  마우스 클릭 감지
        if (!GameManager.InGameInit)
        {
            ClickProcess();
        }
    }

    //  현재는 사용하지 않는 코드
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

    //  활성화 된 타워의 수를 확인할 수 있음(디버깅 용)
    private void SetUITowerInfo()
    {
        towerInfo.SetText("Active Tower : " + activeTowers+" / "+ totalTowers);
    }
    
    //  마우스 클릭 시 타워를 클릭했는지 확인
    private void ClickProcess()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            hit = Physics2D.Raycast(mousePoint, Vector2.zero);
            
            //  UI 클릭 감지을 위한 변수
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };
            
            //  Ui 클릭 시
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("UI is Clicked.");
            }
            
            //  DefaultCanonTower을 클릭 시
            else if (hit.collider!=null && 
                hit.collider.GetComponentInParent<DefaultCanonTurret>() != null)
            {
                //  Missile 타워 해제 (반드시)
                curMissileTower = null;
                
                //  동일 오브젝트를 다시 클릭했다면 무시
                if (curCanonTower == hit.collider.GetComponentInParent<DefaultCanonTurret>())
                {
                    return;
                }
                
                curCanonTower = hit.collider.GetComponentInParent<DefaultCanonTurret>();

                //  이미 UI가 보여지고 있다면, 메뉴에 햅틱
                if (isVisible)
                {
                    StartCoroutine(ShakeMenuCoroutine());
                    SetCanonTowerInfo();
                }
                else
                {
                    Debug.Log("Tower is detected.");
                    SetCanonTowerInfo();
                    isVisible = true;
                }
            }
            
            //  DefaultMissleTower을 클릭 시
            else if (hit.collider != null &&
                     hit.collider.GetComponentInParent<DefaultMissileTurret>() != null)
            {
                //  Canon Tower 해제 (반드시)
                curCanonTower = null;
                
                //  동일 오브젝트를 다시 클릭했다면 무시
                if (curMissileTower == hit.collider.GetComponentInParent<DefaultMissileTurret>())
                {
                    return;
                }
                
                curMissileTower = hit.collider.GetComponentInParent<DefaultMissileTurret>();

                //  이미 UI가 보여지고 있다면, 메뉴에 햅틱
                if (isVisible)
                {
                    StartCoroutine(ShakeMenuCoroutine());
                    SetMissileTowerInfo();
                }
                else
                {
                    Debug.Log("Tower is detected.");
                    SetMissileTowerInfo();
                    isVisible = true;
                }
            }
            //  타워와 UI가 클릭되지 않은 경우(메뉴를 집어 넣음)
            else
            {
                Debug.Log("Tower is not detected.");
                isVisible = false;
                curCanonTower = null;
                curMissileTower = null;
            }
            
            SetVisible();
        }
    }

    //  isVisible을 확인하고 애니메이션 상태 변경
    private void SetVisible()
    {
        _animator.SetBool("isVisible", isVisible);
    }

    //  메뉴에 클릭한 CanonTower 정보를 Display
    private void SetCanonTowerInfo()
    {
        //  디버깅 용
        if (curCanonTower == null)
        {
            Debug.LogError("Tower is Null");
            return;
        }

        nameText.fontSize = defaultFontSize;
        
        nameText.SetText(curCanonTower.GetName());
        levelText.SetText("Lv " + curCanonTower.GetLevel());
        powerText.SetText("Power : " + curCanonTower.GetPower());
        damageText.SetText("Damage : "+curCanonTower.GetDamage());
        rpm.SetText("RPM : "+curCanonTower.GetRpm());
        
        if (curCanonTower.isActivated)
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
    
    //  메뉴에 클릭한 MissileTower 정보를 Display
    private void SetMissileTowerInfo()
    {
        if (curMissileTower == null)
        {
            Debug.LogError("Tower is Null");
            return;
        }

        nameText.fontSize = missileFontSize;
        
        nameText.SetText(curMissileTower.GetName());
        levelText.SetText("Lv " + curMissileTower.GetLevel());
        powerText.SetText("Power : " + curMissileTower.GetPower());
        damageText.SetText("Damage : "+curMissileTower.GetDamage());
        rpm.SetText("RPM : "+curMissileTower.GetRPM());

        if (curMissileTower.isActivated)
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
    
    //  타워의 활성화 상태를 변경할 수 있음 또한 미니맵에 표시되는 색상을 변경
    public void SetTowerActive()
    {
        if (curCanonTower != null)
        {
            //  이미 활성화 상태
            if (curCanonTower.isActivated)
            {
                curCanonTower.DeactivateTurret();

                GameObject[] childs = GameObject.FindGameObjectsWithTag("MapElement");

                foreach (var child in childs)
                {
                    if (child.transform.IsChildOf(curCanonTower.transform))
                    {
                        child.GetComponent<SpriteRenderer>().color = Color.yellow;
                        SetCanonTowerInfo();
                        return;
                    }
                }
            }
            //  비활성화 상태
            else
            {
                if (controlUnit.CheckEnoughPower(curCanonTower.GetPower()))
                {
                    curCanonTower.ActivateTurret();
                    GameObject[] childs = GameObject.FindGameObjectsWithTag("MapElement");

                    foreach (var child in childs)
                    {
                        if (child.transform.IsChildOf(curCanonTower.transform))
                        {
                            child.GetComponent<SpriteRenderer>().color = Color.green;
                            SetCanonTowerInfo();
                            return;
                        }
                    }
                }
                else
                {
                    alertManager.Show(1);
                    Debug.Log("Power가 부족합니다.");
                }
            }
        }
        else if (curMissileTower != null)
        {
            //  이미 활성화 상태
            if (curMissileTower.isActivated)
            {
                curMissileTower.DeactivateTurret();

                GameObject[] childs = GameObject.FindGameObjectsWithTag("MapElement");

                foreach (var child in childs)
                {
                    if (child.transform.IsChildOf(curMissileTower.transform))
                    {
                        child.GetComponent<SpriteRenderer>().color = Color.yellow;
                        SetMissileTowerInfo();
                        return;
                    }
                }
            }
            //  비활성화 상태
            else
            {
                if (controlUnit.CheckEnoughPower(curMissileTower.GetPower()))
                {
                    curMissileTower.ActivateTurret();
            
                    GameObject[] childs = GameObject.FindGameObjectsWithTag("MapElement");

                    foreach (var child in childs)
                    {
                        if (child.transform.IsChildOf(curMissileTower.transform))
                        {
                            child.GetComponent<SpriteRenderer>().color = Color.green;
                            SetMissileTowerInfo();
                            return;
                        }
                    }
                }
                else
                {
                    alertManager.Show(1);
                    Debug.Log("Power가 부족합니다.");
                }
            }
        }
        //  디버깅 용
        else
        {
            Debug.LogError("Tower is null");
            return;
        }
    }
    
    //  Menu 흔들림 효과
    public float shakeTime = 0.2f;
    public float shakeAmount = 1.03f;
    private Vector3 menuOriginalPos;
    private float duration;
    
    private void ShakeMenu()
    {
        float offsetX = Random.Range(-1f, 1f) * shakeAmount;
        float offsetY = Random.Range(-1f, 1f) * shakeAmount;
        towerMenu.GetComponent<RectTransform>().anchoredPosition = menuOriginalPos 
                                                                   + new Vector3(offsetX, offsetY, 0);
    }

    private IEnumerator ShakeMenuCoroutine()
    {
        float elapsed = 0f;
        duration = 0.01f;
        
        while(true)
        {
            if (elapsed >= shakeTime)
            {
                yield break;
            }

            elapsed += duration;
            
            ShakeMenu();
            yield return new WaitForSeconds(duration);
        }
    }
}
