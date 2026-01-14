using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    [Header("Audio Manager")] public AudioManager audioManager;

    [Header("Sliders")] 
    public Slider bgmSlider;
    public Slider sfxSlider;

    [Header("DropBox")] public TMP_Dropdown languageSetting;
    
    [Header("Handler")] public bool isEnable;
    private void Start()
    {
        isEnable = false;
        audioManager = GeneralManager.Instance.audioManager;
    }

    private bool CheckSettingAble()
    {
        if (GameObject.Find("Settings") != null)
        {
            isEnable = true;
            bgmSlider = GameObject.Find("BgmSlider").GetComponent<Slider>();
            sfxSlider = GameObject.Find("SfxSlider").GetComponent<Slider>();
            
            if (GameObject.Find("Dropdown")!=null)
            {
                languageSetting = GameObject.Find("Dropdown").GetComponent<TMP_Dropdown>();
                
                languageSetting.value = (int)GameManager.SelectedLanguage;
                languageSetting.onValueChanged.AddListener((e)=>GameManager.Instance.SetLanguageSetting(e));
            }
            
            bgmSlider.onValueChanged.AddListener((e)=>audioManager.ChangeBgmVolume(e));
            sfxSlider.onValueChanged.AddListener((e)=>audioManager.ChangeSfxVolume(e));
            
            bgmSlider.value = AudioManager.Instance.bgmVolume;
            sfxSlider.value = AudioManager.Instance.sfxVolume;
            
            // Debug.Log(bgmSlider.value);
            // Debug.Log(sfxSlider.value);
            
            return true;
        }
        else
        {
            return false;
        }
    }

    public void AllocateSetting()
    {
        isEnable = true;
        bgmSlider = GameObject.Find("BgmSlider").GetComponent<Slider>();
        sfxSlider = GameObject.Find("SfxSlider").GetComponent<Slider>();
        
        //  silder의 범위 변환
        bgmSlider.value = AudioManager.Instance.bgmVolume;
        sfxSlider.value = AudioManager.Instance.sfxVolume;
            
        // Debug.Log(bgmSlider.value);
        // Debug.Log(sfxSlider.value);
        
        bgmSlider.onValueChanged.AddListener((e)=>audioManager.ChangeBgmVolume(e));
        sfxSlider.onValueChanged.AddListener((e)=>audioManager.ChangeSfxVolume(e));
    }

    public void FixedUpdate()
    {
        if (!isEnable)
        {
            if (!CheckSettingAble())
            {
                bgmSlider = null;
                sfxSlider = null;
            }
        }

        // if (languageSetting != null)
        // {
        //     if (languageSetting.value != GameManager.Language)
        //     {
        //         languageSetting.value = GameManager.Language;
        //     }
        // }
    }
}
