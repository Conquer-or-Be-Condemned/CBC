using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class testMove : MonoBehaviour
{

    public Tilemap map;
    public Animator ani;
    
    [SerializeField] private float moveSpeed = 0.3f;

    private bool _isMoving;

    private void Start()
    {
        _isMoving = false;
    }

    private void FixedUpdate()
    {
        PlayerMove2();
    }
    
    private void LateUpdate()
    {
        //  Animator가 없는 경우를 상정 
        if (_isMoving == false && ani != null) ani.speed = 0;
    }

    //  살짝 Lerp되며 움직이는 경향이 있음(사용 안 함)
    private void PlayerMove()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");
        
        Vector3 tmp = new Vector3(
            Mathf.Clamp(transform.position.x + horizontal *moveSpeed,  map.localBounds.min.x+ transform.localScale.x/2, map.localBounds.max.x - transform.localScale.x/2),
            Mathf.Clamp(transform.position.y + vertical * moveSpeed, map.localBounds.min.y + transform.localScale.y/2, map.localBounds.max.y - transform.localScale.y/2),
            transform.position.z);

        transform.position = tmp;
    }

    //  정상적으로 움직이기에 이를 채택
    //  또한 대각선 이동을 허용하도록 else if 사용하지 않음
    private void PlayerMove2()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            setArrowkeyinput(1);
            transform.Translate(Vector3.up * moveSpeed);
        }   
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            setArrowkeyinput(1);
            transform.Translate(Vector3.left * moveSpeed);
        }   
        if (Input.GetKey(KeyCode.DownArrow))
        {
            setArrowkeyinput(1);
            transform.Translate(Vector3.down * moveSpeed);
        }   
        if (Input.GetKey(KeyCode.RightArrow))
        {
            setArrowkeyinput(1);
            transform.Translate(Vector3.right * moveSpeed);
        }
    }
    
    private void setArrowkeyinput(int key)
    {
        //  애니메이션이 구현되지 않았을 경우를 가정하여
        if (ani != null)
        {
            ani.SetInteger("state",0);
            ani.speed = 1;
            ani.SetInteger("state",key);
        }
        _isMoving = true;
    }
}
