using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIShopHoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    public GameObject upgradeInfoWrapper;
    public GameObject upgradeInfoBox;
    public TMP_Text infoTitle;
    public TMP_Text infoContent;
    public TMP_Text infoNext;
    public TMP_Text infoMaxLevel;
    public TMP_Text infoCost;
    
    private Canvas canvas;
    private Vector2 canvasSize;
    
    private bool isHover;

    [Header("Shop Type")] [SerializeField] private AttributeType shopBoxType;
    
    //  Literal Information (Language, Type 구분) -> String은 const 안 됨
    #region __LITERAL_INFORMATION__
    
    private static readonly string[][] infoTitleList =
    {
        //  Language English
        new []
        {
            "Player Health",
            "Player Bullet",
            "Turret Bullet",
            "Turret Missile",
            "Control Unit Power",
            "Control Unit Health"
        },
        //  Language Korean
        new []
        {
            "플레이어 체력",
            "플레이어 탄환 수",
            "캐논 터렛 공격력",
            "미사일 터렛 공격력",
            "제어장치 전력",
            "제어장치 체력"
        }
    };

    private static readonly string[][] infoContentList = 
    {
        //  Language English
        new []
        {
            "You can increase the maximum\nHp of the player.",
            "You can increase the number\nof bullets the player fires.",
            "You can increase the damage\nof the canon turret.",
            "You can increase the damage\nto the missile turret.",
            "You can increase the power\nof the Control Unit.",
            "You can increase the maximum\nHp of the Control Unit."
        },
        //  Language Korean
        new []
        {
            "플레이어의 최대 체력을\n증가시킬 수 있습니다.",
            "플레이어의 총알 발사 수를\n증가시킬 수 있습니다.",
            "캐논 터렛의 데미지를\n증가시킬 수 있습니다.",
            "미사일 터렛의 데미지를\n증가시킬 수 있습니다.",
            "컨트롤 유닛의 최대 전력량을\n증가시킬 수 있습니다.",
            "컨트롤 유닛의 최대 전력량을\n증가시킬 수 있습니다."
        }
    };
    
    #endregion
    

    private void Start()
    {
        upgradeInfoWrapper.SetActive(false);
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        canvasSize = canvas.GetComponentInParent<RectTransform>().sizeDelta;

        isHover = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        infoMaxLevel.SetText("");
        infoCost.SetText("");
        infoNext.SetText("");
        
        if (!isHover)
        {
            isHover = true;
            upgradeInfoWrapper.SetActive(true);
            
            //  Set UI Text
            infoTitle.SetText(infoTitleList[GameManager.Language][(int)shopBoxType]);
            infoContent.SetText(infoContentList[GameManager.Language][(int)shopBoxType]);

            //  Checking Max Level
            if (DataManager.GetLevel(shopBoxType) == DataManager.LEVEL_MAX)
            {
                infoMaxLevel.SetText("MAX LEVEL <" + DataManager.GetLevel(shopBoxType) + ">");
            }
            else
            {
                infoNext.SetText(DataManager.GetAttributeData(shopBoxType) + " -> " + DataManager.GetAttributeData(shopBoxType) + DataManager.GetMargin(shopBoxType) + "(+ "+DataManager.GetMargin(shopBoxType)+")");
                infoCost.SetText("Cost : "+ DataManager.GetCost(shopBoxType));
            }

            // switch (shopBoxType)
            // {
            //     case ShopBoxType.PlayerHealth:
            //         if (GameManager.Language == 0)
            //         {
            //             infoTitle.SetText("Player Health");
            //             infoContent.SetText("You can increase the maximum\nHp of the player.");
            //         }
            //         else if (GameManager.Language == 1)
            //         {
            //             infoTitle.SetText("Player Health");
            //             infoContent.SetText("플레이어의 최대 체력을\n증가시킬 수 있습니다.");
            //         }
            //
            //         infoMaxLevel.SetText("");
            //         infoCost.SetText("");
            //         infoNext.SetText("");
            //
            //         //  MAX
            //         if (DataManager.PlayerHpLv == DataManager.LEVEL_MAX)
            //         {
            //             infoMaxLevel.SetText("MAX LEVEL <" + DataManager.PlayerHp + ">");
            //         }
            //         else
            //         {
            //             infoNext.SetText(DataManager.PlayerHp + " -> " + (DataManager.PlayerHp + DataManager.GetMargin(0)) + "(+ "+DataManager.GetMargin(0)+")");
            //             infoCost.SetText("Cost : "+ DataManager.GetCost(0));
            //         }
            //         
            //         break;
            //     case ShopBoxType.PlayerBullet:
            //         if (GameManager.Language == 0)
            //         {
            //             infoTitle.SetText("Player Bullet");
            //             infoContent.SetText("You can increase the number\nof bullets the player fires.");
            //         }
            //         else if (GameManager.Language == 1)
            //         {
            //             infoTitle.SetText("Player Bullet");
            //             infoContent.SetText("플레이어의 총알 발사 수를\n증가시킬 수 있습니다.");
            //         }
            //
            //         infoMaxLevel.SetText("");
            //         infoCost.SetText("");
            //         infoNext.SetText("");
            //
            //         //  MAX
            //         if (DataManager.PlayerBulletLv == DataManager.LEVEL_MAX)
            //         {
            //             infoMaxLevel.SetText("MAX LEVEL <" + DataManager.PlayerBullet + ">");
            //         }
            //         else
            //         {
            //             infoNext.SetText(DataManager.PlayerBullet + " -> " + (DataManager.PlayerBullet + DataManager.GetMargin(1)) + "(+ "+DataManager.GetMargin(1)+")");
            //             infoCost.SetText("Cost : "+ DataManager.GetCost(1));
            //         }
            //         break;
            //     case ShopBoxType.TurretBullet:
            //         if (GameManager.Language == 0)
            //         {
            //             infoTitle.SetText("Turret Bullet");
            //             infoContent.SetText("You can increase the damage\nof the canon turret.");
            //         }
            //         else if (GameManager.Language == 1)
            //         {
            //             infoTitle.SetText("Turret Bullet");
            //             infoContent.SetText("캐논 터렛의 데미지를\n증가시킬 수 있습니다.");
            //         }
            //
            //         infoMaxLevel.SetText("");
            //         infoCost.SetText("");
            //         infoNext.SetText("");
            //
            //         //  MAX
            //         if (DataManager.TurretBulletLv == DataManager.LEVEL_MAX)
            //         {
            //             infoMaxLevel.SetText("MAX LEVEL <" + DataManager.TurretBullet + ">");
            //         }
            //         else
            //         {
            //             infoNext.SetText(DataManager.TurretBullet + " -> " + (DataManager.TurretBullet + DataManager.GetMargin(2)) + "(+ "+DataManager.GetMargin(2)+")");
            //             infoCost.SetText("Cost : "+ DataManager.GetCost(2));
            //         }
            //         break;
            //     case ShopBoxType.TurretMissile:
            //         if (GameManager.Language == 0)
            //         {
            //             infoTitle.SetText("Turret Missile");
            //             infoContent.SetText("You can increase the damage\nto the missile turret.");
            //         }
            //         else if (GameManager.Language == 1)
            //         {
            //             infoTitle.SetText("Turret Missile");
            //             infoContent.SetText("미사일 터렛의 데미지를\n증가시킬 수 있습니다.");
            //         }
            //
            //
            //         infoMaxLevel.SetText("");
            //         infoCost.SetText("");
            //         infoNext.SetText("");
            //
            //         //  MAX
            //         if (DataManager.TurretMissileLv == DataManager.LEVEL_MAX)
            //         {
            //             infoMaxLevel.SetText("MAX LEVEL <" + DataManager.TurretMissile + ">");
            //         }
            //         else
            //         {
            //             infoNext.SetText(DataManager.TurretMissile + " -> " + (DataManager.TurretMissile + DataManager.GetMargin(3)) + "(+ "+DataManager.GetMargin(3)+")");
            //             infoCost.SetText("Cost : "+ DataManager.GetCost(3));
            //         }
            //         break;
            //     case ShopBoxType.ControlUnitPower:
            //         if (GameManager.Language == 0)
            //         {
            //             infoTitle.SetText("Control Unit Power");
            //             infoContent.SetText("You can increase the power\nof the Control Unit.");
            //         }
            //         else if (GameManager.Language == 1)
            //         {
            //             infoTitle.SetText("Control Unit Power");
            //             infoContent.SetText("컨트롤 유닛의 최대 전력량을\n증가시킬 수 있습니다.");
            //         }
            //
            //         infoMaxLevel.SetText("");
            //         infoCost.SetText("");
            //         infoNext.SetText("");
            //
            //         //  MAX
            //         if (DataManager.ControlUnitPowerLv == DataManager.LEVEL_MAX)
            //         {
            //             infoMaxLevel.SetText("MAX LEVEL <" + DataManager.ControlUnitPower + ">");
            //         }
            //         else
            //         {
            //             infoNext.SetText(DataManager.ControlUnitPower + " -> " + (DataManager.ControlUnitPower + DataManager.GetMargin(5)) + "(+ "+DataManager.GetMargin(5)+")");
            //             infoCost.SetText("Cost : "+ DataManager.GetCost(5));
            //         }
            //         break;
            //     case ShopBoxType.ControlUnitHealth:
            //         if (GameManager.Language == 0)
            //         {
            //             infoTitle.SetText("Control Unit Health");
            //             infoContent.SetText("You can increase the maximum\nHp of the Control Unit.");
            //         }
            //         else if (GameManager.Language == 1)
            //         {
            //             infoTitle.SetText("Control Unit Health");
            //             infoContent.SetText("컨트롤 유닛의 최대 체력을\n증가시킬 수 있습니다.");
            //         }
            //
            //         infoMaxLevel.SetText("");
            //         infoCost.SetText("");
            //         infoNext.SetText("");
            //
            //         //  MAX
            //         if (DataManager.ControlUnitHpLv == DataManager.LEVEL_MAX)
            //         {
            //             infoMaxLevel.SetText("MAX LEVEL <" + DataManager.ControlUnitHp + ">");
            //         }
            //         else
            //         {
            //             infoNext.SetText(DataManager.ControlUnitHp + " -> " +
            //                              (DataManager.ControlUnitHp + DataManager.GetMargin(4)) + "(+ " +
            //                              DataManager.GetMargin(4) + ")");
            //             infoCost.SetText("Cost : " + DataManager.GetCost(4));
            //         }
            //         break;
            // }
            
            

            RectTransform rectTransform = upgradeInfoBox.GetComponent<RectTransform>();

            // RectTransform의 World Space 크기 계산
            Vector3[] worldCorners = new Vector3[4];
            rectTransform.GetWorldCorners(worldCorners);

            float width = worldCorners[2].x - worldCorners[0].x; // 우측 상단 - 좌측 하단 (World Space 기준 너비)
            float height = worldCorners[2].y - worldCorners[0].y; // 우측 상단 - 좌측 하단 (World Space 기준 높이)

            // 위치 설정
            upgradeInfoWrapper.GetComponent<RectTransform>().position = eventData.position + 
                                                                        new Vector2(-width/2 - 1, height/2 +1);
        }
        
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        if (isHover)
        {
            isHover = false;
            upgradeInfoWrapper.SetActive(false);
        }
    }
    
    public void OnPointerMove(PointerEventData eventData)
    {
        if (isHover)
        {
            RectTransform rectTransform = upgradeInfoBox.GetComponent<RectTransform>();

            // RectTransform의 World Space 크기 계산
            Vector3[] worldCorners = new Vector3[4];
            rectTransform.GetWorldCorners(worldCorners);

            float width = worldCorners[2].x - worldCorners[0].x; // 우측 상단 - 좌측 하단 (World Space 기준 너비)
            float height = worldCorners[2].y - worldCorners[0].y; // 우측 상단 - 좌측 하단 (World Space 기준 높이)

            // 위치 설정
            upgradeInfoWrapper.GetComponent<RectTransform>().position = eventData.position + 
                                                                        new Vector2(-width/2 - 1, height/2 +1);
        }
    }
}
