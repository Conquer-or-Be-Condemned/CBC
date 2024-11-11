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

public class TowerManager : MonoBehaviour
{
    public GameObject towerMenu;
    public TMP_Text towerInfo;
    
    //  혹시 모를 관리의 용이성을 위해 배열로 하지 않고 List로 구현
    [SerializeField] private List<GameObject> towerList;
    [SerializeField] private String towerTag = "Tower";
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

    private RaycastHit2D hit;
    private Animator _animator;
    private bool isVisible;
    private DefaultCanonTurret curCanonTower;
    private DefaultMissileTurret curMissileTower;
    private Transform curTower;
    

    private void InGame()
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
        
        //  Menu 요소 찾기
        // nameText = GameObject.Find("TowerName").GetComponent<TMP_Text>();
        // activateButton = GameObject.Find("ActivationButton");
        // activateText = GameObject.Find("TowerInfoAct").GetComponent<TMP_Text>();
        // levelText = GameObject.Find("TowerInfoLv").GetComponent<TMP_Text>();
        // powerText = GameObject.Find("TowerInfoPw").GetComponent<TMP_Text>();
        // damageText = GameObject.Find("TowerInfoDmg").GetComponent<TMP_Text>();
        // rpm = GameObject.Find("TowerInfoRpm").GetComponent<TMP_Text>();
    
        //  Tower Menu Animation 관련
        _animator = towerMenu.GetComponent<Animator>();
        isVisible = false;
        curCanonTower = null;
        curTower = null;

        activateButton.GetComponent<Button>().onClick.AddListener(SetTowerActive);
        
        //  Cursor
        GameManager.GetInstance().GetComponent<CursorManager>().SetInGameCursor();
        
        //  ControlUnit
        if (controlUnit == null)
        {
            controlUnit = GameObject.Find("ControlUnit").GetComponent<ControlUnitStatus>();
        }

        //  AlertManager
        if (alertManager == null)
        {
            alertManager = GameObject.Find("AlertManager").GetComponent<AlertManager>();
        }

        GameManager.InGame = true;
    }

    private void Awake()
    {
        InGame();
        GameManager.InGameInit = false;

        defaultFontSize = 14f;
        missileFontSize = 14f;

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
        //  마우스 클릭
        if (!GameManager.InGameInit)
        {
            ClickProcess();
        }
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
            
            //  Ui 클릭
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("UI is Clicked.");
            }
            
            //  DefaultCanonTower
            else if (hit.collider!=null && 
                hit.collider.GetComponentInParent<DefaultCanonTurret>() != null)
            {
                //  Missile 타워 해제
                curMissileTower = null;
                
                //  같은 애를 다시 클릭했다면 무시
                if (curCanonTower == hit.collider.GetComponentInParent<DefaultCanonTurret>())
                {
                    return;
                }
                
                curCanonTower = hit.collider.GetComponentInParent<DefaultCanonTurret>();
                // curTower = hit.collider.GetComponentInParent<Transform>();

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
            
            //  DefaultMissleTower
            else if (hit.collider != null &&
                     hit.collider.GetComponentInParent<DefaultMissileTurret>() != null)
            {
                //  Canon Tower 해제
                curCanonTower = null;
                
                //  같은 애를 다시 클릭했다면 무시
                if (curMissileTower == hit.collider.GetComponentInParent<DefaultMissileTurret>())
                {
                    return;
                }
                
                curMissileTower = hit.collider.GetComponentInParent<DefaultMissileTurret>();

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

    private void SetVisible()
    {
        _animator.SetBool("isVisible", isVisible);
    }

    private void SetCanonTowerInfo()
    {
        if (curCanonTower == null)
        {
            Debug.LogError("Tower is Null");
            return;
        }

        nameText.fontSize = defaultFontSize;
        
        nameText.SetText(curCanonTower.GetName());
        levelText.SetText("Lv " + curCanonTower.GetLevel());
        powerText.SetText("Power : " + curCanonTower.GetPower());

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
    

    public void SetTowerActive()
    {
        if (curCanonTower != null)
        {
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
