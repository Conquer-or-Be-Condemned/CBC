using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndingViewer : MonoBehaviour
{
    [Header("Objects")]
    public GameObject endingText;

    public GameObject logoText;

    [Header("Button")] public Button goToMain;

    // [Header("Handlers")] 
    // public float showTerm = 7f;
    // public float resetTerm = 5f;
    // public float hideTerm = 0.5f;

    [Header("Text")] 
    public List<string> endingList_ENG = new List<string>();
    public List<string> endingList_KOR = new List<string>();
    
    private void Start()
    {
        goToMain.onClick.AddListener(()=>SceneController.ChangeScene("Main"));
        SetEndingText();
        
        AudioManager.Instance.PlayBGM(AudioManager.Bgm.Ending,true);
        
        StartCoroutine(ShowTextCoroutine());
    }

    public void ShowText(int idx)
    {
        endingText.GetComponent<Animator>().SetBool("visible", true);
    }

    public void HideText()
    {
        endingText.GetComponent<Animator>().SetBool("visible", false);
    }

    private IEnumerator ShowTextCoroutine()
    {
        int idx = 0;

        int leng = 0;
        
        if (GameManager.Language == 0)
        {
            endingText.GetComponent<TMP_Text>().SetText(endingList_ENG[idx]);
            leng = endingList_ENG.Count;
        }
        else if (GameManager.Language == 1)
        {
            endingText.GetComponent<TMP_Text>().SetText(endingList_KOR[idx]);
            leng = endingList_KOR.Count;
        }
        
        while (true)
        {
            
            ShowText(idx);
            yield return new WaitForSeconds(5.5f);
            idx++;
            
            if (idx >= leng)
            {
                HideText();
                yield return new WaitForSeconds(2);
                endingText.GetComponent<TMP_Text>().SetText("");
                yield return new WaitForSeconds(0.1f);
                StartCoroutine(ShowEndingCreditCoroutine());
                yield break;
            }
            
            StartCoroutine(HideTextCoroutine(idx));

            yield return new WaitForSeconds(2.1f);
        }
    }

    private IEnumerator HideTextCoroutine(int idx)
    {
        HideText();
        
        yield return new WaitForSeconds(2);
        
        StartCoroutine(ResetTextCoroutine(idx));
    }

    private IEnumerator ResetTextCoroutine(int idx)
    {
        if (GameManager.Language == 0)
        {
            endingText.GetComponent<TMP_Text>().SetText(endingList_ENG[idx]);
        }
        else if (GameManager.Language == 1)
        {
            endingText.GetComponent<TMP_Text>().SetText(endingList_KOR[idx]);
        }
        
        yield return new WaitForSeconds(0.1f);
    }

    private void SetEndingText()
    {
        endingList_ENG.Add("Finally, \"The developer\" succeeded colonizing the planets");
        endingList_ENG.Add("Even missions that the soldiers themselves couldn’t complete were successfully accomplished by the developer alone.");
        endingList_ENG.Add("People began to show him big respect.");
        endingList_ENG.Add("The I.S.C.A.‘s chairman was removed from his position and replaced with someone else.");
        endingList_ENG.Add("The new chairman assigned a team to the developer and asked him to take the lead in conquering other planets.");
        endingList_ENG.Add("The developer gladly accepted and led the world’s most formidable unit to conquer the universe.");
        endingList_ENG.Add("To be Continued.....");
        
        endingList_KOR.Add("마침내 \"The developer\" 는 행성들을\n정복하는데 성공했다.");
        endingList_KOR.Add("군인들도 성공하지 못했던 임무들이\n그에 의해서 성공적으로 완수됐다.");
        endingList_KOR.Add("시민들은 그에 대한 존경을\n표하기 시작했다.");
        endingList_KOR.Add("I.S.C.A.의 위원장은 보직에서 해제됐고\n다른사람으로 교체됐다.");
        endingList_KOR.Add("새로운 위원장은 개발자에게 팀을 주며\n다른 행성들의 정복에 앞장 설 것을 부탁했다.");
        endingList_KOR.Add("개발자는 흔쾌히 수락했고 우주를 정복하는\n세계 최강의 부대를 이끌어 나간다.");
        endingList_KOR.Add("To be Continued.....");
    }

    public IEnumerator ShowEndingCreditCoroutine()
    {
        logoText.GetComponent<TMP_Text>().SetText("The Developer");
        yield return new WaitForSeconds(0.5f);
        logoText.GetComponent<Animator>().SetBool("visible",true);
        // yield return new WaitForSeconds(5);
        // logoText.GetComponent<Animator>().SetBool("visible",false);
        // yield return new WaitForSeconds(2);
        //
        // logoText.GetComponent<TMP_Text>().SetText("The End");
        // yield return new WaitForSeconds(0.5f);
        // logoText.GetComponent<Animator>().SetBool("visible",true);
    }

}
