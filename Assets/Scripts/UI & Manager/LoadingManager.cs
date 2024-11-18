using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

/*
 *  사용자의 경험을 증진하기 위한 Loading 창에 대한 스크립트입니다.
 *  모든 Loading 씬은 이 스크립트를 동일하게 사용합니다.
 */
public class LoadingManager : MonoBehaviour
{
    //  스킵 버튼
    public GameObject skip;
    //  로딩 애니메이션 이미지
    public GameObject loading;
    //  Tip Text
    public TMP_Text tips;

    //  Tip List
    public List<string> tipList = new List<string>();
    
    //  Loading 창에서만 작동 (모든 로딩씬은 이것으로 통일)
    private void Start()
    {
        skip = GameObject.Find("Skip");
        loading = GameObject.Find("Loading");
        tips = GameObject.Find("Tips").GetComponent<TMP_Text>();
        
        //  Skip이 처음에는 불가
        skip.SetActive(false);
        GameManager.LoadingSkip = false;
        
        SetTipList();
        SetTip();

        StartCoroutine(LoadingCoroutine());
    }

    //  로딩 시간의 기본 값은 3초
    //  게임 입장 전의 Loading은 SceneController에서 관리함
    private IEnumerator LoadingCoroutine()
    {
        yield return new WaitForSeconds(3f);
        loading.SetActive(false);
        skip.SetActive(true);
        GameManager.LoadingSkip = true;
    }

    //  Tip을 랜덤으로 보여줌
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

    //  랜덤 Seed를 통해서 팁 하나를 리턴
    private String GetRandomTip()
    {
        return tipList[Random.Range(0, tipList.Count)];
    }
}
