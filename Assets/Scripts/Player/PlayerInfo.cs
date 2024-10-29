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
    public UnityEvent onHpChange = new UnityEvent();

    private void Awake()
    {
        curHp = maxHp;
    }

    private void Start()
    {
        
    }

    private void FixedUpdate()
    {
        
    }

    public void Die()
    {
        Debug.Log("Player Die");
        SceneController.ChangeScene("GameOver");
    }

    public void TakeDamage(int damage)
    {
        curHp -= damage;
        onHpChange.Invoke();
        if (curHp <= 0)
        {
            Invoke("Die", 3f);
        }
    }

    public void RecoverHp()
    {
        curHp = maxHp;
    }
    
    
}
