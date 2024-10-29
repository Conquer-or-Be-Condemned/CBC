using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class ControlUnitStatus : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private int MaxPower = 100;
    [SerializeField] private int CurrentPower = 100;
    
    private GameObject[] units;//현재 가동 중인 타워 배열
    List<GameObject> unitsList = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // CurrentPower++;//debug
    }

    public void AddUnit(int power)
    {
        CurrentPower = CurrentPower - power;
    }

    public void RemoveUnit(int power)
    {
        CurrentPower = CurrentPower + power;
        
    }

    public int getCurrentPower()
    {
        return CurrentPower;
    }
}
