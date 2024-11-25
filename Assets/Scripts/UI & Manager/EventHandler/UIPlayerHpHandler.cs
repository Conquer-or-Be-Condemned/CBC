using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPlayerHpHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    public GameObject uiInfoWrapper;
    public TMP_Text uiInfo;

    private PlayerInfo player;
    private Canvas canvas;
    private Vector2 canvasSize;

    private bool isHover;
    private void Start()
    {
        //  UIInfoWrapper 및 UIInfo Raycast에서 제외
        uiInfoWrapper.GetComponent<Image>().raycastTarget = false;
        uiInfo.raycastTarget = false;
        
        uiInfoWrapper.SetActive(false);
        player = GameObject.Find("Player").GetComponent<PlayerInfo>();
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        canvasSize = canvas.GetComponentInParent<RectTransform>().sizeDelta;

        isHover = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHover = true;
        uiInfoWrapper.SetActive(true);
        uiInfo.SetText("HP : " + player.curHp + " / " + player.maxHp);
        uiInfoWrapper.GetComponent<RectTransform>().transform.position = eventData.position;
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        isHover = false;
        uiInfoWrapper.SetActive(false);
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (isHover)
        {
            uiInfoWrapper.GetComponent<RectTransform>().transform.position = eventData.position;
        }
    }
}
