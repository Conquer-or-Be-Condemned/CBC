using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInfo : MonoBehaviour
{
    public int curHp;
    public int maxHp = 100;
    
    public UnityEvent onDeath = new UnityEvent();
    public UnityEvent<int,int> onHpChange = new UnityEvent<int, int>();

    private void Awake()
    {
        curHp = maxHp;
    }

    private void Start()
    {

    }

    private void FixedUpdate()
    {
        onHpChange.Invoke(curHp, maxHp);
    }

    public void Die()
    {
        Debug.Log("Player Die");
        
        //  SceneController를 사용할 때는 반드시 Build Setting 확인
        SceneController.ChangeScene("GameOver");
    }

    public void TakeDamage(int damage)
    {
        curHp -= damage;
        
        if (curHp <= 0)
        {
            Invoke("Die", 1f);
        }
    }

    public void RecoverHp()
    {
        curHp = maxHp;
    }
    
    
}
