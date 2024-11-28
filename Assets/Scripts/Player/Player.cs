using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 0.3f;
    [SerializeField] private float pushAmount = 0.02f; // 충돌 시 밀어내는 크기
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private int bulletsPerShot; // 한 번에 발사할 총알 수
    [SerializeField] private float spreadAngle = 15f; // 총알 퍼짐 각도
    [SerializeField] private float fireRate;
    public Tilemap map;
    public float attackDelay;
    public bool attackAble;

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rb;
    private Vector2 _mouse;
    private Vector3 sumVector;
    private Transform _mouseTransform;
    private float _timeTilFire;
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        if (_rb)
        {
            _rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous; // Continuous로 설정
        }
        
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (map == null)
        {
            map = FindObjectOfType<Tilemap>();
        }

        attackAble = true;
    }

    private void FixedUpdate()
    {
        if (!GeneralManager.Instance.inGameManager.isTalking)
        {
            PlayerMove();
        }
    }

    private void PlayerMove()
    {
        sumVector = Vector3.zero;

        // 키 입력 처리
        if (Input.GetKey(KeyCode.UpArrow)) sumVector += Vector3.up * moveSpeed;
        if (Input.GetKey(KeyCode.LeftArrow)) sumVector += Vector3.left * moveSpeed;
        if (Input.GetKey(KeyCode.DownArrow)) sumVector += Vector3.down * moveSpeed;
        if (Input.GetKey(KeyCode.RightArrow)) sumVector += Vector3.right * moveSpeed;
        if (Input.GetKey(KeyCode.W)) sumVector += Vector3.up * moveSpeed;
        if (Input.GetKey(KeyCode.A)) sumVector += Vector3.left * moveSpeed;
        if (Input.GetKey(KeyCode.S)) sumVector += Vector3.down * moveSpeed;
        if (Input.GetKey(KeyCode.D)) sumVector += Vector3.right * moveSpeed;

        // 이동 처리
        transform.position += sumVector * Time.fixedDeltaTime;

        // 경계 내에서 위치 클램프
        Bounds tilemapBounds = map.GetComponent<Renderer>().bounds;
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, tilemapBounds.min.x, tilemapBounds.max.x),
            Mathf.Clamp(transform.position.y, tilemapBounds.min.y, tilemapBounds.max.y),
            transform.position.z
        );
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            // 충돌 방향 계산
            Vector3 pushDirection = collision.contacts[0].normal;

            // 플레이어를 충돌 방향 반대쪽으로 밀어냄
            transform.position += pushDirection * pushAmount;

            Debug.Log($"Collision Enter: Push Direction = {pushDirection}");
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            // 충돌 방향 계산
            Vector3 pushDirection = collision.contacts[0].normal;

            // 충돌 지속 시에도 플레이어를 충돌 반대쪽으로 계속 밀어냄
            transform.position += pushDirection * pushAmount;

            Debug.Log($"Collision Stay: Push Direction = {pushDirection}");
        }
    }

    private void Update()
    {
        // 마우스의 월드 좌표를 가져옴
        _mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // 방향에 따라 애니메이션 파라미터를 설정
        if (_animator != null)
        {

            if (!GeneralManager.Instance.inGameManager.pauseVisible)
            {
                CheckDirectionToMouse();
            }

        }


        if (!GeneralManager.Instance.inGameManager.isTalking)
        {
            PlayerAttack();
        }
    }

    private void PlayerAttack()
    {
        _timeTilFire += Time.deltaTime;
        if (_timeTilFire >= (1f / fireRate))
        {
            if (Input.GetKey(KeyCode.Space))
            {
                Vector2 baseDirection = (_mouse - (Vector2)bulletSpawnPoint.position).normalized;
                float baseAngle = Mathf.Atan2(baseDirection.y, baseDirection.x) * Mathf.Rad2Deg;

                int bulletsToFire = bulletsPerShot;
                float angleStep = spreadAngle / (bulletsToFire - 1);
                float startAngle = baseAngle - spreadAngle / 2;

                for (int i = 0; i < bulletsToFire; i++)
                {
                    Debug.Log("Shot!");
                    float currentAngle = startAngle + angleStep * i;
                    float radianAngle = currentAngle * Mathf.Deg2Rad;

                    Vector2 bulletDirection = new Vector2(Mathf.Cos(radianAngle), Mathf.Sin(radianAngle));

                    GameObject bulletObj = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
                    PlayerBullet playerBulletScript = bulletObj.GetComponent<PlayerBullet>();
                    playerBulletScript.SetDirection(bulletDirection);
                }
                _timeTilFire = 0f;
            }
        }
    }
    // StartCoroutine(PlayerAttackCoroutine());
        
    

    private IEnumerator PlayerAttackCoroutine()
    {
        attackAble = false;
        yield return new WaitForSeconds(attackDelay);
        attackAble = true;
    }

    private void CheckDirectionToMouse()
    {
        Vector2 direction = _mouse - (Vector2)transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (angle > -45 && angle <= 45)
        {
            SetAnimationState("isSide", true, false, false);
            _spriteRenderer.flipX = true;
        }
        else if (angle > 45 && angle <= 135)
        {
            SetAnimationState("isUp", false, false, true);
            _spriteRenderer.flipX = false;
        }
        else if (angle > -135 && angle <= -45)
        {
            SetAnimationState("isIdle", false, true, false);
            _spriteRenderer.flipX = false;
        }
        else
        {
            SetAnimationState("isSide", true, false, false);
            _spriteRenderer.flipX = false;
        }
    }

    private void SetAnimationState(string activeState, bool side, bool idle, bool up)
    {
        _animator.SetBool("isSide", side);
        _animator.SetBool("isIdle", idle);
        _animator.SetBool("isUp", up);
    }
}
