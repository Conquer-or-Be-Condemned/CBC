using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*
 *  HpBar의 Tag는 정해져있습니다.(HpBar)
 */
public class UIPlayerHp : MonoBehaviour
{
    private GameObject player;
    private float maxWidth;
    private RectTransform hpRect;
    public TMP_Text hpText;

    private void Awake()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
            Debug.Log("UI - Player Object가 연결되었습니다.");
        }

        if (hpText == null)
        {
            hpText = GameObject.FindWithTag("HpText").GetComponent<TMP_Text>();
            if (hpText == null)
            {
                Debug.LogError("UI ERROR : Hp Text를 반드시 연결해주어야 합니다.");
                SceneController.ExitProgram();
            }
        }
        
        //  플레이어와 UI 연결
        player.GetComponent<PlayerInfo>().onHpChange.AddListener(SetUIPlayerHp);
        
        //  본래의 이미지 Width 저장
        hpRect = gameObject.GetComponent<RectTransform>();
        maxWidth = hpRect.sizeDelta.x;
    }
    

    public void SetUIPlayerHp(int curHp, int maxHp)
    {
        float ratio = curHp / (float)maxHp;
        hpRect.sizeDelta = new Vector2(maxWidth * ratio, hpRect.sizeDelta.y);
        hpText.SetText("Health " + curHp + " / " + maxHp);
    }
}
