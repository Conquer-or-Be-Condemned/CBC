using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ControlUnitStatus : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private int maxPower;
    [SerializeField] private int currentPower;
    [SerializeField] private int maxHealth;
    [SerializeField] private int curHealth;
    
    private GameObject[] units;//현재 가동 중인 타워 배열
    List<GameObject> unitsList = new List<GameObject>();
    
    //  UI와의 Event 연결
    public UnityEvent<int, int> onCUHpChange = new UnityEvent<int, int>();
    public UnityEvent<int, int> onCUPowerChange = new UnityEvent<int, int>();
    
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
        currentPower = currentPower - power;
        onCUPowerChange.Invoke(currentPower, maxPower);
    }

    public void RemoveUnit(int power)
    {
        currentPower = currentPower + power;
        onCUPowerChange.Invoke(currentPower, maxPower);
    }

    public int getCurrentPower()
    {
        return currentPower;
    }

    //  여기부터 UI 테스트용 임시 코드입니다. - 양현석
    private void Die()
    {
        Debug.Log("Control Unit was Destroyed!!!");
        SceneController.ChangeScene("GameOver");
    }

    public void GetDamage(int damage)
    {
        curHealth -= damage;
        onCUHpChange.Invoke(curHealth, maxHealth);
        if (curHealth <= 0)
        {
            Die();
        }
    }

    public void GetRepair(int repair)
    {   
        curHealth += repair;
        onCUHpChange.Invoke(curHealth, maxHealth);
    }
}

