using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UICUPowerHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
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
            uiInfo.SetText("Power : " + cuInfo.GetCurPower() + " / " + cuInfo.GetMaxPower());
            uiInfoWrapper.GetComponent<RectTransform>().transform.position = eventData.position;    
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
            uiInfoWrapper.GetComponent<RectTransform>().transform.position = eventData.position;
        }
    }
}
