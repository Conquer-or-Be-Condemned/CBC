using System.Collections;
using System.Collections.Generic;
using Tower;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;

public class CanonTurretLv3 : DefaultCanonTurret
{   
    [Header("References")]
    [SerializeField] private Transform turretRotationPoint; // 타워 회전 각도
    [SerializeField] private LayerMask enemyMask;           //raycast 감지 Layer
    [SerializeField] private Animator animator;             //타워 부분 Animator
    [SerializeField] private GameObject bulletPrefab;           //총알 오브젝트 생성 위한 변수
    [SerializeField] private Transform bulletSpawnPoint1;       //총알 스폰 지점
    [SerializeField] private Transform bulletSpawnPoint2;       //총알 스폰 지점
    [SerializeField] private Transform bulletSpawnPoint3;       //총알 스폰 지점
    [SerializeField] private Transform bulletFireDirection1;    //총 격발 방향
    [SerializeField] private Transform bulletFireDirection2;    //총 격발 방향
    [SerializeField] private Transform bulletFireDirection3;    //총 격발 방향
    [SerializeField] private new SpriteRenderer gunRenderer;    //과열시 색 변화
    
    [Header("Attributes")] 
    [SerializeField] private new float range;                   // 타워 사거리
    [SerializeField] private new float rotationSpeed;           // 타워 회전 속도
    [SerializeField] private new float fireRate;                // 발사 속도, 충격발 애니메이션이랑 연동시키기? ㄱㄴ?
    [SerializeField] private new int power;                     //타워 사용 전력량
    [SerializeField] private new float overHeatTime;            //~초 격발시 과열
    [SerializeField] private new float coolTime;                //~초 지나면 냉각

    private void Start()
    {
        base.GunRenderer = this.gunRenderer;
        base.EnemyMask = this.enemyMask;
        base.Animator = this.animator;
        base.TurretRotationPoint = this.turretRotationPoint;
        base.Range = this.range;       
        base.RotationSpeed = this.rotationSpeed;
        base.FireRate = this.fireRate;       
        base.Power = this.power;            
        base.OverHeatTime = overHeatTime;    
        base.CoolTime = coolTime; 
        base.Level = 3;
        GunRenderer.color = new Color(0.5f, 0.5f, 0.5f);
    } 
    protected override void Shoot()//총알 객체화 후 목표로 발사(FireRateController에서 수행)
    {
        animator.enabled = true; // 발사할 때 애니메이션 시작
        GameObject bulletObj1 = Instantiate(bulletPrefab, bulletSpawnPoint1.position, Quaternion.identity);
        TowerBullet towerBulletScript1 = bulletObj1.GetComponent<TowerBullet>();
        towerBulletScript1.SetTarget(bulletFireDirection1);
        
        GameObject bulletObj2 = Instantiate(bulletPrefab, bulletSpawnPoint2.position, Quaternion.identity);
        TowerBullet towerBulletScript2 = bulletObj2.GetComponent<TowerBullet>();
        towerBulletScript2.SetTarget(bulletFireDirection2);
        
        GameObject bulletObj3 = Instantiate(bulletPrefab, bulletSpawnPoint3.position, Quaternion.identity);
        TowerBullet towerBulletScript3 = bulletObj3.GetComponent<TowerBullet>();
        towerBulletScript3.SetTarget(bulletFireDirection3);
        
    }
    private void OnDrawGizmosSelected()//타워의 반경 그려줌(디버깅용, 인게임에는 안나옴)
    {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, range);
    }
}
