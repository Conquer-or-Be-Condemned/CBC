using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 0.3f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private int bulletsPerShot; // 한 번에 발사할 총알 수
    [SerializeField] private float spreadAngle = 15f; // 총알 퍼짐 각도
    public Tilemap map;
    public float attackDelay;
    public bool attackable;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rb;
    private Vector2 _mouse;
    private Vector3 sumVector;
    private Transform _mouseTransform;
    
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // 타일맵 객체가 없다면 타일맵 객체를 찾음
        if (map == null)
        {
            map = FindObjectOfType<Tilemap>();
        }

        attackable = true;
    }

    private void FixedUpdate()
    {
        PlayerMove2();
    }

    private void Update()
    {
        // 마우스의 월드 좌표를 가져옴
        _mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        // 방향에 따라 애니메이션 파라미터를 설정
        if (_animator != null)
        {
            CheckDirectionToMouse();
        }
        
        PlayerAttack();
    }
   
    private void PlayerMove2()
    {
        sumVector = Vector3.zero;
        
        if (Input.GetKey(KeyCode.UpArrow)) sumVector += Vector3.up * moveSpeed;
        if (Input.GetKey(KeyCode.LeftArrow)) sumVector += Vector3.left * moveSpeed;
        if (Input.GetKey(KeyCode.DownArrow)) sumVector += Vector3.down * moveSpeed;
        if (Input.GetKey(KeyCode.RightArrow)) sumVector += Vector3.right * moveSpeed;
        
        if (Input.GetKey(KeyCode.W)) sumVector += Vector3.up * moveSpeed;
        if (Input.GetKey(KeyCode.A)) sumVector += Vector3.left * moveSpeed;
        if (Input.GetKey(KeyCode.S)) sumVector += Vector3.down * moveSpeed;
        if (Input.GetKey(KeyCode.D)) sumVector += Vector3.right * moveSpeed;
        
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x + sumVector.x, map.localBounds.min.x + transform.localScale.x / 2, map.localBounds.max.x - transform.localScale.x / 2),
            Mathf.Clamp(transform.position.y + sumVector.y, map.localBounds.min.y + transform.localScale.y / 2, map.localBounds.max.y - transform.localScale.y / 2),
            transform.position.z);
    }

    private void PlayerAttack()
    {
        if (Input.GetKeyDown(KeyCode.Space) && attackable)
        {
            Vector2 baseDirection = (_mouse - (Vector2)bulletSpawnPoint.position).normalized;
            float baseAngle = Mathf.Atan2(baseDirection.y, baseDirection.x) * Mathf.Rad2Deg;

            int bulletsToFire = bulletsPerShot;
            float angleStep = spreadAngle / (bulletsToFire - 1);
            float startAngle = baseAngle - spreadAngle / 2;

            for (int i = 0; i < bulletsToFire; i++)
            {
                float currentAngle = startAngle + angleStep * i;
                float radianAngle = currentAngle * Mathf.Deg2Rad;

                Vector2 bulletDirection = new Vector2(Mathf.Cos(radianAngle), Mathf.Sin(radianAngle));

                GameObject bulletObj = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
                PlayerBullet playerBulletScript = bulletObj.GetComponent<PlayerBullet>();
                playerBulletScript.SetDirection(bulletDirection);
            }
        }
        
        StartCoroutine(PlayerAttackCoroutine());
    }

    private IEnumerator PlayerAttackCoroutine()
    {
        attackable = false;
        yield return new WaitForSeconds(attackDelay);
        attackable = true;
    }

    private void CheckDirectionToMouse()
    {
        // 마우스와 플레이어 사이의 각도 계산
        Vector2 direction = _mouse - (Vector2)transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 각도에 따라 애니메이션 설정
        if (angle > -45 && angle <= 45) // 오른쪽
        {
            SetAnimationState("isSide", true, false, false);
            _spriteRenderer.flipX = true;
        }
        else if (angle > 45 && angle <= 135) // 위쪽
        {
            SetAnimationState("isUp", false, false, true);
            _spriteRenderer.flipX = false;
        }
        else if (angle > -135 && angle <= -45) // 아래쪽
        {
            SetAnimationState("isIdle", false, true, false);
            _spriteRenderer.flipX = false;
        }
        else // 왼쪽
        {
            SetAnimationState("isSide", true, false, false);
            _spriteRenderer.flipX = false;
        }
    }

    // 애니메이션 파라미터 설정 함수
    private void SetAnimationState(string activeState, bool side, bool idle, bool up)
    {
        _animator.SetBool("isSide", side);
        _animator.SetBool("isIdle", idle);
        _animator.SetBool("isUp", up);
    }
}
