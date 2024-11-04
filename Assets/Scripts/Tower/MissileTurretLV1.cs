using System.Collections;
using System.Collections.Generic;
using Tower;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;

public class MissileTurretLV1 : MonoBehaviour, ActivateTower
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
    [SerializeField] private float range = 10f;         // 타워 사거리
    [SerializeField] private float rotationSpeed = 400f;// 타워 회전 속도
    [SerializeField] private float fireRate = 5f;       // 발사 속도, 충격발 애니메이션이랑 연동시키기? ㄱㄴ?
    [SerializeField] private int power = 1;            //타워 사용 전력량
    [SerializeField]private float overHeatTime = 5f;    //~초 격발시 과열
    [SerializeField]private float coolTime = 5f;        //~초 지나면 냉각
    
    // [SerializeField] private float damage; // 공격력
    
    //-------------------------------------------------------
    public bool isActivated = false;//타워 가동 여부
    private bool _previousIsActivated = false;//버퍼(토글 확인)
    //-------------------------------------------------------
    private GameObject _OriginPower;    //ControlUnitStatus Script의 함수사용
    private Transform _target1;          //target of missile1
    private Transform _target2;          //target of missile2
    private ControlUnitStatus _cus;     //_cus = _OriginPower.GetComponent<ControlUnitStatus>();
    // private Bullet _bulletScript;
    private float _timeTilFire;         //다음 발사까지의 시간
    private float _angleThreshold = 10f; // 타워와 적의 각도 차이 허용 범위 (조정 가능)
    private float _fireTime = 0f;       //과열시 중지 위한 변수
    private float _totCoolTime;         //냉각시 누적 냉각시간
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
    
    
    private void Start()
    {
        _OriginPower = GameObject.Find("ControlUnit");
        _cus = _OriginPower.GetComponent<ControlUnitStatus>();//제어장치 정보 가져오기 위함
        // GameObject bulletObj = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        // _bulletScript = bulletObj.GetComponent<Bullet>();
    }
    private void Update()
    {
        CheckToggle();//사용자에 의한 타워 가동 토글 확인
        TowerIsActivatedNow();//사용자에 의해 타워가 가동 됐다면 역할 수행
        
    }

    private void TowerIsActivatedNow()//사용자에 의해 타워가 가동 됐다면 역할 수행(Update에서 수행)
    {
        if (isActivated)
        {
            
            NoTargetInRange();//적이 타워 범위에 없을 때 탐색(raycast 사용)
            RotateTowardsTarget();//적 발견시 적을 향해 타워 돌리기
            FireRateController();//총알 객체화 후 발사 동작 수행
            OverHeatAnimationController();//설정시간 도달 시 과열
            
            
        }
    }

    private void NoTargetInRange()//적이 타워 범위에 없을 때 탐색(TowerIsActivatedNow에서 수행)
    {
        if (_target1 == null)
        {
            Debug.Log("Target is null");
            // _fireTime -= Time.deltaTime;
            // if(_fireTime <= 0f) _fireTime = 0f;
            //animator.SetBool("isShoot", false);
            FindTarget();//(raycast 사용)
            return;
        }
    }

    private void FireRateController()//총알 객체화 후 발사 동작 수행(TowerIsActivatedNow에서 수행)
    {
        if (!CheckTargetIsInRange())//적이 범위에 없음
        {
            // _fireTime -= Time.deltaTime;
            // if(_fireTime <= 0f) _fireTime = 0f;
            //animator.SetBool("isShoot", false);
            _target1 = null;
            _target2 = null;
            _timeTilFire = 0f;
        }
        else//적이 범위에 있음
        {
            _timeTilFire += Time.deltaTime;
            if (_timeTilFire >= (1f / fireRate) && IsTargetInSight())//적이 타워의 시야각에 있고 RPS만큼 발사
            {
                Shoot();
                _timeTilFire = 0f;

            }

        }
    }

    private IEnumerator ShootAnimation()
    {
        animator.SetBool("isShoot",true);
        yield return new WaitForSeconds(0.6f);
        animator.SetBool("isShoot",false);

    }
    private void OverHeatAnimationController()//설정시간 도달 시 과열(TowerIsActivatedNow에서 수행)
    {
        gunRenderer.color = new Color(1f,(255f-255f* (_fireTime / overHeatTime))/255f,(255f-255f*
            (_fireTime / overHeatTime))/255f);

        if (IsTargetInSight())//적이 사격 시야에 있음
        {
                
            _fireTime += Time.deltaTime;
            if (_fireTime >= overHeatTime)//터렛 과열
            {
                isActivated = false;
                _previousIsActivated = false;
                //animator.SetBool("isShoot", false);
                StartCoroutine(OverHeat());
            }
            else
            {
                //animator.SetBool("isShoot", true); 
            }
        }
        else//적이 사격 시야에 없음
        {
            // _fireTime -= Time.deltaTime;
            // if(_fireTime <= 0f) _fireTime = 0f;
            //animator.SetBool("isShoot", false);
        }
    }
    private void CheckToggle()//Checks toggle of isActivated
    {
        if (isActivated != _previousIsActivated)//toggle check
        {
            if (isActivated)
            {
                _previousIsActivated = isActivated; // 이전 상태를 현재 상태로 업데이트

                //Debug.Log("Activated");
                AddTurret();
            }
            else if (isActivated == false)
            {
                animator.SetBool("isShoot", false);
                // gunRenderer.color = Color.white;
                _previousIsActivated = isActivated; // 이전 상태를 현재 상태로 업데이트
                StartCoroutine(DeactivateProcess());
                _fireTime = 0f;

                //Debug.Log("DeActivated");
                DeleteTurret();
            }
            
        }
    }

    private IEnumerator DeactivateProcess()
    {
        _totCoolTime = _fireTime;
        while (_totCoolTime <= coolTime)
        {
            gunRenderer.color = new Color(1f,(_totCoolTime / coolTime),(_totCoolTime / coolTime));
            _totCoolTime += Time.deltaTime;
            yield return null;
        }

    }
    private IEnumerator OverHeat()//코루틴 함수 냉각 역할 수행(OverHeatAnimationController에서 수행)
    {
        _totCoolTime = 0f;
        while (_totCoolTime <= coolTime)
        {
            gunRenderer.color = new Color(1f,(_totCoolTime / coolTime),(_totCoolTime / coolTime));
            _totCoolTime += Time.deltaTime;
            yield return null;
        }
        animator.SetBool("isShoot", false);
        //yield return new WaitForSeconds(5f);
        gunRenderer.color = Color.white;
        _fireTime = 0f;
        isActivated = true;
        _previousIsActivated = true;
    }
   
    private void AddTurret()//ControlUnitStatus script 사용(CheckToggle에서 수행)
    {
        if (_cus.getCurrentPower() >= power)
        {
            _cus.AddUnit(power);
        }
        else
        {
            isActivated = false;
            _previousIsActivated = false;
        }
    }
    private void DeleteTurret()//ControlUnitStatus script 사용(CheckToggle에서 수행)
    {
        _cus.RemoveUnit(power);
    }
    private void Shoot()//총알 객체화 후 목표로 발사(FireRateController에서 수행)
    {
        StartCoroutine(ShootAnimation());
        // animator.enabled = true; // 발사할 때 애니메이션 시작
        GameObject missileObj1 = Instantiate(bulletPrefab, missileSpawnPoint.position, Quaternion.identity);
        
        Missile missileScript = missileObj1.GetComponent<Missile>();
        
        missileScript.SetTarget(_target1);
        if (_target2 != null)
        {
            GameObject missileObj2 = Instantiate(bulletPrefab, missileSpawnPoint2.position, Quaternion.identity);
            Missile missileScript2 = missileObj2.GetComponent<Missile>();
            missileScript2.SetTarget(_target2);
        }
        else
        {
            GameObject missileObj2 = Instantiate(bulletPrefab, missileSpawnPoint2.position, Quaternion.identity);
            Missile missileScript2 = missileObj2.GetComponent<Missile>();
            missileScript2.SetTarget(_target1);
        }

        // 애니메이션을 짧은 시간 뒤에 종료
        // StartCoroutine(StopAnimation());
    }


    private void FindTarget()//raycast를 이용한 적 타워 반경 접근 확인 후 배열 추가(NoTargetInRange에서 적을 찾기위해 수행)
    {
        //Debug.Log("FindTarget");
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, range, Vector2.zero, 0f, enemyMask);
        if (hits.Length > 0)
        {
            _target1 = hits[0].transform;
        }
        if (hits.Length > 1)
        {
            _target2 = hits[1].transform;
        }
    }

    private bool CheckTargetIsInRange()//적이 사거리에 있는지 확인(FireRateController에서 수행)
    {
        return Vector2.Distance(_target1.position, transform.position) <= range;
    }

    private void RotateTowardsTarget()//적향해 타워 z축 회전(TowerIsActivatedNow에서 수행)
    {
        float angle = Mathf.Atan2(_target1.position.y - transform.position.y, _target1.position.x - transform.position.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        turretRotationPoint.rotation = Quaternion.RotateTowards(turretRotationPoint.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private bool IsTargetInSight()//적이 시야각에 있는지 확인(FireRateController, OverHeatAnimationController에서 수행)
    {
        float angleToTarget = Mathf.Atan2(_target1.position.y - transform.position.y, _target1.position.x - transform.position.x) * Mathf.Rad2Deg - 90f;
        float turretAngle = turretRotationPoint.eulerAngles.z;
        float angleDifference = Mathf.DeltaAngle(turretAngle, angleToTarget);
        return Mathf.Abs(angleDifference) <= _angleThreshold;
    }

    private void OnDrawGizmosSelected()//타워의 반경 그려줌(디버깅용, 인게임에는 안나옴)
    {
        // Debug.Log(transform.position);
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, range);
    }
    public void ActivateTurret()
    {
        isActivated = true;
    }
    public void DeactivateTurret()
    {
        isActivated = false;
    }
    // private IEnumerator StopAnimation()
    // {
    //     yield return new WaitForSeconds(3f); // 애니메이션 지속 시간 설정 (조정 가능)
    //     animator.enabled = false; // 애니메이션 종료
    // }
}
