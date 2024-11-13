using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class StageInfoManager : MonoBehaviour
{
    [SerializeField] private TMP_Text planetName;
    [SerializeField] private TMP_Text planetStory;
    [SerializeField] private TMP_Text planetInfo;
    [SerializeField] private GameObject planetSet;
    [SerializeField] private Animator animator;
    
    [SerializeField] private GameObject warpButton;
    
    public List<String> nameList;
    public List<String> storyList;
    public List<String> infoList;
    
    private int curStage;
    
    private void Start()
    {
        AudioManager.Instance.PlayBGM(AudioManager.Bgm.StageSelection,true);
        curStage = GameManager.CurStage;

        //  For Debugging
        if (curStage <= 0)
        {
            curStage = 1;
        }
        
        SetPlanet();
       
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

    public void SetStageMenuHide()
    {
        animator.SetBool("isWarp", true);
        warpButton.SetActive(false);
    }

    private IEnumerator WaitShowCoroutine()
    {
        yield return new WaitForSeconds(1.05f);
        
        warpButton.SetActive(true);
        Show();
    }

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

    private void SetPlanet()
    {
        nameList.Add("HJD-1029X2");
        
        storyList.Add("  This planet will be our first destination. It has similar environment with Earth" +
                      ", but has strong enemy forces defending it.\n\n" +
                      "  Our vanguard tried their best to conquer, " +
                      "but now we can't find traces of them anymore. " +
                      "Let's colonize this Planet.\nTake care and Focus Developer.....\nGood Luck.....");
        
        infoList.Add("Average temperature: 15.6\u00b0C\nPlanet diameter: 12,564 km\nBiological Population: 145,235,520\nPlanet type: Earth-type planet");
    }
}
