using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;

public class CamoTurret : MonoBehaviour
{   
    [Header("References")]
    [SerializeField] private Transform turretRotationPoint; // 타워 회전 각도
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private Animator animator;
    //[SerializeField] private GameObject towerPrefab;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private SpriteRenderer gunRenderer;
    [Header("Attributes")]
    [SerializeField] private float range = 10f; // 타워 사거리
    [SerializeField] private float rotationSpeed = 200f; // 타워 회전 속도
    [SerializeField] private float fireRate = 1f; // 발사 속도
    [SerializeField] private int power = 60;
    public bool isActivated = false;
    [SerializeField]private bool _previousIsActivated = false;

    // [SerializeField] private float damage; // 공격력
    private GameObject OriginPower;
    private GameObject _gunPrefab;
    private Transform _target;
    private SpriteRenderer _gunSprite;
    private float _timeTilFire;
    private float _angleThreshold = 5f; // 타워와 적의 각도 차이 허용 범위 (조정 가능)
    private float _fireTime = 0f;//과열시 중지 위한 변수
    // private bool isActivated = false;
    // private bool _previousIsActivated = false;

    private void Start()
    {
        
        
        OriginPower = GameObject.Find("ControlUnit");
        // ControlUnitStatus cus = gameObject.GetComponent<ControlUnitStatus>();
        //GameObject tw = Instantiate(towerPrefab, Vector3.zero, Quaternion.identity);

    }
    private void Update() 
    {
        if (isActivated != _previousIsActivated)
        {
            if (isActivated)
            {
                _previousIsActivated = isActivated; // 이전 상태를 현재 상태로 업데이트

                Debug.Log("Activated");
                AddTurret();
            }
            else if (isActivated == false)
            {
                animator.SetBool("isShoot", false);

                _previousIsActivated = isActivated; // 이전 상태를 현재 상태로 업데이트

                Debug.Log("DeActivated");
                DeleteTurret();
            }
            
        }
        if (isActivated)
        {
            
            
            if (_target == null)
            {
                animator.SetBool("isShoot", false);
                FindTarget();
                return;
            }

            RotateTowardsTarget();

            if (!CheckTargetIsInRange())
            {
                animator.SetBool("isShoot", false);
                _target = null;
                _timeTilFire = 0f;
            }
            else
            {
                _timeTilFire += Time.deltaTime;
                if (_timeTilFire >= (1f / fireRate) && IsTargetInSight())
                {
                    //animator.enabled = true;

                    Shoot();
                    // animator.enabled = false;
                    _timeTilFire = 0f;

                }

            }

            if (IsTargetInSight())
            {
                _fireTime += Time.deltaTime;
                if (_fireTime >= 5f)
                {
                    isActivated = false;
                    _previousIsActivated = false;
                    animator.SetBool("isShoot", false);
                    gunRenderer.color = Color.red;
                    StartCoroutine(OverHeat());
                }
                else
                {
                    animator.SetBool("isShoot", true); 
                }
            }
            else
            {
                animator.SetBool("isShoot", false);
            }
        }
    }

    private IEnumerator OverHeat()
    {
        animator.SetBool("isShoot", false);
        yield return new WaitForSeconds(5f);
        gunRenderer.color = Color.white;
        _fireTime = 0f;
        isActivated = true;
        _previousIsActivated = true;
    }
    private void FixedUpdate()
    {
        
    }

    private void AddTurret()
    {
        
        
        if (OriginPower.GetComponent<ControlUnitStatus>().getCurrentPower() >= power)
        {
            OriginPower.GetComponent<ControlUnitStatus>().AddUnit(power);  
        }
        else
        {
            isActivated = false;
            _previousIsActivated = false;
        }
        
    }
    private void DeleteTurret()
    {
        

        OriginPower.GetComponent<ControlUnitStatus>().RemoveUnit(power);
    }
    private void Shoot()
    {
        animator.enabled = true; // 발사할 때 애니메이션 시작
        GameObject bulletObj = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        Bullet bulletScript = bulletObj.GetComponent<Bullet>();
        bulletScript.SetTarget(_target);

        // 애니메이션을 짧은 시간 뒤에 종료
        // StartCoroutine(StopAnimation());
    }

    // private IEnumerator StopAnimation()
    // {
    //     yield return new WaitForSeconds(3f); // 애니메이션 지속 시간 설정 (조정 가능)
    //     animator.enabled = false; // 애니메이션 종료
    // }

    private void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, range, Vector2.zero, 0f, enemyMask);
        if (hits.Length > 0)
        {
            _target = hits[0].transform;
        }
    }

    private bool CheckTargetIsInRange()
    {
        return Vector2.Distance(_target.position, transform.position) <= range;
    }

    private void RotateTowardsTarget()
    {
        float angle = Mathf.Atan2(_target.position.y - transform.position.y, _target.position.x - transform.position.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        turretRotationPoint.rotation = Quaternion.RotateTowards(turretRotationPoint.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private bool IsTargetInSight()
    {
        float angleToTarget = Mathf.Atan2(_target.position.y - transform.position.y, _target.position.x - transform.position.x) * Mathf.Rad2Deg - 90f;
        float turretAngle = turretRotationPoint.eulerAngles.z;
        float angleDifference = Mathf.DeltaAngle(turretAngle, angleToTarget);
        return Mathf.Abs(angleDifference) <= _angleThreshold;
    }

    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, range);
    }
    
}