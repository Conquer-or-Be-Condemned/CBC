using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UICUHpHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    public GameObject uiInfoWrapper;
    public TMP_Text uiInfo;
    
    private ControlUnitStatus cuInfo;
    private Canvas canvas;
    private Vector2 canvasSize;
    
    private bool isHover;

    private void Start()
    {
        uiInfoWrapper.SetActive(false);
        cuInfo = GameObject.Find("ControlUnit").GetComponent<ControlUnitStatus>();
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        canvasSize = canvas.GetComponentInParent<RectTransform>().sizeDelta;

        isHover = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
        if (!isHover)
        {
            isHover = true;
            uiInfoWrapper.SetActive(true);
            uiInfo.SetText("HP : " + cuInfo.GetCurHp() + " / " + cuInfo.GetMaxHp());

            RectTransform rectTransform = uiInfoWrapper.GetComponent<RectTransform>();

// RectTransform의 World Space 크기 계산
            Vector3[] worldCorners = new Vector3[4];
            rectTransform.GetWorldCorners(worldCorners);

            float width = worldCorners[2].x - worldCorners[0].x; // 우측 상단 - 좌측 하단 (World Space 기준 너비)
            float height = worldCorners[2].y - worldCorners[0].y; // 우측 상단 - 좌측 하단 (World Space 기준 높이)

// 위치 설정
            uiInfoWrapper.GetComponent<RectTransform>().position = eventData.position + 
                                                                        new Vector2(-width/2 - 1, height/2 +1);
        }
        
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        if (isHover)
        {
            isHover = false;
            uiInfoWrapper.SetActive(false);
        }
    }
    
    public void OnPointerMove(PointerEventData eventData)
    {
        if (isHover)
        {
            RectTransform rectTransform = uiInfoWrapper.GetComponent<RectTransform>();

// RectTransform의 World Space 크기 계산
            Vector3[] worldCorners = new Vector3[4];
            rectTransform.GetWorldCorners(worldCorners);

            float width = worldCorners[2].x - worldCorners[0].x; // 우측 상단 - 좌측 하단 (World Space 기준 너비)
            float height = worldCorners[2].y - worldCorners[0].y; // 우측 상단 - 좌측 하단 (World Space 기준 높이)

// 위치 설정
            uiInfoWrapper.GetComponent<RectTransform>().position = eventData.position + 
                                                                   new Vector2(-width/2 - 1, height/2 +1);
        }
    }
}
