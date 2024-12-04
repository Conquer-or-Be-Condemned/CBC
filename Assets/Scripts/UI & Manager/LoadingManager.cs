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
    public List<string> tipList_ENG = new List<string>();
    public List<string> tipList_KOR = new List<string>();
    
    //  Loading 창에서만 작동 (모든 로딩씬은 이것으로 통일)
    private void Start()
    {
        //  TODO : 아예 브금을 안나오게 하는 Method 있으면 좋을 듯
        AudioManager.Instance.PlayBGM(AudioManager.Bgm.StartingScene,false);
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
        tipList_ENG.Clear();
        
        tipList_ENG.Add("[Tips] Recommend turning on towers before the wave starts.");
        tipList_ENG.Add("[Tips] Hello, World!");
        tipList_ENG.Add("[Tips] It's very useful.");
        tipList_ENG.Add("[Tips] Please save electricity.");
        tipList_ENG.Add("[Tips] When you cook ramen, put the soup first.");
        tipList_ENG.Add("[Tips] We always welcome sponsorship.");
        tipList_ENG.Add("[Tips] Why is the team name HJD? Well, ask Jae-dong.");
        tipList_ENG.Add("[Tips] I'm sorry. Actually, I don't have much to give you.");
        tipList_ENG.Add("[Tips] Minimap is a very useful map...");
        
        tipList_KOR.Clear();
        
        // tipList_KOR.Add("[팁] 웨이브가 시작하기 전에 타워를 켜는 것을 추천합니다.");
        // tipList_KOR.Add("[팁] 안녕하세요, 세상!");
        // tipList_KOR.Add("[팁] 이건 매우 유용합니다.");
        // tipList_KOR.Add("[팁] 전기를 아껴주세요.");
        // tipList_KOR.Add("[팁] 라면을 끓일 때, 스프를 먼저 넣으세요.");
        // tipList_KOR.Add("[팁] 우리는 항상 후원을 환영합니다.");
        // tipList_KOR.Add("[팁] 팀 이름이 왜 HJD인가요? 음... 재동에게 물어보세요.");
        // tipList_KOR.Add("[팁] 죄송합니다. 사실 드릴 게 별로 없네요.");
        // tipList_KOR.Add("[팁] 미니맵은 매우 유용한 지도입니다...");
        tipList_KOR.Add("마시지마 ㅋㅋ N빵이야");
        tipList_KOR.Add("우리 1002844030401");

    }

    //  랜덤 Seed를 통해서 팁 하나를 리턴
    private String GetRandomTip()
    {
        if (GameManager.Language == 0)
        {
            return tipList_ENG[Random.Range(0, tipList_ENG.Count)];
        }
        else if (GameManager.Language == 1)
        {
            return tipList_KOR[Random.Range(0, tipList_KOR.Count)];
        }
        else
        {
            //  ERROR
            return null;
        }
    }
}
