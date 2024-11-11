using UnityEngine;

public class Treant : Monster
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private float attackTimer = 0f;
    private float attackCooldown = 1f;
    private bool isAttacking = false;
    private bool isMoving = false;
    private float debugTimer = 0f;
    private float debugInterval = 0.5f;
    private int currentDirection = 0;

    // 방향 상수 정의
    private const int DIRECTION_DOWN = 0;
    private const int DIRECTION_UP = 1;
    private const int DIRECTION_LEFT = 2;
    private const int DIRECTION_RIGHT = 3;

    // 현재 타겟을 추적하기 위한 변수
    private Transform currentTarget;

    protected override void Start()
    {
        base.Start();

        monsterName = "Treant";

        // 필요한 스탯 설정
        if (maxHealth == 0) maxHealth = 150f;
        if (attackDamage == 0) attackDamage = 15f;
        if (moveSpeed == 0) moveSpeed = 3f;
        if (attackRange == 0) attackRange = 1.5f;
        if (detectionRange == 0) detectionRange = 5f; // 기본값 설정

        // Treant의 체력바 Y 오프셋 설정
        healthBarYOffset = 9f;

        // Animator 및 SpriteRenderer 설정
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        SetDirection(DIRECTION_DOWN);

        // ControlUnitStatus가 Monster.cs에서 자동 할당되었는지 확인
        if (controlUnitStatus == null)
        {
            Debug.LogWarning($"{monsterName}의 Control Unit이 할당되지 않았습니다.");
        }
    }

    private void Update()
    {
        if (player == null || controlUnitStatus == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        float distanceToControl = Vector2.Distance(transform.position, controlUnitStatus.transform.position);

        // 현재 타겟 설정
        if (distanceToPlayer <= detectionRange) {
            Debug.LogWarning(detectionRange);
            currentTarget = player;
        } else  {
            Debug.LogWarning(detectionRange);
            currentTarget = controlUnitStatus.transform;
        }

        Vector2 directionToTarget = (currentTarget.position - transform.position).normalized;
        float distanceToTarget = Vector2.Distance(transform.position, currentTarget.position);

        UpdateState(distanceToTarget, directionToTarget);

        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }

        UpdateDebugInfo(directionToTarget);
    }

    private void UpdateState(float distanceToTarget, Vector2 directionToTarget)
    {
        // 방향 업데이트
        UpdateDirection(directionToTarget);

        // 상태 업데이트
        if (distanceToTarget <= attackRange)
        {
            SetMoving(false);
            if (attackTimer <= 0 && !isAttacking)
            {
                StartAttack(directionToTarget);
            }
        }
        else
        {
            SetMoving(true);
            if (!isAttacking)
            {
                MoveTowards(currentTarget.position);
            }
        }

        // 애니메이션 상태 업데이트
        UpdateAnimationState();
    }

    private void UpdateDirection(Vector2 moveDirection)
    {
        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;

        int newDirection;
        if (angle > -45 && angle <= 45) // 오른쪽
        {
            newDirection = DIRECTION_RIGHT;
        }
        else if (angle > 45 && angle <= 135) // 위
        {
            newDirection = DIRECTION_UP;
        }
        else if (angle > -135 && angle <= -45) // 아래
        {
            newDirection = DIRECTION_DOWN;
        }
        else // 왼쪽
        {
            newDirection = DIRECTION_LEFT;
        }

        if (currentDirection != newDirection)
        {
            SetDirection(newDirection);
        }
    }

    private void SetDirection(int direction)
    {
        currentDirection = direction;
        animator.SetInteger("direction", direction);
    }

    private void SetMoving(bool moving)
    {
        isMoving = moving;
        animator.SetBool("isMoving", moving && !isAttacking);
    }

    private void UpdateAnimationState()
    {
        animator.SetBool("isMoving", isMoving && !isAttacking);
        animator.SetBool("isAttacking", isAttacking);
    }

    private void Move(Vector2 targetPosition)
    {
        base.Move(targetPosition);
    }

    private void MoveTowards(Vector2 targetPosition)
    {
        Move(targetPosition);
    }

    private void StartAttack(Vector2 attackDirection)
    {
        isAttacking = true;
        SetMoving(false);
        attackTimer = attackCooldown;

        UpdateAnimationState();

        // 플레이어 또는 제어 장치에게 데미지
        if (currentTarget.CompareTag("Player"))
        {
            PlayerInfo playerInfo = player.GetComponent<PlayerInfo>();
            if (playerInfo != null)
            {
                playerInfo.TakeDamage((int)attackDamage);
            }
        }
        else if (currentTarget.CompareTag("CU"))
        {
            ControlUnitStatus controlUnit = controlUnitStatus;
            if (controlUnit != null)
            {
                controlUnit.GetDamage((int)attackDamage);
            }
        }

        Invoke(nameof(FinishAttack), 1f);
    }

    private void FinishAttack()
    {
        isAttacking = false;
        UpdateAnimationState();
    }

    private string GetDirectionName(int direction)
    {
        return direction switch
        {
            DIRECTION_DOWN => "Down",
            DIRECTION_UP => "Up",
            DIRECTION_LEFT => "Left",
            DIRECTION_RIGHT => "Right",
            _ => "Unknown"
        };
    }

    private void UpdateDebugInfo(Vector2 moveDirection)
    {
        debugTimer += Time.deltaTime;

        if (debugTimer >= debugInterval)
        {
            // 디버그 정보 출력 (필요 시 주석 해제)
            // Debug.Log($"Position - Monster: {transform.position}, Player: {player.position}, ControlUnit: {controlUnitStatus.transform.position}");
            // Debug.Log($"Movement Vector2: ({moveDirection.x}, {moveDirection.y}, {moveDirection.z})");
            // Debug.Log($"Current State - Direction: {GetDirectionName(currentDirection)}, Moving: {isMoving}, Attacking: {isAttacking}");
            debugTimer = 0f;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        if (player != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, player.position);
        }

        if (controlUnitStatus != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, controlUnitStatus.transform.position);
        }
    }
}