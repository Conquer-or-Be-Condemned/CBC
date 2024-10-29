using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Rigidbody2D rb;
    

    [Header("Attributes")] 
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private float bulletDamage = 10f;
    private Transform _target;
    
    // 타겟을 설정하는 메서드
    public void SetTarget(Transform target)
    {
        this._target = target;
        StartCoroutine(destroyObjectIfNotHit());
    }

    private void FixedUpdate()
    {
        if (!_target) return; // 타겟이 없으면 아무 것도 하지 않음

        // 타겟을 향하는 방향 벡터 계산
        Vector2 direction = (_target.position - transform.position).normalized;

        // Rigidbody2D의 속도를 방향과 속도에 맞게 설정
        rb.velocity = direction * bulletSpeed;

        // 총알의 회전 설정 (방향 벡터 기준)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private IEnumerator destroyObjectIfNotHit()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
    // 충돌 시 총알 파괴
    private void OnCollisionEnter2D(Collision2D other)
    {   
        Monster monster = other.gameObject.GetComponent<Monster>();
        monster.TakeDamage(bulletDamage);
        Destroy(gameObject);
    }
}