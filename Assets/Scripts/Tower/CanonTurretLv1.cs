using System.Collections;
using System.Collections.Generic;
using Tower;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class CanonTurretLv1 : DefaultCanonTurret
{   
    [Header("References")]
    [SerializeField] private Transform turretRotationPoint; // 타워 회전 각도
    [SerializeField] private LayerMask enemyMask;           //raycast 감지 Layer
    [SerializeField] private Animator animator;             //타워 부분 Animator
    [SerializeField] private GameObject bulletPrefab;       //총알 오브젝트 생성 위한 변수
    [SerializeField] private Transform bulletSpawnPoint;    //총알 스폰 지점
    [SerializeField] private SpriteRenderer gunRenderer;    //과열시 색 변화
    [SerializeField] private Transform bulletFirePoint;
    [SerializeField] private SpriteRenderer rangeRenderer;
    [SerializeField] private Transform rangeTransform;
    // [SerializeField] private LayerMask enemyMask;
    [Header("Attributes")] 
    [SerializeField] private float range;        // 타워 사거리
    [SerializeField] private float rotationSpeed;// 타워 회전 속도
    [SerializeField] private float fireRate;       // 발사 속도, 충격발 애니메이션이랑 연동시키기? ㄱㄴ?
    [SerializeField] private int power;            //타워 사용 전력량
    [SerializeField] private float overHeatTime;    //~초 격발시 과열
    [SerializeField] private float coolTime;        //~초 지나면 냉각
    [SerializeField] private bool showRange;
    // private void Update()
    // {
    //     rangeRenderer.enabled = showRange;
    // }
    private void Start()
    {
        GunRenderer = gunRenderer;
        Animator = animator;
        TurretRotationPoint = turretRotationPoint;
        Range = range;         // 타워 사거리
        RotationSpeed = rotationSpeed;// 타워 회전 속도
        FireRate = fireRate;       // 발사 속도, 충격발 애니메이션이랑 연동시키기? ㄱㄴ?
        Power = power;            //타워 사용 전력량
        OverHeatTime = overHeatTime;    //~초 격발시 과열
        CoolTime = coolTime; //~초 지나면 냉각
        Level = 1;
        GunRenderer.color = new Color(0.5f, 0.5f, 0.5f);
        RPM = (int)(60 / (1 / fireRate));
        Damage = 10;
        EnemyMask = enemyMask;
        RangeRenderer = rangeRenderer;
        
        // RangeTransform = rangeTransform;
        rangeTransform.localScale = new Vector3(Range*2.5f, Range*2.5f, 1f);

    }
    override 
    protected void Shoot()//총알 객체화 후 목표로 발사(FireRateController에서 수행)
    {
        animator.enabled = true; // 발사할 때 애니메이션 시작
        GameObject bulletObj = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        TowerBullet towerBulletScript = bulletObj.GetComponent<TowerBullet>();
        float randomValue = Random.Range(-1f, 1f);
        bulletFirePoint.position = new Vector3(bulletFirePoint.position.x+randomValue, bulletFirePoint.position.y,0f);
        towerBulletScript.SetTarget(bulletFirePoint);   
        // AudioManager.Instance.PlaySfx(AudioManager.Sfx.Fire,true);
    }
    private void OnDrawGizmosSelected()//타워의 반경 그려줌(디버깅용, 인게임에는 안나옴)
    {
#if UNITY_EDITOR
        Handles.color = Color.magenta;
        Handles.DrawWireDisc(transform.position, transform.forward, range);
#endif
    }
}
