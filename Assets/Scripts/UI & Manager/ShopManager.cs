using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public TMP_Text[] shopTexts;
    public Button[] shopButtons;

    [Header("UI")] public Button shopButton;
    public Button closeButton;
    public GameObject shopWrapper;
    public GameObject blind;
    public TMP_Text coinText;
    public GameObject checkBox;
    public Button checkBuyButton;
    public Button checkCancelButton;

    [Header("Handle")] private int curMode;

    [Header("Alert")] public GameObject alertBox;
    public TMP_Text alertText;
    private int curAlert;
    private string[] alerts =
    {
        "Upgrade Completed.",
        "Not enough bits."
    };

    private void Start()
    {
        ValidateText();

        for (int i = 0; i < shopButtons.Length; i++)
        {
            int finalNum = i;
            shopButtons[i].onClick.AddListener(()=>CheckingBuy(finalNum));
        }
    }

    private void ValidateText()
    {
        coinText.SetText(DataManager.Coin.ToString());

        for (int i = 0; i < shopTexts.Length; i++)
        {
            shopTexts[i].SetText(DataManager.LevelList[i]+" / " + DataManager.LEVEL_MAX);
        }
    }

    private void ValidateUpgradeButtons()
    {
        for (int i = 0; i < shopButtons.Length; i++)
        {
            if (DataManager.LevelList[i] == DataManager.LEVEL_MAX)
            {
                shopButtons[i].interactable = false;
            }
            else
            {
                shopButtons[i].interactable = true;
            }
        }
    }

    public void Buy(bool buy)
    {
        if (buy)
        {
            if (DataManager.Upgrade(curMode))
            {
                ValidateText();
                ValidateUpgradeButtons();
                Debug.Log("업그레이드 완료");
                curAlert = 0;
                //  TODO : Alert + Audio
            }
            else
            {
                //  TODO : Alert
                curAlert = 1;
                Debug.Log("업그레이드 불가");
                Debug.Log("돈 없거나 최대 레벨"); //    최대 레벨은 버튼을 막기로 결정
            }
            
            alertText.SetText(alerts[curAlert]);
            ShowAlert();
        }
        else
        {
            Debug.Log("구매 취소 처리 완료");
        }
        
        //  button click 방지 해제
        for (int i = 0; i < shopButtons.Length; i++)
        {
            shopButtons[i].interactable = true;
        }
        
        checkBox.SetActive(false);
    }

    public void CheckingBuy(int mode)
    {
        curMode = mode;
        checkBox.SetActive(true);
        
        //  button click 방지
        for (int i = 0; i < shopButtons.Length; i++)
        {
            shopButtons[i].interactable = false;
        }
    }
    
    //  Animation
    public void ShowShop()
    {
        shopWrapper.GetComponent<Animator>().SetBool("visible", true);
        blind.SetActive(true);
    }

    public void HideShop()
    {
        shopWrapper.GetComponent<Animator>().SetBool("visible", false);
        blind.SetActive(false);
    }
    
    //  Alert
    public void ShowAlert()
    {
        
        StartCoroutine(AlertCoroutine());
    }

    private IEnumerator AlertCoroutine()
    {
        alertBox.GetComponent<Animator>().SetBool("visible", true);
        yield return new WaitForSeconds(1.2f);
        HideAlert();
    }

    public void HideAlert()
    {
        alertBox.GetComponent<Animator>().SetBool("visible", false);
    }
}