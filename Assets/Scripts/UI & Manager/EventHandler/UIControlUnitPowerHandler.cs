// using System.Collections;
// using System.Collections.Generic;
// using TMPro;
// using UnityEngine;
// using UnityEngine.EventSystems;
//
// public class UIControlUnitPowerHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
// {
//     public GameObject upgradeInfoWrapper;
//     public GameObject upgradeInfoBox;
//     public TMP_Text infoTitle;
//     public TMP_Text infoContent;
//     public TMP_Text infoNext;
//     public TMP_Text infoMaxLevel;
//     public TMP_Text infoCost;
//     
//     private Canvas canvas;
//     private Vector2 canvasSize;
//     
//     private bool isHover;
//
//     private void Start()
//     {
//         upgradeInfoWrapper.SetActive(false);
//         canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
//         canvasSize = canvas.GetComponentInParent<RectTransform>().sizeDelta;
//
//         isHover = false;
//     }
//
//     public void OnPointerEnter(PointerEventData eventData)
//     {
//         infoMaxLevel.SetText("");
//         infoCost.SetText("");
//         infoNext.SetText("");
//         
//         if (!isHover)
//         {
//             isHover = true;
//             upgradeInfoWrapper.SetActive(true);
//             
//             if (GameManager.Language == 0)
//             {
//                 infoTitle.SetText("Control Unit Power");
//                 infoContent.SetText("You can increase the power\nof the Control Unit.");
//             }
//             else if (GameManager.Language == 1)
//             {
//                 infoTitle.SetText("Control Unit Power");
//                 infoContent.SetText("컨트롤 유닛의 최대 전력량을\n증가시킬 수 있습니다.");
//             }
//             
//             infoMaxLevel.SetText("");
//             infoCost.SetText("");
//             infoNext.SetText("");
//
//             //  MAX
//             if (DataManager.ControlUnitPowerLv == DataManager.LEVEL_MAX)
//             {
//                 infoMaxLevel.SetText("MAX LEVEL <" + DataManager.ControlUnitPower + ">");
//             }
//             else
//             {
//                 infoNext.SetText(DataManager.ControlUnitPower + " -> " + (DataManager.ControlUnitPower + DataManager.GetMargin(5)) + "(+ "+DataManager.GetMargin(5)+")");
//                 infoCost.SetText("Cost : "+ DataManager.GetCost(5));
//             }
//
//             RectTransform rectTransform = upgradeInfoBox.GetComponent<RectTransform>();
//
// // RectTransform의 World Space 크기 계산
//             Vector3[] worldCorners = new Vector3[4];
//             rectTransform.GetWorldCorners(worldCorners);
//
//             float width = worldCorners[2].x - worldCorners[0].x; // 우측 상단 - 좌측 하단 (World Space 기준 너비)
//             float height = worldCorners[2].y - worldCorners[0].y; // 우측 상단 - 좌측 하단 (World Space 기준 높이)
//
// // 위치 설정
//             upgradeInfoWrapper.GetComponent<RectTransform>().position = eventData.position + 
//                                                                         new Vector2(-width/2 - 1, height/2 +1);
//         }
//         
//     }
//     
//     public void OnPointerExit(PointerEventData eventData)
//     {
//         if (isHover)
//         {
//             isHover = false;
//             upgradeInfoWrapper.SetActive(false);
//         }
//     }
//     
//     public void OnPointerMove(PointerEventData eventData)
//     {
//         if (isHover)
//         {
//             RectTransform rectTransform = upgradeInfoBox.GetComponent<RectTransform>();
//
// // RectTransform의 World Space 크기 계산
//             Vector3[] worldCorners = new Vector3[4];
//             rectTransform.GetWorldCorners(worldCorners);
//
//             float width = worldCorners[2].x - worldCorners[0].x; // 우측 상단 - 좌측 하단 (World Space 기준 너비)
//             float height = worldCorners[2].y - worldCorners[0].y; // 우측 상단 - 좌측 하단 (World Space 기준 높이)
//
// // 위치 설정
//             upgradeInfoWrapper.GetComponent<RectTransform>().position = eventData.position + 
//                                                                         new Vector2(-width/2 - 1, height/2 +1);
//         }
//     }
// }
//
