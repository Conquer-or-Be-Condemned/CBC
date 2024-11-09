using System.Collections;
using System.Collections.Generic;
using Tower;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;

public class MissileTurretLV3 : DefaultMissileTurret
{   
    [Header("References")]
    [SerializeField] private Transform turretRotationPoint; // 타워 회전 각도
    [SerializeField] private LayerMask enemyMask;           //raycast 감지 Layer
    [SerializeField] private Animator animator;             //타워 부분 Animator
    [SerializeField] private GameObject bulletPrefab;       //총알 오브젝트 생성 위한 변수
    [SerializeField] private Transform missileSpawnPoint;    //미사일 스폰 지점
    [SerializeField] private Transform missileSpawnPoint2;    //미사일 스폰 지점
    [SerializeField] private Transform missileSpawnPoint3;    //미사일 스폰 지점
    [SerializeField] private Transform missileSpawnPoint4;    //미사일 스폰 지점
    [SerializeField] private Transform missileSpawnPoint5;    //미사일 스폰 지점
    [SerializeField] private Transform missileSpawnPoint6;    //미사일 스폰 지점
    [SerializeField] private SpriteRenderer gunRenderer;    //과열시 색 변화
    
    
    
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
        base.Level = 3;
        base.Name = "Missile Turret";
    }
    protected override void Shoot()
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
        
        // 세번째 미사일 생성
        GameObject missileObj3 = Instantiate(bulletPrefab, missileSpawnPoint3.position, turretRotationPoint.rotation);
        TowerMissile missileScript3 = missileObj3.GetComponent<TowerMissile>();

        // 네번째 미사일 생성
        GameObject missileObj4 = Instantiate(bulletPrefab, missileSpawnPoint4.position, turretRotationPoint.rotation);
        TowerMissile missileScript4 = missileObj4.GetComponent<TowerMissile>();

        // 다섯번째 미사일 생성
        GameObject missileObj5 = Instantiate(bulletPrefab, missileSpawnPoint5.position, turretRotationPoint.rotation);
        TowerMissile missileScript5 = missileObj5.GetComponent<TowerMissile>();

        // 여섯번째 미사일 생성
        GameObject missileObj6 = Instantiate(bulletPrefab, missileSpawnPoint6.position, turretRotationPoint.rotation);
        TowerMissile missileScript6 = missileObj6.GetComponent<TowerMissile>();
        missileScript6.SetTarget(Target6);

        if (Target2 == null)
        {
            missileScript2.SetTarget(Target1);
            missileScript3.SetTarget(Target1);
        }
        else if(Target3 == null)
        {
            missileScript2.SetTarget(Target2);
            missileScript3.SetTarget(Target2);
        }
        else
        {
            missileScript2.SetTarget(Target2);
            missileScript3.SetTarget(Target3);
        }

        if (Target5 == null)
        {
            missileScript5.SetTarget(Target6);
            missileScript4.SetTarget(Target6);
        }
        else if (Target4 == null)
        {
            missileScript5.SetTarget(Target5);
            missileScript4.SetTarget(Target5);
        }
        else
        {
            missileScript5.SetTarget(Target5);
            missileScript4.SetTarget(Target4);
        }
    
        Target1 = null;
        Target2 = null;
        Target3 = null;
        Target4 = null;
        Target5 = null;
        Target6 = null;
    }


    protected override  void FindTarget()
    {
        // OverlapCircleAll을 사용하여 범위 내의 모든 적 탐지
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, range, enemyMask);
    
        // 탐지된 적이 없으면 종료
        if (hits.Length == 0) return;
    
        // 거리에 따라 정렬하기 위한 리스트 생성
        List<(Collider2D collider, float distance)> sortedTargets = new List<(Collider2D, float)>();
    
        foreach (var hit in hits)
        {
            float distance = Vector2.Distance(transform.position, hit.transform.position);
            sortedTargets.Add((hit, distance));
        }
    
        // 거리순으로 정렬
        sortedTargets.Sort((a, b) => a.distance.CompareTo(b.distance));
        
        if (sortedTargets.Count > 0)//1,6,2,5,3,4 순서로 표적 할당
        {
            Target1 = sortedTargets[0].collider.transform;
            if(sortedTargets.Count<2)
                Target6 = sortedTargets[0].collider.transform;
        }
        
        if (sortedTargets.Count > 1)
        {
            if (Target6 == null)
            {
                Target6 = sortedTargets[1].collider.transform;
            }
        }
        if (sortedTargets.Count > 2)
        {
            if (Target2 == null)
            {
                Target2 = sortedTargets[2].collider.transform;
            }
        }
        if (sortedTargets.Count > 3)
        {
            if (Target5 == null)
            {
                Target5 = sortedTargets[3].collider.transform;
            }
        }
        if (sortedTargets.Count > 4)
        {
            if (Target3 == null)
            {
                Target3 = sortedTargets[4].collider.transform;
            }
        }
        if (sortedTargets.Count > 5)
        {
            if (Target4 == null)
            {
                Target4 = sortedTargets[5].collider.transform;
            }
        }
    
        if (Target1 != null)
        {
            Debug.DrawLine(transform.position, Target1.position, Color.red, 0.1f);
        }
        if (Target4 != null)
        {
            Debug.DrawLine(transform.position, Target4.position, Color.blue, 0.1f);
        }
        if (Target2 != null)
        {
            Debug.DrawLine(transform.position, Target2.position, Color.blue, 0.1f);
        }
        if (Target3 != null)
        {
            Debug.DrawLine(transform.position, Target3.position, Color.blue, 0.1f);
        }
        if (Target6 != null)
        {
            Debug.DrawLine(transform.position, Target6.position, Color.blue, 0.1f);
        }
        if (Target5 != null)
        {
            Debug.DrawLine(transform.position, Target5.position, Color.blue, 0.1f);
        }
    }
    protected override  void RotateTowardsTarget()//적향해 타워 z축 회전(TowerIsActivatedNow에서 수행)
    {
        if (Target1 == null) return;
        if (Target6 != null)
        {
            float angle = Mathf.Atan2((Target1.position.y + Target6.position.y)/2 - transform.position.y, (Target1.position.x + Target6.position.x)/2  - transform.position.x) * Mathf.Rad2Deg - 90f;
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