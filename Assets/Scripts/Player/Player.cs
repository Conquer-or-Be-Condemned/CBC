using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Debug = System.Diagnostics.Debug;

public class Player : MonoBehaviour
{
    public Tilemap map;
    public Animator ani;
    
    [SerializeField] private float moveSpeed = 0.3f;

    private bool _isMoving;

    private void Start()
    {
        _isMoving = false;

        //  타일맵 객체가 없다면 타일맵 객체를 찾는다.
        if (map == null)
        {
            map = FindObjectOfType<Tilemap>();
        }
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
        //  반드시 0으로 초기화 시켜야 오류가 생기지 않음
        Vector3 sumVector = new Vector3(0,0,0);
        
        if (Input.GetKey(KeyCode.UpArrow))
        {
            setArrowkeyinput(1);
            sumVector += Vector3.up * moveSpeed;
        }   
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            setArrowkeyinput(1);
            sumVector += Vector3.left * moveSpeed;
        }   
        if (Input.GetKey(KeyCode.DownArrow))
        {
            setArrowkeyinput(1);
            sumVector += Vector3.down * moveSpeed;

        }   
        if (Input.GetKey(KeyCode.RightArrow))
        {
            setArrowkeyinput(1);
            sumVector += Vector3.right * moveSpeed;
        }
    
        //  조금 복잡하긴 하지만, collider를 사용하지 않고 플레이어가 맵 밖으로 나가지 않도록
        //  설정할 수 있는 최선의 방법 - 양현석
        transform.position = new Vector3(Mathf.Clamp(transform.position.x+sumVector.x, 
                map.localBounds.min.x + transform.localScale.x / 2,
                map.localBounds.max.x - transform.localScale.x / 2),
            Mathf.Clamp(transform.position.y + sumVector.y,
                map.localBounds.min.y + transform.localScale.y / 2,
                map.localBounds.max.y - transform.localScale.y / 2)
            , transform.position.z);
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
