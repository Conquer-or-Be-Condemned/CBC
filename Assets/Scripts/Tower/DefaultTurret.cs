using System.Collections;
using System.Collections.Generic;
using Tower;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;

public abstract class DefaultTurret : MonoBehaviour, ActivateTower
{   
    
    protected Transform turretRotationPoint; // 타워 회전 각도
    protected LayerMask enemyMask;           //raycast 감지 Layer
    protected Animator animator;             //타워 부분 Animator
    protected SpriteRenderer gunRenderer;    //과열시 색 변화
    // protected GameObject bulletPrefab;       //총알 오브젝트 생성 위한 변수
    // protected Transform bulletSpawnPoint1;    //총알 스폰 지점
    // protected Transform bulletSpawnPoint2;    //총알 스폰 지점
    // protected Transform bulletFireDirection1;    //총 격발 방향
    // protected Transform bulletFireDirection2;    //총 격발 방향
    // [SerializeField] private Transform bulletSpawnPoint3;    //총알 스폰 지점
    
    //[SerializeField] private GameObject towerPrefab;
    
   
    protected float range;         // 타워 사거리
    protected float rotationSpeed;// 타워 회전 속도
    protected float fireRate;       // 발사 속도, 충격발 애니메이션이랑 연동시키기? ㄱㄴ?
    protected int power;            //타워 사용 전력량
    protected float overHeatTime;    //~초 격발시 과열
    protected float coolTime;        //~초 지나면 냉각
    protected Transform _target;          //target of bullets
    
    // [SerializeField] private float damage; // 공격력
    
    //-------------------------------------------------------
    public bool isActivated = false;//타워 가동 여부
    public bool _previousIsActivated = false;//버퍼(토글 확인)
    //-------------------------------------------------------
    private GameObject _originPower;    //ControlUnitStatus Script의 함수사용
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
    
    
    private void Awake()
    {
        _originPower = GameObject.Find("ControlUnit");
        _cus = _originPower.GetComponent<ControlUnitStatus>();//제어장치 정보 가져오기 위함
        // GameObject bulletObj = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        // _bulletScript = bulletObj.GetComponent<Bullet>();
    }
    private void Update()
    {
        CheckToggle();//사용자에 의한 타워 가동 토글 확인
        TowerIsActivatedNow();//사용자에 의해 타워가 가동 됐다면 역할 수행
        
    }

    protected void TowerIsActivatedNow()//사용자에 의해 타워가 가동 됐다면 역할 수행(Update에서 수행)
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
        if (_target == null)
        {
            _fireTime -= Time.deltaTime;
            if(_fireTime <= 0f) _fireTime = 0f;
            animator.SetBool("isShoot", false);
            FindTarget();//(Overlap 사용)
            return;
        }
    }

    private void FireRateController()//총알 객체화 후 발사 동작 수행(TowerIsActivatedNow에서 수행)
    {
        if (!CheckTargetIsInRange())//적이 범위에 없음
        {
            _fireTime -= Time.deltaTime;
            if(_fireTime <= 0f) _fireTime = 0f;
            animator.SetBool("isShoot", false);
            _target = null;
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
                animator.SetBool("isShoot", false);
                StartCoroutine(OverHeat());
            }
            else
            {
                animator.SetBool("isShoot", true); 
            }
        }
        else//적이 사격 시야에 없음
        {
            _fireTime -= Time.deltaTime;
            if(_fireTime <= 0f) _fireTime = 0f;
            animator.SetBool("isShoot", false);
        }
    }
    protected void CheckToggle()//Checks toggle of isActivated
    {
        if (isActivated != _previousIsActivated)//toggle check
        {
            if (isActivated)
            {
                _previousIsActivated = isActivated; // 이전 상태를 현재 상태로 업데이트

                // Debug.Log("Activated");
                AddTurret();
            }
            else if (isActivated == false)
            {
                animator.SetBool("isShoot", false);
                StartCoroutine(DeactivateProcess());
                // _fireTime = 0f;

                _previousIsActivated = isActivated; // 이전 상태를 현재 상태로 업데이트

                // Debug.Log("DeActivated");
                DeleteTurret();
            }
            
        }
    }
    private IEnumerator DeactivateProcess()
    {
        // _totCoolTime = _fireTime;
        while (_fireTime >= 0f)
        {
            gunRenderer.color = new Color(1f,((coolTime - _fireTime) / coolTime),((coolTime - _fireTime) / coolTime));
            _fireTime -= Time.deltaTime;
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

    protected abstract void Shoot();//총알 객체화 후 목표로 발사(FireRateController에서 수행)


    private void FindTarget()//raycast를 이용한 적 타워 반경 접근 확인 후 배열 추가(NoTargetInRange에서 적을 찾기위해 수행)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, range);
        foreach (var monster in hits)
        {
            if (monster.CompareTag("Enemy"))
            {
                if(!_target)_target = monster.transform;
                else return;
            }
        }
    }

    private bool CheckTargetIsInRange()//적이 사거리에 있는지 확인(FireRateController에서 수행)
    {
        if (_target == null) return false;
        return Vector2.Distance(_target.position, transform.position) <= range;
    }

    private void RotateTowardsTarget()//적향해 타워 z축 회전(TowerIsActivatedNow에서 수행)
    {
        if (_target != null)
        {
            float angle =
                Mathf.Atan2(_target.position.y - transform.position.y, _target.position.x - transform.position.x) *
                Mathf.Rad2Deg - 90f;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
            turretRotationPoint.rotation = Quaternion.RotateTowards(turretRotationPoint.rotation, targetRotation,
                rotationSpeed * Time.deltaTime);
        }
    }

    private bool IsTargetInSight()//적이 시야각에 있는지 확인(FireRateController, OverHeatAnimationController에서 수행)
    {
        if (_target == null) return false;
        float angleToTarget = Mathf.Atan2(_target.position.y - transform.position.y, _target.position.x - transform.position.x) * Mathf.Rad2Deg - 90f;
        float turretAngle = turretRotationPoint.eulerAngles.z;
        float angleDifference = Mathf.DeltaAngle(turretAngle, angleToTarget);
        return Mathf.Abs(angleDifference) <= _angleThreshold;
    }

    private void OnDrawGizmosSelected()//타워의 반경 그려줌(디버깅용, 인게임에는 안나옴)
    {
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
