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

    [Space]
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
            infoTitle.SetText(infoTitleList[(int)GameManager.SelectedLanguage][(int)shopBoxType]);
            infoContent.SetText(infoContentList[(int)GameManager.SelectedLanguage][(int)shopBoxType]);

            //  Checking Max Level
            if (DataManager.GetLevel(shopBoxType) == DataManager.LEVEL_MAX)
            {
                infoMaxLevel.SetText("MAX LEVEL <" + DataManager.GetLevel(shopBoxType) + ">");
            }
            else
            {
                infoNext.SetText(DataManager.GetAttributeData(shopBoxType) + " -> " +
                                 DataManager.GetAttributeData(shopBoxType) + DataManager.GetMargin(shopBoxType) +
                                 "(+ " + DataManager.GetMargin(shopBoxType) + ")");
                infoCost.SetText("Cost : " + DataManager.GetCost(shopBoxType));
            }


            RectTransform rectTransform = upgradeInfoBox.GetComponent<RectTransform>();

            // RectTransform의 World Space 크기 계산
            Vector3[] worldCorners = new Vector3[4];
            rectTransform.GetWorldCorners(worldCorners);

            float width = worldCorners[2].x - worldCorners[0].x; // 우측 상단 - 좌측 하단 (World Space 기준 너비)
            float height = worldCorners[2].y - worldCorners[0].y; // 우측 상단 - 좌측 하단 (World Space 기준 높이)

            // 위치 설정
            upgradeInfoWrapper.GetComponent<RectTransform>().position = eventData.position +
                                                                        new Vector2(-width / 2 - 1, height / 2 + 1);
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
                                                                        new Vector2(width/2, height/2);
        }
    }
}
