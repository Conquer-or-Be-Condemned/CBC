using System.Collections;
using System.Collections.Generic;
using Tower;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;

public class MissileTurretLV3 : DefaultMissileTurret
{
    [Header("References")] [SerializeField]
    private Transform turretRotationPoint; // 타워 회전 각도

    [SerializeField] private LayerMask enemyMask; //raycast 감지 Layer
    [SerializeField] private Animator animator; //타워 부분 Animator
    [SerializeField] private GameObject bulletPrefab; //총알 오브젝트 생성 위한 변수
    [SerializeField] private Transform[] missileSpawnPoint; //미사일 스폰 지점
    [SerializeField] private Transform[] missileTargetPoint; //미사일 스폰 지점
    [SerializeField] private SpriteRenderer gunRenderer; //과열시 색 변화
    [SerializeField] private SpriteRenderer rangeRenderer;
    [SerializeField] private Transform rangeTransform;

    [Header("Attributes")] [SerializeField]
    private float range; //타워 사거리

    [SerializeField] private float rotationSpeed; //타워 회전 속도
    [SerializeField] private float fireRate; //발사 속도
    [SerializeField] private int power; //타워 사용 전력량
    [SerializeField] private int overHeatMissileCount; //~초 격발시 과열
    [SerializeField] private float coolTime; //~초 지나면 냉각
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
        RangeRenderer = rangeRenderer;
        gunRenderer.color = new Color(0.5f, 0.5f, 0.5f);
        Targets = new Transform[6];
        _missileObj = new GameObject[missileSpawnPoint.Length];
        
        //Turrets Attack Range
        rangeTransform.localScale = new Vector3(Range*2.5f, Range*2.5f, 1f);;
        //Info for UI
        Level = 3;
        RPM = (int)(60 / (1 / fireRate));
        Damage = 30;
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
        }

        for (var i = 0; i < _missileObj.Length; i++)
        {
            Targets[i] = null;
        }
    }
    private void OnDrawGizmosSelected()//타워의 반경 그려줌(디버깅용, 인게임에는 안나옴)
    {
#if UNITY_EDITOR
        Handles.color = Color.magenta;
        Handles.DrawWireDisc(transform.position, transform.forward, range);
#endif
    }
}