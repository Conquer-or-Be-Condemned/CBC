using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

/*
 *  Stage 선택 창에서 Stage에 대한 정보를 출력하고 저장하는 스크립트입니다.
 */
public class StageInfoManager : MonoBehaviour
{
    //  UI의 정보들
    [SerializeField] private TMP_Text planetName;
    [SerializeField] private TMP_Text planetStory;
    [SerializeField] private TMP_Text planetInfo;
    [SerializeField] private GameObject planetSet;
    [SerializeField] private Animator animator;
    
    [SerializeField] private GameObject warpButton;
    
    //  정보 저장을 위한 List
    public List<String> nameList;
    public List<String> storyList;
    public List<String> infoList;
    
    //  현재 Display되고 있는 스테이지
    private int curStage;
    
    private void Start()
    {
        curStage = GameManager.CurStage;

        //  For Debugging
        // if (curStage <= 0)
        // {
        //     curStage = 1;
        // }
        
        SetPlanet();
       
        //  모든 정보를 공백으로 초기화
        planetName.SetText("");
        planetStory.SetText("");
        planetInfo.SetText("");

        if (planetSet == null)
        {
            planetSet = GameObject.Find("PlanetSet");
        }

        if (warpButton == null)
        {
            warpButton = GameObject.Find("ToGame");
        }
        warpButton.SetActive(false);

        animator = planetSet.GetComponent<Animator>();
        
        StartCoroutine(WaitShowCoroutine());
    }

    //  Stage 정보 메뉴를 숨기는 함수
    public void SetStageMenuHide()
    {
        animator.SetBool("isWarp", true);
        warpButton.SetActive(false);
    }

    //  씬 전환 직후 애니메이션과 워프 버튼에 딜레이 부여
    private IEnumerator WaitShowCoroutine()
    {
        yield return new WaitForSeconds(1.05f);
        
        warpButton.SetActive(true);
        Show();
    }

    //  String Builder를 사용하기 위한 코루틴
    private void Show()
    {
        StartCoroutine(NameCoroutine());
    }
    
    private IEnumerator NameCoroutine()
    {
        StringBuilder stringBuilder = new StringBuilder();

        for (int i = 0; i < nameList[curStage-1].Length; i++)
        {
            stringBuilder.Append(nameList[curStage - 1][i]);
            planetName.text = stringBuilder.ToString();
            yield return new WaitForSeconds(0.05f);
        }
        StartCoroutine(StoryCoroutine());
    }
    //  NameCoroutine 다음에 연달아 실행
    private IEnumerator StoryCoroutine()
    {
        StringBuilder stringBuilder = new StringBuilder();

        for (int i = 0; i < storyList[curStage-1].Length; i++)
        {
            stringBuilder.Append(storyList[curStage - 1][i]);
            planetStory.text = stringBuilder.ToString();
            yield return new WaitForSeconds(0.005f);
        }
        StartCoroutine(InfoCoroutine());
    }
    //  마찬가지로 연달아 실행
    private IEnumerator InfoCoroutine()
    {
        StringBuilder stringBuilder = new StringBuilder();

        for (int i = 0; i < infoList[curStage-1].Length; i++)
        {
            stringBuilder.Append(infoList[curStage - 1][i]);
            planetInfo.text = stringBuilder.ToString();
            yield return new WaitForSeconds(0.005f);
        }
    }

    //  정보 저장을 위한 Method
    private void SetPlanet()
    {
        nameList.Add("HJD-1029X2");
        
        storyList.Add("This planet is our first destination. It has a very similar environment " +
                      "to the Earth, but it turns out that the enemy force is not large. " +
                      "It is the planet our vanguard tried to capture the most. " +
                      "But we can't find any more traces of them. " +
                      "Let's expand our colonies from this planet. Good luck, developer.");
        
        infoList.Add("Average temperature: 15.6\u00b0C\nPlanet diameter: 12,564 km\nBiological Population: 145,235,520\nPlanet type: Earth-type planet");
    }
}
