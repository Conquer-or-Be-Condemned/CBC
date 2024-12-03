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

    [Header("Text")] public List<string> endingList = new List<string>();
    private void Start()
    {
        goToMain.onClick.AddListener(()=>SceneController.ChangeScene("Main"));
        SetEndingText();
        
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
        
        endingText.GetComponent<TMP_Text>().SetText(endingList[idx]);
        
        while (true)
        {
            
            ShowText(idx);
            yield return new WaitForSeconds(3.2f);
            idx++;
            
            if (idx >= endingList.Count)
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
        endingText.GetComponent<TMP_Text>().SetText(endingList[idx]);
        
        yield return new WaitForSeconds(0.1f);
    }

    private void SetEndingText()
    {
        endingList.Add("마침내 모든 식민 전쟁은 종료 되었다.");
        endingList.Add("점령에 성공한 개발자는 마침내 고향행성으로 돌아갔다.");
        endingList.Add("하지만, 지구는 이미 없어지고 말았다");
        endingList.Add("바로 황재동의 계엄령 때문이었다.");
        endingList.Add("개발자는 Chat-GPT에게 물어봤다.");
        endingList.Add("Chat-GPT는 개발자에게 말했다.");
        endingList.Add("Fatal : Null Pointer Exception");
        endingList.Add("여기까지 엔딩입니다.");
        endingList.Add("플레이 해주셔서 감사합니다.");
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
