using UnityEngine;

public class Treant : Monster
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private float attackTimer = 0f;
    private float attackCooldown = 2f;
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

    protected override void Start()
    {
        monsterName = "Treant";
        // maxHealth = 150f;
        // attackDamage = 15f;
        // moveSpeed = 3f;
        // attackRange = 1.5f;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        SetDirection(DIRECTION_DOWN);
        base.Start();
    }

    private void Update()
    {
        if (player == null) return;

        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // MonsterMovement 관련 코드 제거

        UpdateState(distanceToPlayer, directionToPlayer);

        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }

        UpdateDebugInfo(directionToPlayer);
    }
    
    
    private void UpdateState(float distanceToPlayer, Vector3 directionToPlayer)
    {
        // 방향 업데이트
        UpdateDirection(directionToPlayer);

        // 상태 업데이트
        if (distanceToPlayer <= attackRange)
        {
            SetMoving(false);
            if (attackTimer <= 0 && !isAttacking)
            {
                StartAttack(directionToPlayer);
            }
        }
        else
        {
            SetMoving(true);
            if (!isAttacking)
            {
                Move();
            }
        }

        // 애니메이션 상태 업데이트
        UpdateAnimationState();
    }

    private void UpdateDirection(Vector3 moveDirection)
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
        
        if (debugTimer < 0.1f)
        {
            // Debug.Log($"Direction changed to: {GetDirectionName(direction)}");
        }
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

    protected override void Move()
    {
        if (player == null || isAttacking) return;

        Vector3 direction = (player.position - transform.position).normalized;
        Vector3 movement = direction * (moveSpeed * Time.deltaTime);
        transform.position += movement;
    }

    private void StartAttack(Vector3 attackDirection)
    {
        isAttacking = true;
        SetMoving(false);
        attackTimer = attackCooldown;
        
        UpdateAnimationState();
        
        // 플레이어에게 데미지
        PlayerInfo playerInfo = player.GetComponent<PlayerInfo>();
        if (playerInfo != null)
        {
            playerInfo.TakeDamage((int)attackDamage);
        }

        Invoke(nameof(FinishAttack), 1f);
    }

    private void FinishAttack()
    {
        isAttacking = false;
        UpdateAnimationState();
        // Debug.Log("Attack Finished");
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

    private void UpdateDebugInfo(Vector3 moveDirection)
    {
        debugTimer += Time.deltaTime;
        
        if (debugTimer >= debugInterval)
        {
            // Debug.Log($"Position - Monster: {transform.position}, Player: {player.position}");
            // Debug.Log($"Movement Vector3: ({moveDirection.x}, {moveDirection.y}, {moveDirection.z})");
            // Debug.Log($"Current State - Direction: {GetDirectionName(currentDirection)}, Moving: {isMoving}, Attacking: {isAttacking}");
            debugTimer = 0f;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        
        if (player != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, player.position);
        }
    }
}