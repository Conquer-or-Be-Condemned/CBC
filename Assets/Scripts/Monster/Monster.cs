using UnityEngine;

public abstract class Monster : MonoBehaviour
{
    [Header("Monster Stats")]
    public string monsterName;
    public float maxHealth;
    public float currentHealth;
    public float attackDamage;
    public float moveSpeed;
    public float attackRange;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        //InitTurretSprite fire = GetComponentInParent<InitTurretSprite>();
        //fire.am.SetBool("isShoot",false);
        // 사망 처리 로직
        Destroy(gameObject);
    }

    protected virtual void Move()
    {
        // 기본 이동 로직
    }

    protected virtual void Attack()
    {
        // 기본 공격 로직
    }
}