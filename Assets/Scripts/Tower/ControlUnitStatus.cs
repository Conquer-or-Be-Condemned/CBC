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
    
    // 몬스터가 공격할 제어 장치 접근 포인트들
    [Header("Access Points")]
    public Transform[] accessPoints; 

    //  UI와의 Event 연결
    public UnityEvent<int, int> onCUHpChange = new UnityEvent<int, int>();
    public UnityEvent<int, int> onCUPowerChange = new UnityEvent<int, int>();

    private bool attackCool;

    private void Start()
    {
        attackCool = false;
        
        ValidateData();
    }

    private void ValidateData()
    {
        maxHealth = DataManager.ControlUnitHp;
        curHealth = DataManager.ControlUnitHp;

        maxPower = DataManager.ControlUnitPower;
        currentPower = DataManager.ControlUnitPower;
    }

    public void AddUnit(int power)
    {
        currentPower = currentPower - power;
        onCUPowerChange.Invoke(currentPower, maxPower);
    }

    public void RemoveUnit(int power)
    {
        //  반드시 이 Method를 거쳐야 합니다. (천천히 파워가 올라감)
        RecoverPower(power);
        onCUPowerChange.Invoke(currentPower, maxPower);
    }

    public int GetCurrentPower()
    {
        return currentPower;
    }
    
    private void Die()
    {
        Debug.Log("Control Unit was Destroyed!!!");
        
        GeneralManager.Instance.inGameManager.GameOver();
    }

    public void GetDamage(int damage)
    {
        //  계속 보여주지 않기 위함
        if (!attackCool)
        {
            GeneralManager.Instance.alertManager.Show(4);
            StartCoroutine(AttackCoolCoroutine());
        }
        
        curHealth -= damage;
        onCUHpChange.Invoke(curHealth, maxHealth);
        
        if (curHealth <= 0)
        {
            Invoke("Die", 1f);
        }
    }

    private IEnumerator AttackCoolCoroutine()
    {
        attackCool = true;
        yield return new WaitForSeconds(10f);
        attackCool = false;
    }

    //  사용하지 않음.
    // public void GetRepair(int repair)
    // {   
    //     curHealth += repair;
    //     onCUHpChange.Invoke(curHealth, maxHealth);
    // }

    public int GetMaxHp()
    {
        return maxHealth;
    }
    public int GetCurHp()
    {
        return curHealth;
    }
    public int GetMaxPower()
    {
        return maxPower;
    }
    public int GetCurPower()
    {
        return currentPower;
    }

    public bool CheckEnoughPower(int offset)
    {
        int diff = currentPower - offset;

        return (diff >= 0 ? true : false);
    }

    //  파워 회복량, 속도
    [SerializeField] private int powerOffset = 5;
    [SerializeField] private float recoverSpeed = 0.5f;

    private void RecoverPower(int power)
    {
        StartCoroutine(RecoverCoroutine(power));
    }

    private IEnumerator RecoverCoroutine(int power)
    {
        //  tmp는 혹시 몰라 만들어 놓는다.(실제 회복량은 다를 수 있기에 복원을 위해)
        int tmp = power;
        while (true)
        {
            if (tmp <= 0) yield break;
            
            tmp--;
            currentPower += powerOffset;
            
            onCUPowerChange.Invoke(currentPower,maxPower);
            yield return new WaitForSeconds(recoverSpeed);
        }
    }
}

