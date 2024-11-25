using System.Collections;
using System.Collections.Generic;
using Tower;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;

public class MissileTurretLV2 : DefaultMissileTurret
{   
    [Header("References")]
    [SerializeField] private Transform turretRotationPoint; // 타워 회전 각도
    [SerializeField] private LayerMask enemyMask;           //raycast 감지 Layer
    [SerializeField] private Animator animator;             //타워 부분 Animator
    [SerializeField] private GameObject bulletPrefab;       //총알 오브젝트 생성 위한 변수
    // [SerializeField] private Transform missileSpawnPoint;    //미사일 스폰 지점
    // [SerializeField] private Transform missileSpawnPoint2;    //미사일 스폰 지점
    // [SerializeField] private Transform missileSpawnPoint3;    //미사일 스폰 지점
    [SerializeField] protected Transform []missileSpawnPoint;    //미사일 스폰 지점
    [SerializeField] protected Transform []missileTargetPoint;    //미사일 스폰 지점
    
    [SerializeField] private SpriteRenderer gunRenderer;    //과열시 색 변화
    
    //[SerializeField] private GameObject towerPrefab;
    
    [Header("Attributes")]
    [SerializeField] private float range;         // 타워 사거리
    [SerializeField] private float rotationSpeed;// 타워 회전 속도
    [SerializeField] private float fireRate;       // 발사 속도, 충격발 애니메이션이랑 연동시키기? ㄱㄴ?
    [SerializeField] private int power;            //타워 사용 전력량
    [SerializeField]private int overHeatMissileCount;    //~초 격발시 과열
    [SerializeField]private float coolTime;        //~초 지나면 냉각
    private GameObject[] _missileObj;
    private void Start()
    {
        TurretRotationPoint = turretRotationPoint;
        Animator = animator;
        GunRenderer = gunRenderer;
        Range = range;
        FireRate = fireRate;
        Power = power;
        OverHeatMissileCount = overHeatMissileCount;
        RotationSpeed = rotationSpeed;
        EnemyMask = enemyMask;
        Level = 2;
        
        gunRenderer.color = new Color(0.5f, 0.5f, 0.5f);
        _missileObj = new GameObject[missileSpawnPoint.Length];
        Targets = new Transform[4];

        RPM = (int)(60 / (1 / fireRate));
        Damage = 20;
    }
    protected override void Shoot()
    {
        CurMissileCount += 1;
        StartCoroutine(ShootAnimation());
        for (int i = 0; i < _missileObj.Length; i++)
        {
            if (Targets[i] != null)
            {
                _missileObj[i] = Instantiate(bulletPrefab, missileSpawnPoint[i].position, turretRotationPoint.rotation);
                TowerMissile missileScript = _missileObj[i].GetComponent<TowerMissile>();
                missileScript.SetTarget(Targets[i]);
            }
            
            // if (Targets[i] != null) missileScript.SetTarget(Targets[i]);
            // else if((Targets[i] != null) && (i > 1))
            // {
            //     if (Targets[i - 2] != null) missileScript.SetTarget(Targets[i - 2]);
            //     else missileScript.SetTarget(Targets[i - 4]);
            // }
            
        }
        for (var i = 0; i < _missileObj.Length; i++)
        {
            Targets[i] = null;
        }
    }
}