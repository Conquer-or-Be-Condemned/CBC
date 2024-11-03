using UnityEngine;

public class Treant : Monster
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private float attackTimer = 0f;
    private float attackCooldown = 2f;
    private bool isAttacking = false;
    private float debugTimer = 0f;
    private float debugInterval = 0.5f;
    private MonsterMovement movement;

    protected override void Start()
    {
        monsterName = "Treant";
        maxHealth = 150f;
        attackDamage = 15f;
        moveSpeed = 3f;
        attackRange = 1.5f;
        
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        movement = GetComponent<MonsterMovement>();
        
        base.Start();
    }

    private void Update()
    {
        if (player == null) return;

        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // MonsterMovement가 있다면 비활성화
        if (movement != null)
        {
            movement.enabled = false;
        }

        // 이동 및 방향 업데이트
        if (distanceToPlayer <= attackRange)
        {
            if (attackTimer <= 0 && !isAttacking)
            {
                // StartAttack(directionToPlayer);
            }
            UpdateAnimation(directionToPlayer, true);
        }
        else
        {
            if (!isAttacking)
            {
                Move();
                UpdateAnimation(directionToPlayer, false);
            }
        }

        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }
    }

    private void UpdateAnimation(Vector3 moveDirection, bool inAttackRange)
    {
        debugTimer += Time.deltaTime;
        
        animator.SetBool("isMoving", !isAttacking && !inAttackRange);
        animator.SetBool("isAttacking", isAttacking);

        // 방향 결정
        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        
        if (debugTimer >= debugInterval)
        {
            Debug.Log($"Position - Monster: {transform.position}, Player: {player.position}");
            Debug.Log($"Movement Vector3: ({moveDirection.x}, {moveDirection.y}, {moveDirection.z})");
            Debug.Log($"Calculated Angle: {angle}");
            debugTimer = 0f;
        }

        int direction;
        string directionName;

        // 각도를 기반으로 방향 설정
        if (angle > -45 && angle <= 45) // 오른쪽
        {
            direction = 3;
            directionName = "Right";
        }
        else if (angle > 45 && angle <= 135) // 위
        {
            direction = 1;
            directionName = "Up";
        }
        else if (angle > -135 && angle <= -45) // 아래
        {
            direction = 0;
            directionName = "Down";
        }
        else // 왼쪽
        {
            direction = 2;
            directionName = "Left";
        }

        if (debugTimer < 0.1f) // 디버그 정보는 위의 주기와 별개로 방향 변경시에만 출력
        {
            Debug.Log($"Current Direction: {directionName} (Value: {direction})");
            Debug.Log($"Animation States - Moving: {!isAttacking && !inAttackRange}, Attacking: {isAttacking}");
        }
        
        animator.SetInteger("direction", direction);

        // 좌우 스프라이트 반전
        if (moveDirection.x != 0)
        {
            spriteRenderer.flipX = (moveDirection.x < 0);
        }
    }

    protected override void Move()
    {
        if (player == null || isAttacking) return;

        Vector3 direction = (player.position - transform.position).normalized;
        Vector3 movement = direction * (moveSpeed * Time.deltaTime);
        
        if (debugTimer >= debugInterval)
        {
            Debug.Log($"Move Vector3: ({movement.x}, {movement.y}, {movement.z})");
        }
        
        transform.position += movement;
    }

    // private void StartAttack(Vector3 attackDirection)
    // {
    //     isAttacking = true;
    //     attackTimer = attackCooldown;
    //     
    //     if (debugTimer >= debugInterval)
    //     {
    //         Debug.Log($"Starting Attack in direction: ({attackDirection.x}, {attackDirection.y}, {attackDirection.z})");
    //     }
    //     
    //     // 플레이어에게 데미지
    //     PlayerInfo playerInfo = player.GetComponent<PlayerInfo>();
    //     if (playerInfo != null)
    //     {
    //         playerInfo.TakeDamage((int)attackDamage);
    //     }
    //
    //     Invoke(nameof(FinishAttack), 1f);
    // }

    private void FinishAttack()
    {
        isAttacking = false;
        Debug.Log("Attack Finished");
    }

    // protected override void Die()
    // {
    //     Debug.Log($"{monsterName} died at position: {transform.position}");
    //     base.Die();
    // }

    // 추가 디버그용 메서드
    private void OnDrawGizmos()
    {
        // 공격 범위 시각화
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        
        // 플레이어 방향 시각화
        if (player != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, player.position);
        }
    }
}