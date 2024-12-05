using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class OpeningViewer : MonoBehaviour
{
    [Header("Talk UIs")] public GameObject talkWrapper;
    public TMP_Text talkText;

    [Header("Handlers")] private int curSceneId = 0;
    private int talkIdx;

    [Header("Skip Button")] public GameObject skipButton;
    
    [Header("Space Ship")]
    public GameObject spaceShip;

    private void Start()
    {
        TalkManager.SetTalkData();
        
        AudioManager.Instance.PlayBGM(AudioManager.Bgm.Opening,true);

        talkIdx = 1;

        if (GameManager.TutorialEnd)
        {
            skipButton.SetActive(true);
            
            skipButton.GetComponent<Button>().onClick.AddListener(()=>SceneController.ChangeScene("StageMenu"));
        }
        else
        {
            skipButton.SetActive(false);
        }

        StartCoroutine(TalkProcess());
    }
    
    private IEnumerator TalkProcess()
    {
        talkWrapper.GetComponent<Animator>().SetBool("isShow", true);
        int idx = 0;

        talkWrapper.SetActive(true);

        while (true)
        {
            string buff = TalkManager.GetTalk(curSceneId + talkIdx, idx);

            if (buff == null)
            {
                talkWrapper.GetComponent<Animator>().SetBool("isShow", false);
                StartCoroutine(WaitCoroutine());
                yield break;
            }

            //  이 코루틴이 종료될 때까지 대기후 idx++ (연산 오류 발생 방지)
            yield return StartCoroutine(TalkCoroutine(buff));
            idx++;

            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        }
    }

    private IEnumerator TalkCoroutine(string buff)
    {
        talkText.text = "";
        StringBuilder stringBuilder = new StringBuilder();

        for (int i = 0; i < buff.Length; i++)
        {
            stringBuilder.Append(buff[i]);
            talkText.text = stringBuilder.ToString();
            yield return new WaitForSeconds(0.005f);
        }
    }

    private IEnumerator WaitCoroutine()
    {
        yield return new WaitForSeconds(5f);
        talkIdx++;

        if (GameManager.Language == 0)
        {
            if (talkIdx <= TalkManager.ForOpeningIdx)
            {
                StartCoroutine(TalkProcess());
            }
            else
            {
                spaceShip.GetComponent<Animator>().SetBool("isEnd", true);
                yield return new WaitForSeconds(2f);
                GameManager.TutorialEnd = true;
                SceneController.ChangeScene("StageMenu");
            }
        }
        else
        {
            if (talkIdx <= TalkManager.ForOpeningIdx)
            {
                StartCoroutine(TalkProcess());
            }
            else
            {
                spaceShip.GetComponent<Animator>().SetBool("isEnd", true);
                yield return new WaitForSeconds(2f);
                GameManager.TutorialEnd = true;
                SceneController.ChangeScene("StageMenu");
            }
        }
    }
    
}
