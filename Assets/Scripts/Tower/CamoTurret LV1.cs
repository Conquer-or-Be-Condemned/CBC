using System.Collections;
using System.Collections.Generic;
using Tower;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

public class CamoTurretLV1 : DefaultTurret
{   
    [Header("References")]
    [SerializeField] private Transform turretRotationPoint; // 타워 회전 각도
    [SerializeField] private LayerMask enemyMask;           //raycast 감지 Layer
    [SerializeField] private Animator animator;             //타워 부분 Animator
    [SerializeField] private GameObject bulletPrefab;       //총알 오브젝트 생성 위한 변수
    [SerializeField] private Transform bulletSpawnPoint;    //총알 스폰 지점
    [SerializeField] private SpriteRenderer gunRenderer;    //과열시 색 변화

    [Header("Attributes")] 
    [SerializeField] private new float range;        // 타워 사거리
    [SerializeField] private new float rotationSpeed;// 타워 회전 속도
    [SerializeField] private new float fireRate;       // 발사 속도, 충격발 애니메이션이랑 연동시키기? ㄱㄴ?
    [SerializeField] private new int power;            //타워 사용 전력량
    [SerializeField] private new float overHeatTime;    //~초 격발시 과열
    [SerializeField] private new float coolTime;        //~초 지나면 냉각

    //[SerializeField] private GameObject towerPrefab;
    // CamoTurretLV1()
    // {
    //     base.turretRotationPoint = this.turretRotationPoint;
    //     range = 10f;         // 타워 사거리
    //     rotationSpeed = 200f;// 타워 회전 속도
    //     fireRate = 5f;       // 발사 속도, 충격발 애니메이션이랑 연동시키기? ㄱㄴ?
    //     power = 60;            //타워 사용 전력량
    //     overHeatTime = 5f;    //~초 격발시 과열
    //     coolTime = 5f;        //~초 지나면 냉각
    // }

    private void Start()
    {
        base.gunRenderer = this.gunRenderer;
        base.enemyMask = this.enemyMask;
        base.animator = this.animator;
        base.turretRotationPoint = this.turretRotationPoint;
        base.range = this.range;         // 타워 사거리
        base.rotationSpeed = this.rotationSpeed;// 타워 회전 속도
        base.fireRate = this.fireRate;       // 발사 속도, 충격발 애니메이션이랑 연동시키기? ㄱㄴ?
        base.power = this.power;            //타워 사용 전력량
        base.overHeatTime = overHeatTime;    //~초 격발시 과열
        base.coolTime = coolTime; //~초 지나면 냉각
    }
    // [SerializeField] private float damage; // 공격력
    
    //-------------------------------------------------------
    // public bool isActivated = false;//타워 가동 여부
    // private bool _previousIsActivated = false;//버퍼(토글 확인)
    //-------------------------------------------------------
    // private GameObject _OriginPower;    //ControlUnitStatus Script의 함수사용
    // private Transform _target;          //target of bullets
    // private ControlUnitStatus _cus;     //_cus = _OriginPower.GetComponent<ControlUnitStatus>();
    // // private Bullet _bulletScript;
    // private float _timeTilFire;         //다음 발사까지의 시간
    // private float _angleThreshold = 5f; // 타워와 적의 각도 차이 허용 범위 (조정 가능)
    // private float _fireTime = 0f;       //과열시 중지 위한 변수
    // private float _totCoolTime;         //냉각시 누적 냉각시간
    //-------------------------------------------------------
    
    
    
    // private GameObject _gunPrefab;
    // private SpriteRenderer _gunSprite;
    // private bool isActivated = false;
    // private bool _previousIsActivated = false;
    // private void Awake()
    // {
    //     OriginPower = GameObject.Find("ControlUnit");
    //     ControlUnitStatus cus = OriginPower.GetComponent<ControlUnitStatus>();
    //     
    // }
    
    
    // private void Start()
    // {
    //     _OriginPower = GameObject.Find("ControlUnit");
    //     _cus = _OriginPower.GetComponent<ControlUnitStatus>();//제어장치 정보 가져오기 위함
    //     // GameObject bulletObj = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
    //     // _bulletScript = bulletObj.GetComponent<Bullet>();
    // }
    // private void Update()
    // {
    //     CheckToggle();//사용자에 의한 타워 가동 토글 확인
    //     TowerIsActivatedNow();//사용자에 의해 타워가 가동 됐다면 역할 수행
    //     
    // }
    override 
    protected void Shoot()//총알 객체화 후 목표로 발사(FireRateController에서 수행)
    {
        animator.enabled = true; // 발사할 때 애니메이션 시작
        GameObject bulletObj = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        TowerBullet towerBulletScript = bulletObj.GetComponent<TowerBullet>();
        towerBulletScript.SetTarget(base._target);

        // 애니메이션을 짧은 시간 뒤에 종료
        // StartCoroutine(StopAnimation());
    }
    // private IEnumerator StopAnimation()
    // {
    //     yield return new WaitForSeconds(3f); // 애니메이션 지속 시간 설정 (조정 가능)
    //     animator.enabled = false; // 애니메이션 종료
    // }
    
    //  UI와 연동하기 위한 함수  ->  미완성 !!!
    // public UnityEvent<int> onActivateChange = new UnityEvent<int>();
    // public void SetIsActivated(bool activated)
    // {
    //     _previousIsActivated = isActivated;
    //     
    //     isActivated = activated;
    //     Debug.Log("Now activated: " + activated);
    //     
    //     onActivateChange.Invoke(3);
    // }

}
