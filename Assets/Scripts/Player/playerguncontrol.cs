using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerguncontrol : MonoBehaviour
{
    public GameObject gunside;  // gunside 오브젝트 참조
    public GameObject gunup;    // gunup 오브젝트 참조
    public GameObject gundown;  // gundown 오브젝트 참조
    public Transform playerpoint; // 중심점이 되는 player 좌표
    private Vector2 _mousePosition; // 마우스 위치 저장 변수
    private SpriteRenderer gunsideRenderer; // gunside의 SpriteRenderer

    private bool gunSideLR;
    // Start is called before the first frame update
    void Start()
    {
        gunSideLR = false;
        // gunside의 SpriteRenderer를 가져오기
        if (gunside != null)
        {
            gunsideRenderer = gunside.GetComponent<SpriteRenderer>();
        }

        // 기본적으로 모든 오브젝트를 비활성화
        gunside.SetActive(false);
        gunup.SetActive(false);
        gundown.SetActive(true); // 기본 상태로 gundown 활성화
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
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 각도에 따라 오브젝트 활성화
        if (angle > -45 && angle <= 45)
        {
            // 마우스가 오른쪽 방향에 있을 때 gunside 활성화
            ActivateGun(gunside);

            // gunside의 방향을 오른쪽으로 설정
            if (gunsideRenderer != null)
            {
                gunsideRenderer.flipX = true; // 오른쪽일 때 뒤집지 않음
            }
        }
        else if (angle > 45 && angle <= 135)
        {
            // 마우스가 위쪽 방향에 있을 때 gunup 활성화
            ActivateGun(gunup);
        }
        else if (angle > -135 && angle <= -45)
        {
            // 마우스가 아래쪽 방향에 있을 때 gundown 활성화
            ActivateGun(gundown);
        }
        else
        {
            // 마우스가 왼쪽 방향에 있을 때 gunside 활성화
            ActivateGun(gunside);

            // gunside의 방향을 왼쪽으로 설정
            if (gunsideRenderer != null)
            {
                gunsideRenderer.flipX = false; // 왼쪽일 때 뒤집기
            }

            if (gunSideLR == false) {
                // 현재 위치를 가져와서 x 값만 수정한 뒤 다시 position에 할당
                Vector3 newPosition = playerpoint.transform.position;
                newPosition.x = newPosition.x + 2; // x 값을 2 증가
                playerpoint.transform.position = newPosition; // 수정된 위치를 다시 할당
                gunSideLR = true;
            }
        }
    }

    // 특정 오브젝트를 활성화하고 나머지는 비활성화하는 함수
    void ActivateGun(GameObject activeGun)
    {
        gunside.SetActive(activeGun == gunside);
        gunup.SetActive(activeGun == gunup);
        gundown.SetActive(activeGun == gundown);
    }

    // 특정 오브젝트의 위치를 중심점(playerpoint)을 기준으로 반대쪽으로 설정
    
}
