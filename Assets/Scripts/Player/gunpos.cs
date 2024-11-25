using UnityEngine;

public class CopyPosition : MonoBehaviour
{
    public GameObject targetObject; // 위치를 복사할 오브젝트

    void Update()
    {
        if (targetObject != null)
        {
            // 현재 오브젝트의 위치를 타겟 오브젝트의 위치로 설정
            transform.position = targetObject.transform.position;
        }
    }
}