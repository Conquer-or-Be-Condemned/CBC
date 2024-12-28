using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOutlineController : MonoBehaviour
{
    public Camera mainCamera; // 메인 카메라
    public Camera minimapCamera; // 미니맵 카메라

    public Color outlineColor; // 외곽선 색상
    public float outlineWidth = 0.1f; // 외곽선 두께

    private LineRenderer lineRenderer; // 외곽선을 그리는 LineRenderer

    void Start()
    {
        // LineRenderer 설정
        SetupLineRenderer();
    }

    void Update()
    {
        if (mainCamera != null)
        {
            // 메인 카메라의 Viewport를 기준으로 외곽선 업데이트
            UpdateOutline();
        }
    }

    private void SetupLineRenderer()
    {
        // LineRenderer 추가 및 초기화
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = true;
        lineRenderer.startWidth = outlineWidth;
        lineRenderer.endWidth = outlineWidth;
        lineRenderer.loop = true;
        lineRenderer.startColor = outlineColor;
        lineRenderer.endColor = outlineColor;
        lineRenderer.sortingLayerName = "MiniMap"; // 또는 원하는 Sorting Layer 이름
        lineRenderer.sortingOrder = 10;           // 레이어 순서를 높게 설정 (다른 오브젝트 위로)
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));

    }

    private void UpdateOutline()
    {
        if (mainCamera == null)
        {
            // Debug.LogWarning("메인 카메라가 설정되지 않았습니다.");
            return;
        }

        // 메인 카메라의 Viewport 코너 좌표 가져오기
        Vector3[] corners = new Vector3[4];
        corners[0] = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane)); // 좌하단
        corners[1] = mainCamera.ViewportToWorldPoint(new Vector3(0, 1, mainCamera.nearClipPlane)); // 좌상단
        corners[2] = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, mainCamera.nearClipPlane)); // 우상단
        corners[3] = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, mainCamera.nearClipPlane)); // 우하단

        // LineRenderer에 외곽선 좌표 설정
        lineRenderer.positionCount = corners.Length;
        for (int i = 0; i < corners.Length; i++)
        {
            lineRenderer.SetPosition(i, corners[i]); // 월드 좌표 그대로 사용
        }

        // LineRenderer를 닫아서 외곽선을 완성
        lineRenderer.loop = true;
    }
}



