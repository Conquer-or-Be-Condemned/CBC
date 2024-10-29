using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AlertManager : MonoBehaviour
{
    [SerializeField] private GameObject alertBox;
    [SerializeField] private TMP_Text alertInfo;

    private void Start()
    {
        if (alertBox == null)
        {
            alertBox = GameObject.FindGameObjectWithTag("AlertBox");
        }

        if (alertInfo == null)
        {
            alertInfo = GameObject.FindGameObjectWithTag("AlertInfo").GetComponent<TMP_Text>();
        }
    }
}
