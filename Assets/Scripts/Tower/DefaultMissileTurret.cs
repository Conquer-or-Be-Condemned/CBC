using System;
using System.Collections;
using System.Collections.Generic;
using Tower;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;
using UnityEngine.Serialization;

public abstract class DefaultMissileTurret : MonoBehaviour, IActivateTower
{   
    //-------------------------------------------------------
    public bool isActivated = false;//타워 가동 여부
    [FormerlySerializedAs("_previousIsActivated")] public bool previousIsActivated = false;//버퍼(토글 확인)
    //-------------------------------------------------------
    protected Transform TurretRotationPoint;//타워 회전 각도
    protected Transform Target1;            //target of missile1
    protected Transform Target2;            //target of missile2
    protected Transform Target3;            //target of missile3
    protected Transform Target4;            //target of missile4
    protected Transform Target5;            //target of missile5
    protected Transform Target6;            //target of missile6
    protected Animator Animator;            //타워 부분 Animator
    protected SpriteRenderer GunRenderer;   //과열시 색 변화
    protected String Name;                  //타워이름
    
    protected float Range;                  //타워 사거리
    protected float FireRate;               //발사 속도, 충격발 애니메이션이랑 연동시키기? ㄱㄴ?
    protected int Power;                    //타워 사용 전력량
    protected int OverHeatMissileCount;     //~초 격발시 과열
    protected int Level;
    protected int RPM;
    protected int Damage;
    protected float CurMissileCount = 0f;   //과열시 중지 위한 변수
    
    private GameObject _originPower;        //ControlUnitStatus Script의 함수사용
    private ControlUnitStatus _cus;         //_cus = _OriginPower.GetComponent<ControlUnitStatus>();
    private float _timeTilFire;             //다음 발사까지의 시간
    private float _angleThreshold = 360f;   // 타워와 적의 각도 차이 허용 범위 (조정 가능)
    private float _totCoolTime;             //냉각시 누적 냉각시간
    
    //Override Methods---------------------------
    protected abstract void Shoot();
    protected abstract void FindTarget();
    protected abstract void RotateTowardsTarget();//적향해 타워 z축 회전(TowerIsActivatedNow에서 수행)
    //--------------------------------------------
    private void Awake()
    {
        _originPower = GameObject.Find("ControlUnit");
        _cus = _originPower.GetComponent<ControlUnitStatus>();//제어장치 정보 가져오기 위함
    }
    private void Update()
    {
        CheckToggle();//사용자에 의한 타워 가동 토글 확인
        TowerIsActivatedNow();//사용자에 의해 타워가 가동 됐다면 역할 수행
        
    }
    private void CheckToggle()//Checks toggle of isActivated
    {
        if (isActivated != previousIsActivated)//toggle check
        {
            if (isActivated)
            {
                previousIsActivated = isActivated; // 이전 상태를 현재 상태로 업데이트
                AddTurret();
            }
            else if (isActivated == false)
            {
                
                Animator.SetBool("isShoot", false);
                previousIsActivated = isActivated; // 이전 상태를 현재 상태로 업데이트
                StartCoroutine(DeactivateProcess());
                DeleteTurret();
            }
        }
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
        if (Target1 == null)
        {
            FindTarget();//(raycast 사용)
        }
    }
    private void FireRateController()//총알 객체화 후 발사 동작 수행(TowerIsActivatedNow에서 수행)
    {
        if (!CheckTargetIsInRange())//적이 범위에 없음
        {
            Target1 = null;
            Target2 = null;
            Target3 = null;
            Target4 = null;
            Target5 = null;
            Target6 = null;
            _timeTilFire = 0f;
        }
        else//적이 범위에 있음
        {
            _timeTilFire += Time.deltaTime;
            if (_timeTilFire >= (1f / FireRate) && IsTargetInSight())//적이 타워의 시야각에 있고 RPS만큼 발사
            {
                Shoot();
                _timeTilFire = 0f;
            }
        }
    }
    private void OverHeatAnimationController()//설정시간 도달 시 과열(TowerIsActivatedNow에서 수행)
    {
        GunRenderer.color = new Color(1f,(255f-255f* (CurMissileCount / OverHeatMissileCount))/255f,(255f-255f*
            (CurMissileCount / OverHeatMissileCount))/255f);
        if (IsTargetInSight())//적이 사격 시야에 있음
        {
            if (CurMissileCount >= OverHeatMissileCount)//터렛 과열
            {
                isActivated = false;
                previousIsActivated = false;
                StartCoroutine(OverHeat());
            }
        }
    }
    private bool CheckTargetIsInRange()//적이 사거리에 있는지 확인(FireRateController에서 수행)
    {
        if (Target1 == null) return false;
        return Vector2.Distance(Target1.position, transform.position) <= Range;
    }
    private bool IsTargetInSight()//적이 시야각에 있는지 확인(FireRateController, OverHeatAnimationController에서 수행)
    {
        if(Target1==null) return false;
        float angleToTarget = Mathf.Atan2(Target1.position.y - transform.position.y, Target1.position.x - transform.position.x) * Mathf.Rad2Deg - 90f;
        float turretAngle = TurretRotationPoint.eulerAngles.z;
        float angleDifference = Mathf.DeltaAngle(turretAngle, angleToTarget);
        return Mathf.Abs(angleDifference) <= _angleThreshold;
        
    }
    //for Control Unit----------------------------------------------------------------------
    private void AddTurret()//ControlUnitStatus script 사용(CheckToggle에서 수행)
    {
        if (_cus.GetCurrentPower() >= Power)
        {
            _cus.AddUnit(Power);
        }
        else
        {
            isActivated = false;
            previousIsActivated = false;
        }
    }
    private void DeleteTurret()//ControlUnitStatus script 사용(CheckToggle에서 수행)
    {
        _cus.RemoveUnit(Power);
    }
    //Coroutine Methods------------------------------------------------------------------
    private IEnumerator DeactivateProcess()
    {
        while (CurMissileCount>=0)
        {
            GunRenderer.color = new Color(1f,(255f-255f* (CurMissileCount / OverHeatMissileCount))/255f,(255f-255f*
                (CurMissileCount / OverHeatMissileCount))/255f);
            CurMissileCount -= Time.deltaTime;
            yield return null;
        }
        GunRenderer.color = new Color(0.5f, 0.5f, 0.5f);
    }
    private IEnumerator OverHeat()//코루틴 함수 냉각 역할 수행(OverHeatAnimationController에서 수행)
    {
        while (CurMissileCount>=0)
        {
            GunRenderer.color = new Color(1f,(255f-255f* (CurMissileCount / OverHeatMissileCount))/255f,(255f-255f*
                (CurMissileCount / OverHeatMissileCount))/255f);
            CurMissileCount -= Time.deltaTime;
            yield return null;
        }
        Animator.SetBool("isShoot", false);
        GunRenderer.color = Color.white;
        CurMissileCount = 0f;
        isActivated = true;
        previousIsActivated = true;
    }
    protected IEnumerator ShootAnimation()
    {
        Animator.SetBool("isShoot",true);
        yield return new WaitForSeconds(0.6f);
        Animator.SetBool("isShoot",false);
    }
    //---------------------------------------------------------------------------
    //For UI----------------------------------
    public void ActivateTurret()
    {
        isActivated = true;
    }
    public void DeactivateTurret()
    {
        isActivated = false;
    }
    
    //Getter
    public String GetName()
    {
        return Name;
    }

    public int GetLevel()
    {
        return Level;
    }

    public int GetPower()
    {
        return Power;
    }
    public int GetRPM()
    {
        return RPM;
    }
    public int GetDamage()
    {
        return Damage;
    }
    public int GetRpm()
    {
        return RPM;
    }
    //  TODO : 데미지 정보를 만들어야 함.
   
}