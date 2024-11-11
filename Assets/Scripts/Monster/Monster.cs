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

    [Header("Detection Settings")]
    [Tooltip("플레이어를 감지할 범위")]
    public float detectionRange; // 기본값 설정

    [Header("References")]
    [Tooltip("플레이어 Transform 할당")]
    public Transform player;

    [Tooltip("제어 장치의 ControlUnitStatus를 할당")]
    public ControlUnitStatus controlUnitStatus;

    [Header("UI Elements")]
    public GameObject healthBarPrefab;
    private Image healthBarCurrentImage; // Current 이미지를 참조
    private Transform healthBarTransform;

    [Header("Health Bar Settings")]
    [Tooltip("체력바의 Y 위치 오프셋을 설정")]
    public float healthBarYOffset;

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

        // ControlUnitStatus가 할당되지 않은 경우, 태그를 통해 씬에서 찾아 할당
        if (controlUnitStatus == null)
        {
            GameObject controlUnitObj = GameObject.FindWithTag("CU");
            if (controlUnitObj != null)
            {
                controlUnitStatus = controlUnitObj.GetComponent<ControlUnitStatus>();
                if (controlUnitStatus == null)
                {
                    Debug.LogWarning("ControlUnitStatus component not found on the ControlUnit object.");
                }
            }
            else
            {
                Debug.LogWarning("Control Unit not found in the scene. Please assign it manually.");
            }
        }

        // Player가 할당되지 않은 경우, 태그를 통해 씬에서 찾아 할당
        if (player == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
            else
            {
                Debug.LogWarning("Player not found in the scene. Please assign it manually.");
            }
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

    protected virtual void Move(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        Vector3 movement = direction * (moveSpeed * Time.deltaTime);
        transform.position += movement;
    }

    protected virtual void Attack()
    {
        // 기본 공격 로직
    }
}
