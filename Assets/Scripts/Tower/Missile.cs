using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;

public class Missile : MonoBehaviour
{
    [Header("References")] [SerializeField]
    Rigidbody2D rb;

    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject explodePrefab;
    [SerializeField] private Transform explosionPosition;
    [Header("Attributes")] [SerializeField]
    private float bulletSpeed = 5f;

    [SerializeField] private float bulletDamage = 10f;
    [SerializeField] private float explosionRange = 5f;

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
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg-90f;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private IEnumerator destroyObjectIfNotHit()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }

    // 충돌 시 총알 파괴
    private void OnCollisionEnter2D(Collision2D other)
    {
        StartCoroutine(DestroyObject());
        //Vector2 missileVector = new Vector2(other.transform.position.x, other.transform.position.y);
        GameObject explosion = Instantiate(explodePrefab, explosionPosition.position, Quaternion.identity);
        Explode ee = explosion.GetComponent<Explode>();
        ee.TriggerExplosion();

        
        
    }
    
    private IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(0.1f);
        Collider2D[] monsters = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), 10f);
        foreach (var monster in monsters)
        {
            if (monster.tag == "Enemy")
            {
                monster.GetComponent<Monster>().TakeDamage(bulletDamage);
            }
        }
        Destroy(gameObject);
    }
    private void OnDrawGizmosSelected()//타워의 반경 그려줌(디버깅용, 인게임에는 안나옴)
    {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, 10f);
    }
}