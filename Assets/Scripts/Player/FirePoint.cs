using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePoint : MonoBehaviour
{
    private Vector2 _mousePosition; // 마우스 위치 저장 변수
    public Transform playerpoint;

    private Vector3 oriPosition;
    private Vector3 lastPosition; // 마지막 위치 추적 변수

    // Start is called before the first frame update
    void Start()
    {
        oriPosition = transform.position;
        lastPosition = oriPosition; // 초기값 설정
    }

    // Update is called once per frame
    void Update()
    {
        CheckDirectionToMouse();
    }

    private void CheckDirectionToMouse()
    {
        // 마우스 위치를 월드 좌표로 변환
        _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // 현재 위치와 마우스 위치 간의 방향 계산
        Vector2 direction = _mousePosition - (Vector2)playerpoint.position;
        // 방향의 각도를 계산
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 새로운 위치 초기화
        Vector3 newPosition = oriPosition;

        // 각도에 따라 오브젝트 위치 변경
        if (angle > -45 && angle <= 45)
        {
            // 오른쪽
            newPosition = oriPosition + new Vector3(1f, 0, 0);
        }
        else if (angle > 45 && angle <= 135)
        {
            // 위쪽
            newPosition = oriPosition + new Vector3(0, 1f, 0);
        }
        else if (angle > -135 && angle <= -45)
        {
            // 아래쪽
            newPosition = oriPosition + new Vector3(0, -1f, 0);
        }
        else
        {
            // 왼쪽
            newPosition = oriPosition + new Vector3(-1f, 0, 0);
        }

        // 새로운 위치가 마지막 위치와 다를 경우만 이동
        if (newPosition != lastPosition)
        {
            transform.position = newPosition;
            lastPosition = newPosition; // 마지막 위치 갱신
        }
    }
}