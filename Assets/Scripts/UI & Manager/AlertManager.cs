using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/*
 *  알림창을 전반적으로 관리하는 Script입니다. 알림창의 애니메이션과 내용을 관할하니,
 *  내용 기입에 주의해주세요.
 */

public class AlertManager : MonoBehaviour
{
    [SerializeField] private GameObject alertBox;
    [SerializeField] private TMP_Text alertInfo;
    
    public Animator alertAnimator;

    private WaitForSeconds _UIDelay1 = new WaitForSeconds(2.0f);
    private WaitForSeconds _UIDelay2 = new WaitForSeconds(2.0f);
    
    //  Alert Text 배열입니다. 사용에 주의하세요.
    private List<String> alertTexts = new List<String>();

    //  게임 내에서만 실행 됨
    private void Start()
    {
        InGame();
    }

    //  인 게임에서 필요한 변수들과 세팅들을 초기화 시킴
    private void InGame()
    {
        if (alertBox == null)
        {
            alertBox = GameObject.FindGameObjectWithTag("AlertBox");
            if (alertBox == null)
            {
                Debug.LogError("Alert Box가 없습니다.");
            }
        }

        if (alertInfo == null)
        {
            alertInfo = GameObject.FindGameObjectWithTag("AlertInfo").GetComponent<TMP_Text>();
            if (alertInfo == null)
            {
                Debug.LogError("Alert Info가 없습니다.");
            }
        }
        
        alertBox.SetActive(false);
        
        //  Alert Text init.
        SetAlertText();
        
        //  Set Animator
        alertAnimator = alertBox.GetComponent<Animator>();
        
        //  Active 체크 Event
        //_camoTurretLV1.onActivateChange.AddListener(Show);
        
        //  시작 시 알림 등장
        Show("Watch out next wave is coming!\nTry your best Developer");
    }

    //  원하는 메시지 쓰고 싶을 때 사용
    public void Show(String message)
    {
        alertInfo.SetText(message);
        alertBox.SetActive(false);
        StopAllCoroutines();
        StartCoroutine(SubDelay());
    }

    //  OverLoading Method - 정해진 알림 띄울 때 사용
    public void Show(int i)
    {
        if (i <= 0)
        {
            Debug.LogError("Alert Box의 Index 값이 잘못되었습니다.");
            return;
        }
        
        alertInfo.SetText(alertTexts[i - 1]);
        alertBox.SetActive(false);
        StopAllCoroutines();
        StartCoroutine(SubDelay());
    }

    //  알림창 애니메이션
    private IEnumerator SubDelay()
    {
        alertBox.SetActive(true);
        alertAnimator.SetBool("show",true);
        yield return _UIDelay1;
        
        alertAnimator.SetBool("show",false);
        yield return _UIDelay2;
        alertBox.SetActive(false);
    }

    //  Alert 관련 추가는 여기서 하면 됩니다.
    //  Notion의 데이터 베이트 참고 바랍니다.
    private void SetAlertText()
    {
        alertTexts.Clear();
        
        alertTexts.Add("You are out of power!!!");
        alertTexts.Add("Tower has been deactivated.");
        alertTexts.Add("Tower has been activated.");
    }
}
