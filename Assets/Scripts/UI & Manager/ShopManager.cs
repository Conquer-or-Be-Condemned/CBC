using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [Space]
    public TMP_Text[] shopTexts;
    public Button[] shopButtons;

    [Space]
    [Header("UI")] public Button shopButton;
    public Button closeButton;
    public GameObject shopWrapper;
    public GameObject blind;

    [Space] 
    public GameObject CoinWrapper;
    public TMP_Text coinText;
    
    [Space]
    public GameObject checkBox;
    public TMP_Text checkBoxText;
    public Button checkBuyButton;
    public Button checkCancelButton;

    [Space]
    [Header("Handle")] private int curMode;

    [Space]
    [Header("Alert")] public GameObject alertBox;
    public TMP_Text alertText;
    
    private int curAlert;
    private readonly string[][] alerts =
    {
        new string[]
        {
            "Upgrade Completed.",
            "Not enough bits."
        },
        new string[]
        {
            "업그레이드 완료",
            "Bit가 부족합니다."
        }
    };

    [Space]
    [Header("Handler")]
    public bool isInit = false;


    private void Awake()
    {
        ValidateText();

        for (int i = 0; i < shopButtons.Length; i++)
        {
            int finalNum = i;
            shopButtons[i].onClick.AddListener(()=>CheckingBuy(finalNum));
        }
        
        ValidateUpgradeButtons();
    }

    private void FixedUpdate()
    {
        if (!isInit)
        {
            Awake();
        }
    }

    private void ValidateText()
    {
        coinText.SetText(DataManager.Coin.ToString());
        
        RevalidateAlignment();
        // LayoutRebuilder.ForceRebuildLayoutImmediate(gameObject.GetComponent<RectTransform>());

        for (int i = 0; i < shopTexts.Length; i++)
        {
            shopTexts[i].SetText(DataManager.GetLevel(i)+" / " + DataManager.LEVEL_MAX);
        }
    }

    private void ValidateUpgradeButtons()
    {
        for (int i = 0; i < shopButtons.Length; i++)
        {
            if (DataManager.GetLevel(i) == DataManager.LEVEL_MAX)
            {
                shopButtons[i].interactable = false;
            }
            else
            {
                shopButtons[i].interactable = true;
            }
        }

        isInit = true;
    }

    public void Buy(bool buy)
    {
        if (buy)
        {
            if (DataManager.Upgrade(curMode))
            {
                ValidateText();
                ValidateUpgradeButtons();
                // Debug.Log("업그레이드 완료");
                curAlert = 0;
                //  TODO : Alert + Audio
            }
            else
            {
                //  TODO : Alert
                curAlert = 1;
                // Debug.Log("업그레이드 불가");
                // Debug.Log("돈 없거나 최대 레벨"); //    최대 레벨은 버튼을 막기로 결정
            }
            
            alertText.SetText(alerts[(int)GameManager.SelectedLanguage][curAlert]);
            ShowAlert();
        }
        else
        {
            // Debug.Log("구매 취소 처리 완료");
        }
        
        //  button click 방지 해제
        for (int i = 0; i < shopButtons.Length; i++)
        {
            shopButtons[i].interactable = true;
        }
        
        checkBox.SetActive(false);
        
        ValidateUpgradeButtons();
    }

    public void CheckingBuy(int mode)
    {
        curMode = mode;
        checkBox.SetActive(true);

        if (GameManager.SelectedLanguage == AvailableLanguage.English)
        {
            checkBoxText.SetText("Are you sure you want to buy it?");
        }
        else if(GameManager.SelectedLanguage == AvailableLanguage.Korean)
        {
            checkBoxText.SetText("구매하시겠습니까?");
        }
        
        //  button click 방지
        for (int i = 0; i < shopButtons.Length; i++)
        {
            shopButtons[i].interactable = false;
        }
    }
    
    //  Animation
    public void ShowShop()
    {
        RevalidateAlignment();
        shopWrapper.GetComponent<Animator>().SetBool("visible", true);
        blind.SetActive(true);
    }

    public void HideShop()
    {
        shopWrapper.GetComponent<Animator>().SetBool("visible", false);
        blind.SetActive(false);
    }

    private void RevalidateAlignment()
    {
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(CoinWrapper.GetComponent<RectTransform>());
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
