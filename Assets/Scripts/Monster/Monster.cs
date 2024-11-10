using UnityEngine;
using UnityEngine.UI;

public abstract class Monster : MonoBehaviour
{
    [Header("Monster Stats")]
    public string monsterName;
    public float maxHealth;
    public float currentHealth;
    public float attackDamage;
    public float moveSpeed;
    public float attackRange;

    [Header("References")]
    [Tooltip("플레이어의 Transform을 할당하세요.")]
    public Transform player;

    [Header("UI Elements")]
    public GameObject healthBarPrefab;
    private Slider healthBarSlider;
    private Transform healthBarTransform;

    [Header("Health Bar Settings")]
    [Tooltip("체력바의 Y 위치 오프셋을 설정합니다.")]
    public float healthBarYOffset = 2f; // 기본값을 2f로 설정

    protected virtual void Start()
    {
        currentHealth = maxHealth;

        // 체력바 프리팹 인스턴스화
        if (healthBarPrefab != null)
        {
            GameObject healthBarInstance = Instantiate(healthBarPrefab, transform.position, Quaternion.identity);
            healthBarTransform = healthBarInstance.transform;
            healthBarSlider = healthBarInstance.GetComponentInChildren<Slider>();

            // 체력바의 최대값 설정
            healthBarSlider.maxValue = maxHealth;
            healthBarSlider.value = currentHealth;

            // 체력바를 몬스터의 자식으로 설정
            healthBarTransform.SetParent(transform, false);

            // 로컬 위치를 조정하여 몬스터 위에 표시
            healthBarTransform.localPosition = new Vector3(0, healthBarYOffset, 0); // Y 값을 변수로 설정

            // 체력바가 항상 카메라를 향하도록 설정
            healthBarInstance.AddComponent<Billboard>();
        }
        else
        {
            Debug.LogWarning("Health bar prefab is not assigned on " + monsterName);
        }
    }

    public virtual void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthBarSlider != null)
        {
            healthBarSlider.value = currentHealth;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        // 체력바 제거
        if (healthBarTransform != null)
        {
            Destroy(healthBarTransform.gameObject);
        }

        // 몬스터 제거
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
