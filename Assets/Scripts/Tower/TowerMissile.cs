using UnityEngine;
using System.Collections;
using UnityEditor;

public class TowerMissile : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject explodePrefab;
    [SerializeField] private Transform explosionPosition;

    [Header("Movement Settings")] 
    [SerializeField] private float initialSpeed = 10f;     // 초기 직진 속도
    [SerializeField] private float maxSpeed = 15f;         // 최대 속도
    [SerializeField] private float turnSpeed = 180f;       // 회전 속도 (도/초)
    [SerializeField] private float acceleration = 5f;      // 가속도
    [SerializeField] private float initialStraightTime =2f; // 초기 직진 시간
    
    [Header("Combat Settings")]
    [SerializeField] private float bulletDamage = 10f;
    [SerializeField] private float explosionRange = 5f;
    
    private Transform _target;
    private bool _isHoming = false;
    private Vector2 _initialDirection;
    private float _currentSpeed;

    public void SetTarget(Transform target)
    {
        _target = target;
        _initialDirection = transform.up;
        _currentSpeed = initialSpeed;
        rb.velocity = _initialDirection * _currentSpeed;
        
        StartCoroutine(InitialStraightMovement());
        StartCoroutine(ExplodeMissileIfNotHit());
    }

    private IEnumerator InitialStraightMovement()
    {
        yield return new WaitForSeconds(initialStraightTime);
        _isHoming = true;
    }

    private void FixedUpdate()
    {
        if(_target != null)
            Debug.DrawLine(transform.position, _target.position, Color.blue, 0.1f);
        if (_target == null)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 100);
            foreach (var monster in hits)
            {
                if (monster.CompareTag("Enemy"))
                {
                    if(_target==null)_target = monster.transform;
                    else return;
                }
            }
        }
        if (!_isHoming) return;

        // 현재 진행 방향과 목표 방향 사이의 각도 계산
        Vector2 directionToTarget = ((Vector2)_target.position - rb.position).normalized;
        Vector2 currentDirection = rb.velocity.normalized;
        
        // 회전 방향 결정 (양수: 시계방향, 음수: 반시계방향)
        float angleToTarget = Vector2.SignedAngle(currentDirection, directionToTarget);
        
        // 한 프레임당 최대 회전 각도 제한
        float rotationThisFrame = Mathf.Clamp(angleToTarget, -turnSpeed * Time.fixedDeltaTime, turnSpeed * Time.fixedDeltaTime);
        
        // 현재 속도 벡터를 회전
        Vector2 newDirection = Quaternion.Euler(0, 0, rotationThisFrame) * currentDirection;
        
        // 가속도 적용
        _currentSpeed = Mathf.MoveTowards(_currentSpeed, maxSpeed, acceleration * Time.fixedDeltaTime);
        
        // 새로운 속도 적용
        rb.velocity = newDirection * _currentSpeed;
        
        // 미사일 스프라이트 회전
        float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }


    // ... (나머지 코드는 동일)

    // 벡터를 특정 각도만큼 회전시키는 헬퍼 함수
    private Vector2 RotateVector2(Vector2 vector, float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        float cos = Mathf.Cos(radian);
        float sin = Mathf.Sin(radian);
        
        return new Vector2(
            vector.x * cos - vector.y * sin,
            vector.x * sin + vector.y * cos
        );
    }

    private IEnumerator ExplodeMissileIfNotHit()
    {
        yield return new WaitForSeconds(10f);
        StartCoroutine(DestroyObject());
        GameObject explosion = Instantiate(explodePrefab, explosionPosition.position, Quaternion.identity);
        explosion.GetComponent<Explode>().TriggerExplosion();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        StartCoroutine(DestroyObject());
        GameObject explosion = Instantiate(explodePrefab, explosionPosition.position, Quaternion.identity);
        explosion.GetComponent<Explode>().TriggerExplosion();
    }

    private IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(0.1f);
        Collider2D[] monsters = Physics2D.OverlapCircleAll(rb.position, explosionRange);
        foreach (var monster in monsters)
        {
            if (monster.CompareTag("Enemy"))
            {
                monster.GetComponent<Monster>().TakeDamage(bulletDamage);
            }
        }
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, explosionRange);
    }
    
    
    
}