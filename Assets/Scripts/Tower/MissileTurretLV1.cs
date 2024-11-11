using System.Collections;
using System.Collections.Generic;
using Tower;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;

public class MissileTurretLV1 : DefaultMissileTurret
{   
    [Header("References")]
    [SerializeField] private Transform turretRotationPoint; // 타워 회전 각도
    [SerializeField] private LayerMask enemyMask;           //raycast 감지 Layer
    [SerializeField] private Animator animator;             //타워 부분 Animator
    [SerializeField] private GameObject bulletPrefab;       //총알 오브젝트 생성 위한 변수
    [SerializeField] private Transform missileSpawnPoint;    //미사일 스폰 지점
    [SerializeField] private Transform missileSpawnPoint2;    //미사일 스폰 지점
    [SerializeField] private SpriteRenderer gunRenderer;    //과열시 색 변화
    
    //[SerializeField] private GameObject towerPrefab;
    
    [Header("Attributes")]
    [SerializeField] private float range;         // 타워 사거리
    [SerializeField] private float rotationSpeed;// 타워 회전 속도
    [SerializeField] private float fireRate;       // 발사 속도, 충격발 애니메이션이랑 연동시키기? ㄱㄴ?
    [SerializeField] private int power;            //타워 사용 전력량
    [SerializeField]private int overHeatMissileCount;    //~초 격발시 과열
    [SerializeField]private float coolTime;        //~초 지나면 냉각
    
    private void Start()
    {
        base.TurretRotationPoint = this.turretRotationPoint;
        base.EnemyMask = this.enemyMask;
        base.Animator = this.animator;
        base.GunRenderer = this.gunRenderer;
        base.Range = this.range;
        base.RotationSpeed = this.rotationSpeed;
        base.FireRate = this.fireRate;
        base.Power = this.power;
        base.OverHeatMissileCount = this.overHeatMissileCount;
        base.Level = 1;
        base.Name = "Missile Turret";
        gunRenderer.color = new Color(0.5f, 0.5f, 0.5f);
    }
    override
    protected void Shoot()
    {
        CurMissileCount += 1;
        StartCoroutine(ShootAnimation());
    
        // 첫 번째 미사일 생성 - 터렛 회전값 적용
        GameObject missileObj1 = Instantiate(bulletPrefab, missileSpawnPoint.position, turretRotationPoint.rotation);
        TowerMissile missileScript = missileObj1.GetComponent<TowerMissile>();
        missileScript.SetTarget(Target1);
    
        // 두 번째 미사일 생성
        GameObject missileObj2 = Instantiate(bulletPrefab, missileSpawnPoint2.position, turretRotationPoint.rotation);
        TowerMissile missileScript2 = missileObj2.GetComponent<TowerMissile>();
    
        if (Target2 != null)
        {
            missileScript2.SetTarget(Target2);
        }
        else
        {
            missileScript2.SetTarget(Target1);
        }
    
        Target1 = null;
        Target2 = null;
    }


    protected override void FindTarget()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, range, enemyMask);
        if (hits.Length == 0) return;

        // 사용할 수 있는 타겟들의 리스트를 만듭니다
        List<(Collider2D collider, float distance)> availableTargets = new List<(Collider2D, float)>();
        foreach (var hit in hits)
        {
            float distance = Vector2.Distance(transform.position, hit.transform.position);
            availableTargets.Add((hit, distance));
        }
        availableTargets.Sort((a, b) => a.distance.CompareTo(b.distance));
    
        // 각 Target에 대해 아직 할당되지 않은 가장 가까운 적을 찾아 할당합니다
        if (availableTargets.Count > 0)
        {
            Target1 = availableTargets[0].collider.transform;
            Target2 = availableTargets[0].collider.transform;
            availableTargets.RemoveAt(0); // 할당된 타겟은 리스트에서 제거
            availableTargets.RemoveAt(0); // 할당된 타겟은 리스트에서 제거
        }
        if (availableTargets.Count > 0)
        {
            Target2 = availableTargets[0].collider.transform;
            availableTargets.RemoveAt(0);
            availableTargets.RemoveAt(0);
        }
    }
    
    protected override void RotateTowardsTarget()//적향해 타워 z축 회전(TowerIsActivatedNow에서 수행)
    {
        if (Target1 == null) return;
        if (Target2 != null)
        {
            float angle = Mathf.Atan2((Target1.position.y + Target2.position.y)/2 - transform.position.y, (Target1.position.x + Target2.position.x)/2  - transform.position.x) * Mathf.Rad2Deg - 90f;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
            turretRotationPoint.rotation = Quaternion.RotateTowards(turretRotationPoint.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            float angle =
                Mathf.Atan2(Target1.position.y - transform.position.y, Target1.position.x - transform.position.x) *
                Mathf.Rad2Deg - 90f;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
            turretRotationPoint.rotation = Quaternion.RotateTowards(turretRotationPoint.rotation, targetRotation,
                rotationSpeed * Time.deltaTime);
        }
    }
}