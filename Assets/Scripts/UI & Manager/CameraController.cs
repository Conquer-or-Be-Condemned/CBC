using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraController : MonoBehaviour
{
    //  타겟 지정
    public GameObject target;
    public Tilemap map;
    
    [SerializeField] private float smoothSpeed = 0.125f;
    
    private float _cameraHalfHeight;
    private float _cameraHalfWidth;

    private void Awake()
    {
        //  카메라의 size를 초기화 
        gameObject.GetComponent<Camera>().orthographicSize = 8;
    }
    private void Start()
    {
        if (target == null)
        {
            //  tag로 재 검색
            target = GameObject.FindGameObjectWithTag("Player");
        }
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
        Vector3 desiredPosition = new Vector3(
            Mathf.Clamp(target.transform.position.x,  map.localBounds.min.x+ _cameraHalfWidth, map.localBounds.max.x - _cameraHalfWidth),
            Mathf.Clamp(target.transform.position.y, map.localBounds.min.y + _cameraHalfHeight, map.localBounds.max.y - _cameraHalfHeight),
            transform.position.z);
        
        //  부드러운 움직임
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
    }
}
