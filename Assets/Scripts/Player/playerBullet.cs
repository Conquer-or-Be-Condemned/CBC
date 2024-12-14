using System;
using System.Collections;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    
    [Header("Attributes")] 
    [SerializeField] private float bulletSpeed = 6f;
    [SerializeField] private float bulletDamage = 13f;

    private Vector2 _direction;

    private Vector2 _playerSpeed;
    // 방향을 설정하는 메서드
    public void SetDirection(Vector2 dir,Vector2 playerSpeed)
    {
        _direction = dir.normalized;
        _playerSpeed = playerSpeed;
        StartCoroutine(destroyObjectIfNotHit());
    }

  

    private void FixedUpdate()
    {
        if (_direction == Vector2.zero) return;
        // Debug.Log(rb.velocity);
        // Rigidbody2D의 속도를 방향과 속도에 맞게 설정
        // Debug.Log(_playerSpeed.magnitude);
        rb.velocity = _direction * (bulletSpeed + _playerSpeed.magnitude);
        // rb.velocity = _direction * bulletSpeed;
        // Debug.Log(rb.velocity);

        // 총알의 회전 설정 (방향 벡터 기준)
        float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private IEnumerator destroyObjectIfNotHit()
    {
        yield return new WaitForSeconds(2f);
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