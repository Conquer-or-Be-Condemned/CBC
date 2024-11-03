using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/*
 *  UI 중 Control Unit에 대한 체력바 등을 관리하는 스크립트입니다.
 *  Control Unit의 태그는 CU, TMP는 CUHpText로 지정되어 있습니다.
 */

public class UICUInfo : MonoBehaviour
{
    private GameObject controlUnit;
    private ControlUnitStatus status;
    private RectTransform hpRect;
    public TMP_Text hpText;
    public float maxWidth;

    private void Awake()
    {
        //  Control Unit Object 자동 연결
        if (controlUnit == null)
        {
            controlUnit = GameObject.FindGameObjectWithTag("CU");
        }
        status = controlUnit.GetComponent<ControlUnitStatus>();
        
        //  Control Unit UI의 Text 자동 연결
        if (hpText == null)
        {
            hpText = GameObject.FindWithTag("CUHpText").GetComponent<TMP_Text>();
            if (hpText == null)
            {
                Debug.LogError("UI ERROR : Hp Text를 반드시 연결해주어야 합니다.");
                SceneController.ExitProgram();
            }
        }
        
        //  본래의 이미지 Width 저장
        hpRect = gameObject.GetComponent<RectTransform>();
        maxWidth = hpRect.sizeDelta.x;
        
        //  Control Unit Status의 Event와 연결
        status.OnCUHpChange.AddListener(SetUICUHpInfo);
    }
    
    //  Hp에 대한 변동사항이 있을 때 호출됩니다.
    public void SetUICUHpInfo(int curHp, int maxHp)
    {
        float ratio = curHp / (float)maxHp;
        hpRect.sizeDelta = new Vector2(maxWidth * ratio, hpRect.sizeDelta.y);
        hpText.SetText("Health " + curHp + " / " + maxHp);
    }
    
}
