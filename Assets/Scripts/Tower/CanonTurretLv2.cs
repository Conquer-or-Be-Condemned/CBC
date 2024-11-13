using System.Collections;
using System.Collections.Generic;
using Tower;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;

public class CanonTurretLv2 : DefaultCanonTurret
{   
    [Header("References")]
    [SerializeField] private Transform turretRotationPoint; // 타워 회전 각도
    [SerializeField] private Transform[] bulletSpawnPoint;    //총알 스폰 지점
    [SerializeField] private Transform []bulletFireDirection;    //총 격발 방향
    [SerializeField] private LayerMask enemyMask;           //raycast 감지 Layer
    [SerializeField] private Animator animator;             //타워 부분 Animator
    [SerializeField] private SpriteRenderer gunRenderer;    //과열시 색 변화
    [SerializeField] private GameObject bulletPrefab;       //총알 오브젝트 생성 위한 변수
    
    [Header("Attributes")] 
    [SerializeField] private new float range;        // 타워 사거리
    [SerializeField] private new float rotationSpeed;// 타워 회전 속도
    [SerializeField] private new float fireRate;       // 발사 속도, 충격발 애니메이션이랑 연동시키기? ㄱㄴ?
    [SerializeField] private new int power;            //타워 사용 전력량
    [SerializeField] private new float overHeatTime;    //~초 격발시 과열
    [SerializeField] private new float coolTime;        //~초 지나면 냉각
    
    private GameObject []_bulletObj;
    private void Start()
    {
        _bulletObj = new GameObject[bulletSpawnPoint.Length];
        base.GunRenderer = this.gunRenderer;
        base.EnemyMask = this.enemyMask;
        base.Animator = this.animator;
        base.TurretRotationPoint = this.turretRotationPoint;
        base.Range = this.range;         // 타워 사거리
        base.RotationSpeed = this.rotationSpeed;// 타워 회전 속도
        base.FireRate = this.fireRate;       // 발사 속도, 충격발 애니메이션이랑 연동시키기? ㄱㄴ?
        base.Power = this.power;            //타워 사용 전력량
        base.OverHeatTime = overHeatTime;    //~초 격발시 과열
        base.CoolTime = coolTime; //~초 지나면 냉각
        base.Level = 2;
        GunRenderer.color = new Color(0.5f, 0.5f, 0.5f);
    }
    override 
    protected void Shoot()//총알 객체화 후 목표로 발사(FireRateController에서 수행)
    {
        Debug.Log(bulletSpawnPoint.Length);
        Debug.Log(bulletFireDirection.Length);

        animator.enabled = true; // 발사할 때 애니메이션 시작
        for (int i = 0; i < 2; i++)
        {
            _bulletObj[i] = Instantiate(bulletPrefab, bulletSpawnPoint[i].position, Quaternion.identity);
            TowerBullet towerBulletScript = _bulletObj[i].GetComponent<TowerBullet>();
            towerBulletScript.SetTarget(bulletFireDirection[i]);
        }
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.fire);
    }
    
    private void OnDrawGizmosSelected()//타워의 반경 그려줌(디버깅용, 인게임에는 안나옴)
    {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, range);
    }
}
