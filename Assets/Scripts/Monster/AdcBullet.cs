using System.Collections;
using UnityEngine;

public class AdcBullet : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    
    [Header("Attributes")] 
    [SerializeField] private float bulletSpeed = 17f;
    [SerializeField] private float bulletDamage = 30f;
    [SerializeField] private Transform bulletSpawnPoint;

    [Header("ControlUnit")] public ControlUnitStatus controlUnit;
    
    private Vector2 direction;
    
    // 방향을 설정하는 메서드
    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
        StartCoroutine(DestroyObjectIfNotHit());
    }

    private void FixedUpdate()
    {
        if (direction == Vector2.zero) return;

        // Rigidbody2D의 속도를 방향과 속도에 맞게 설정
        rb.velocity = direction * bulletSpeed;

        // 총알의 회전 설정 (방향 벡터 기준)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private IEnumerator DestroyObjectIfNotHit()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }

    public void SetControlUnitTarget(ControlUnitStatus target)
    {
        controlUnit = target;
    }

    // 충돌 시 총알 파괴
    private void OnCollisionEnter2D(Collision2D other)
    {   
        //Monster monster = other.gameObject.GetComponent<Monster>();
        if (other.gameObject.GetComponent<Player>() != null)
        {
            Player player = other.gameObject.GetComponent<Player>();
            PlayerInfo playerInfo = player.GetComponent<PlayerInfo>();
            if (player != null)
            {
                Debug.Log("Damaged!");
                playerInfo.TakeDamage((int)bulletDamage);
            }
            Destroy(gameObject);
        }
        
        //  수정이 필요하긴 할 것 같음 - 현석
        if (other.gameObject.CompareTag("CUBoundary"))
        {
            Debug.Log("충돌!");
            if (controlUnit == null)
            {
                Debug.LogError("CU가 감지되지 않습니다.");
                return;
            }
            controlUnit.GetDamage((int)bulletDamage);
            Destroy(gameObject);
        }
    }
}