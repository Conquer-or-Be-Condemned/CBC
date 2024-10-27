using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float smoothing = 0.2f;
    private void FixedUpdate()
    {
        if (player== null)
        {
            Debug.Log("Player Object is NULL");
        }
        
        Vector3 targetPos = new Vector3(player.transform.position.x, player.transform.position.y, this.transform.position.z);
        
        //  Lerp는 선형 보간 -> 부드러운 움직임 구현
        transform.position = Vector3.Lerp(transform.position, targetPos, smoothing);
    }
}
