using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.Tilemaps;

/*
 *  Main Camera의 전반적인 부분을 다루는 스크립트입니다.
 *  인스펙터의 Camera를 만지기보다는 스크립트의 내용을 바꾸어 주세요.
 */
public class CameraController : MonoBehaviour
{
    //  타겟 지정
    public GameObject target;
    public Tilemap map;
    
    //  카메라의 부드러움의 정도
    [SerializeField] private float smoothSpeed = 0.125f;
    
    private float _cameraHalfHeight;
    private float _cameraHalfWidth;

    private void Awake()
    {
        //  카메라의 size를 초기화 
        gameObject.GetComponent<Camera>().orthographicSize = 18f;
        
        //  항상 맵의 크기가 최적화되도록 추가한 코드
        map.CompressBounds();
    }
    
    private void Start()
    {
        if (target == null)
        {
            //  tag로 재 검색
            target = GameObject.FindGameObjectWithTag("Player");
        }
        
        //  카메라의 가로 세로의 절반 계산
        _cameraHalfHeight = Camera.main.orthographicSize;
        _cameraHalfWidth = Camera.main.aspect * Camera.main.orthographicSize;
    }

    private void FixedUpdate()
    {
        CameraMove();
    }
    
    private void CameraMove()
    {
        //  영역 지정
        //  Clamp는 구간 내의 범위에서만 수를 허용하는 함수
        //  Tilemap의 지정범위 내에서만 작동하도록 합니다.
        //  Scale 값이 조정되어 인식을 하지 못하므로 Bounds를 이용합니다.
        
        Bounds tilemapBounds = map.GetComponent<Renderer>().bounds;
        
        Vector3 desiredPosition = new Vector3(
            Mathf.Clamp(target.transform.position.x, tilemapBounds.min.x + _cameraHalfWidth, tilemapBounds.max.x - _cameraHalfWidth),
            Mathf.Clamp(target.transform.position.y, tilemapBounds.min.y + _cameraHalfHeight, tilemapBounds.max.y - _cameraHalfHeight),
            transform.position.z
        );
        
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
    }

}
