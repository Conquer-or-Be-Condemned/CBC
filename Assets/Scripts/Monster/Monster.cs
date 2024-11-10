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
    private Image healthBarCurrentImage; // Current 이미지를 참조
    private Transform healthBarTransform;

    [Header("Health Bar Settings")]
    [Tooltip("체력바의 Y 위치 오프셋을 설정합니다.")]
    public float healthBarYOffset = 2f;

    protected virtual void Start()
    {
        currentHealth = maxHealth;

        // 체력바 프리팹 인스턴스화
        if (healthBarPrefab != null)
        {
            GameObject healthBarInstance = Instantiate(healthBarPrefab);
            healthBarTransform = healthBarInstance.transform;

            // Current 이미지를 찾아서 참조
            healthBarCurrentImage = healthBarInstance.transform.Find("Current").GetComponent<Image>();

            // 체력바를 몬스터의 자식으로 설정
            healthBarTransform.SetParent(transform, false);

            // 로컬 위치를 조정하여 몬스터 위에 표시
            RectTransform healthBarRect = healthBarTransform.GetComponent<RectTransform>();
            healthBarRect.localPosition = Vector3.zero;
            healthBarRect.anchoredPosition = new Vector2(0, healthBarYOffset);

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

        if (healthBarCurrentImage != null)
        {
            float fillAmount = currentHealth / maxHealth;
            healthBarCurrentImage.fillAmount = fillAmount;
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
