using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class StageSelectManager : MonoBehaviour
{
    [Header("Planets")] public GameObject[] planets;

    [Header("Buttons")] 
    public GameObject leftButton;
    public GameObject rightButton;
    public GameObject warpButton;

    public int curSelectStage;

    [Header("Various Buttons")] public Button goToMainButton;
    // public Button tipButton;

    public void Start()
    {
        curSelectStage = GameManager.CurStage - 1;
        
        planets[curSelectStage].SetActive(true);
        planets[curSelectStage].GetComponent<Animator>().SetBool("visible", true);
        
        //  Init
        for (int i = 0; i < planets.Length; i++)
        {
            if (curSelectStage > i)
            {
                planets[i].GetComponent<Animator>().SetBool("visible", false);
                planets[i].GetComponent<Animator>().SetBool("isLeft", true);
                // planets[i].SetActive(false);
            }
            else if(curSelectStage < i)
            {
                planets[i].GetComponent<Animator>().SetBool("visible", false);
                planets[i].GetComponent<Animator>().SetBool("isLeft", false);
                // planets[i].SetActive(false);
            }
        }

        if (curSelectStage == 0)
        {
            leftButton.GetComponent<Button>().interactable = false;
        }
        else if (curSelectStage == planets.Length - 1)
        {
            leftButton.GetComponent<Button>().interactable = true;
        }
        
        goToMainButton.onClick.AddListener(()=>SceneController.ChangeScene("Main"));
    }

    //  추가 검증을 위한 점검
    private void FixedUpdate()
    {
        if (curSelectStage == 0)
        {
            leftButton.GetComponent<Button>().interactable = false;
        }
        else
        {
            leftButton.GetComponent<Button>().interactable = true;
        }

        if (curSelectStage == planets.Length - 1)
        {
            rightButton.GetComponent<Button>().interactable = false;
        }
        else
        {
            rightButton.GetComponent<Button>().interactable = true;
        }
        
        //  Warp Button
        if (curSelectStage <= GameManager.CurStage - 1)
        {
            warpButton.GetComponent<Button>().interactable = true;
        }
        else
        {
            warpButton.GetComponent<Button>().interactable = false;
        }
    }

    public void OnLeftButton()
    {
        curSelectStage--;
        
        if (curSelectStage <= 0)
        {
            curSelectStage = 0;
            leftButton.GetComponent<Button>().interactable = false;
            rightButton.GetComponent<Button>().interactable = true;
            Debug.Log("Left Boundary");
        }
        else
        {
            leftButton.GetComponent<Button>().interactable = true;
        }
        
        planets[curSelectStage + 1].GetComponent<Animator>().SetBool("isLeft", false);
        StartCoroutine(HidePlanetCoroutine(curSelectStage + 1));
        
        planets[curSelectStage].SetActive(true);
        planets[curSelectStage].GetComponent<Animator>().SetBool("visible", true);
        
        GeneralManager.Instance.stageInfoManager.Show();
    }

    private IEnumerator HidePlanetCoroutine(int idx)
    {
        planets[idx].GetComponent<Animator>().SetBool("visible", false);
        yield return new WaitForSeconds(1f);
        // planets[idx].SetActive(false);
    }

    public void OnRightButton()
    {
        curSelectStage++;
        
        if (curSelectStage >= planets.Length - 1)
        {
            curSelectStage = planets.Length - 1;
            leftButton.GetComponent<Button>().interactable = true;
            rightButton.GetComponent<Button>().interactable = false;
            Debug.Log("Right Boundary");
        }
        else
        {
            rightButton.GetComponent<Button>().interactable = true;
        }
        
        planets[curSelectStage - 1].GetComponent<Animator>().SetBool("isLeft", true);
        StartCoroutine(HidePlanetCoroutine(curSelectStage - 1));
        
        planets[curSelectStage].GetComponent<Animator>().SetBool("visible", true);
        planets[curSelectStage].SetActive(true);
        
        GeneralManager.Instance.stageInfoManager.Show();
    }

    //  For Debug
    private GameObject CheckNowPlanet()
    {
        return planets[curSelectStage];
    }

    public int GetCurSelectStage()
    {
        return curSelectStage;
    }

}
