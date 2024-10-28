using UnityEngine;

public class MonsterMovement : MonoBehaviour
{
    private Vector2 moveDirection;
    private float moveSpeed;

    public void Initialize(Vector2 direction, float speed)
    {
        moveDirection = direction;
        moveSpeed = speed;
    }

    private void Update()
    {
        // 설정된 방향으로 이동
        transform.position += (Vector3)(moveDirection * moveSpeed * Time.deltaTime);
    }
}