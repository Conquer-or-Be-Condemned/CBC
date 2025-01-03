using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/*
 *  UI 중 Control Unit에 대한 체력바 등을 관리하는 스크립트입니다.
 *  Control Unit의 태그는 CU, TMP는 CUHpText로 지정되어 있습니다.
 *  (본 스크립트 CU Power Object에만 연결되어 있습니다.)
 */

public class UICUInfo : MonoBehaviour
{
    private GameObject controlUnit;
    private ControlUnitStatus status;
    public TMP_Text powerText;
    public TMP_Text hpText;
    
    [Header("Filler")]
    public Image filledHpImage;

    public Image filledPowerImage;

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
                // Debug.LogError("UI ERROR : Hp Text를 반드시 연결해주어야 합니다.");
                SceneController.ExitProgram();
            }
        }
        
        
        //  Control Unit Status의 Event와 연결
        status.onCUHpChange.AddListener(SetUICUHpInfo);
        status.onCUPowerChange.AddListener(SetUICUPowerInfo);
    }
    
    //  Hp에 대한 변동사항이 있을 때 호출됩니다.
    public void SetUICUHpInfo(int curHp, int maxHp, float preRatio)
    {
        int ratio = (int)((curHp / (float)maxHp) * 100);
        if (preRatio > (curHp / (float)maxHp))
        {
            StartCoroutine(UIHpInfoCoroutine(preRatio, (curHp / (float)maxHp)));
        }
        else
        {
            filledHpImage.fillAmount = curHp / (float)maxHp;
        }
        hpText.SetText(ratio + "%");
    }
    
    //  Power에 대한 변동사항이 있을 때 호출됩니다.
    public void SetUICUPowerInfo(int curPower, int maxPower, float preRatio)
    {
        int ratio = (int)((curPower / (float)maxPower) * 100);
        
        if (preRatio > (curPower / (float)maxPower))
        {
            StartCoroutine(UIPowerInfoCoroutine(preRatio, (curPower / (float)maxPower)));
        }
        //  Power Recover
        else
        {
            filledPowerImage.fillAmount = (curPower / (float)maxPower);
        }
        
        
        powerText.SetText(ratio + "%");
    }

    private IEnumerator UIPowerInfoCoroutine(float preRatio, float fRatio)
    {
        float elapsedRatio = preRatio;

        if (preRatio > fRatio)  //  power use
        {
            while (true)
            {
                if (elapsedRatio <= fRatio)
                {
                    yield break;
                }

                elapsedRatio -= 0.01f;

                filledPowerImage.fillAmount = elapsedRatio;
            
                yield return new WaitForSeconds(0.01f);
            }
        }
        //  power recover
        // else
        // {
        //     while (true)
        //     {
        //         if (elapsedRatio >= fRatio)
        //         {
        //             yield break;
        //         }
        //
        //         elapsedRatio += 0.01f;
        //
        //         filledPowerImage.fillAmount = elapsedRatio;
        //     
        //         yield return new WaitForSeconds(0.01f);
        //     }
        // }
    }

    private IEnumerator UIHpInfoCoroutine(float preRatio, float fRatio)
    {
        float elapsedRatio = preRatio;

        if (preRatio > fRatio)  //  power use
        {
            while (true)
            {
                if (elapsedRatio <= fRatio)
                {
                    yield break;
                }

                elapsedRatio -= 0.01f;

                filledHpImage.fillAmount = elapsedRatio;
            
                yield return new WaitForSeconds(0.01f);
            }
        }
        //  power recover
        // else
        // {
        //     while (true)
        //     {
        //         if (elapsedRatio >= fRatio)
        //         {
        //             yield break;
        //         }
        //
        //         elapsedRatio += 0.01f;
        //
        //         filledHpImage.fillAmount = elapsedRatio;
        //     
        //         yield return new WaitForSeconds(0.01f);
        //     }
        // }
    }
    
}
