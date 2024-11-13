using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LoadingManager : MonoBehaviour
{
    public GameObject skip;
    public GameObject loading;
    public TMP_Text tips;

    public List<String> tipList;
    private void Start()
    {
        if (skip == null)
        {
            skip = GameObject.Find("Skip");
        }

        if (loading == null)
        {
            loading = GameObject.Find("Loading");
        }
        
        skip.SetActive(false);
        GameManager.LoadingSkip = false;
        
        SetTipList();
        SetTip();
        AudioManager.Instance.PlayBGM(AudioManager.Bgm.StartingScene,false);
        AudioManager.Instance.PlayBGM(AudioManager.Bgm.StageSelection,false);
        StartCoroutine(LoadingCoroutine());
    }

    private IEnumerator LoadingCoroutine()
    {
        yield return new WaitForSeconds(3f);
        loading.SetActive(false);
        skip.SetActive(true);
        GameManager.LoadingSkip = true;
    }

    private void SetTip()
    {
        tips.SetText(GetRandomTip());
    }

    private void SetTipList()
    {
        tipList.Clear();
        
        tipList.Add("[Tips] Recommend turning on towers before the wave starts.");
        tipList.Add("[Tips] Hello, World!");
        tipList.Add("[Tips] It's very useful.");
        tipList.Add("[Tips] Please save electricity.");
        tipList.Add("[Tips] When you cook ramen, put the soup first.");
        tipList.Add("[Tips] We always welcome sponsorship.");
        tipList.Add("[Tips] Why is the team name HJD? Well, ask Jae-dong.");
        tipList.Add("[Tips] I'm sorry. Actually, I don't have much to give you.");
        tipList.Add("[Tips] Minimap is a very useful map...");
    }

    private String GetRandomTip()
    {
        return tipList[Random.Range(0, tipList.Count)];
    }
}
