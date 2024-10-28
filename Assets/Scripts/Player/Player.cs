using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

public class Player : MonoBehaviour
{
    [SerializeField] private float playerSpeed;
    
    private Vector3 PlayerPos;//player포지션 x,y,z
    private Animator ani;   //
    private bool isMoving; //플레이어 동작 상태
    private void Awake() //프로그램 시작할때 변수값 초기설정
    {
        
        ani = GetComponent<Animator>();
        isMoving = false;
    }

    void Update()
    {
        isMoving = false;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            setArrowkeyinput(1);
            transform.Translate(Vector3.up * playerSpeed * Time.deltaTime);
            
        }   
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            setArrowkeyinput(2);
            transform.Translate(Vector3.left * playerSpeed * Time.deltaTime);
        }   
        if (Input.GetKey(KeyCode.DownArrow))
        {
            setArrowkeyinput(3);
            transform.Translate(Vector3.down * playerSpeed * Time.deltaTime);
        }   
        if (Input.GetKey(KeyCode.RightArrow))
        {
            setArrowkeyinput(4);
            transform.Translate(Vector3.right * playerSpeed * Time.deltaTime);
        }   


    }

    void setArrowkeyinput(int key)
    {
        ani.SetInteger("state",0);
        ani.speed = 1;
        ani.SetInteger("state",key);
        isMoving = true;
        
        PlayerPos = transform.position;
    }
    private void LateUpdate()
    {
        if (isMoving == false) ani.speed = 0;
    }
    
}
