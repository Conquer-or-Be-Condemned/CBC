using System.Collections;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    
    [Header("Attributes")] 
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private float bulletDamage = 10f;

    private Vector2 direction;

    // 방향을 설정하는 메서드
    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
        StartCoroutine(destroyObjectIfNotHit());
    }

    private void FixedUpdate()
    {
        if (direction == Vector2.zero) return;

        // Rigidbody2D의 속도를 방향과 속도에 맞게 설정
        rb.velocity = direction * bulletSpeed;

        // 총알의 회전 설정 (방향 벡터 기준)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private IEnumerator destroyObjectIfNotHit()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    // 충돌 시 총알 파괴
    private void OnCollisionEnter2D(Collision2D other)
    {   
        Monster monster = other.gameObject.GetComponent<Monster>();
        if (monster != null)
        {
            monster.TakeDamage(bulletDamage);
        }
        Destroy(gameObject);
    }
}