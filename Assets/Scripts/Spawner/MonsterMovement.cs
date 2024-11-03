// using UnityEngine;
//
// public class MonsterMovement : MonoBehaviour
// {
//     private Vector3 moveDirection;
//     private float moveSpeed;
//
//     public void Initialize(Vector2 direction, float speed)
//     {
//         moveDirection = direction;
//         moveSpeed = speed;
//     }
//
//     private void Update()
//     {
//         // 설정된 방향으로 이동
//         transform.position += (moveDirection * moveSpeed * Time.deltaTime);
//     }
// }