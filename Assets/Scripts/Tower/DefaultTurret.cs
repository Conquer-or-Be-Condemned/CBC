using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class DefaultTurret : MonoBehaviour
{   
    [Header("Refereces")]
    [SerializeField] private Transform turretRotationPoint;//타워 회전 각도
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;

    [Header("Attributes")]
    // [SerializeField] 
    // private bool turretActivation = false; //공격력
    // [SerializeField] 
    // private float ATK; //공격력
    // [SerializeField] 
    // private float attackSpeed = 10.0f; //공격속도
    // [SerializeField] 
    // private float powerUse = 20.0f; //소모 전력량

    [SerializeField] private float range = 10f;//타워 사거리
    [SerializeField] private float rotationSpeed = 200f;//타워 회전 속도
    [SerializeField] private float fireRate = 1f;//발사 속도
    [SerializeField] private float damage;//공격력
    private Transform _target;
    private float _timeTilFire = -0.3f;
    private void Update() {
        if (_target == null) {
            FindTarget();
            return;
        }
        RotateTowardsTarget();
        if (!CheckTargetIsInRange()) {
            //Debug.Log("notinrange");
            _target = null;
            _timeTilFire = -0.3f;
        }
        else
        {
            //Debug.Log(_timeTilFire);
            _timeTilFire += Time.deltaTime;
            if (_timeTilFire >= (1f / fireRate))
            {
                Shoot();
                _timeTilFire = 0f;
            }
        }
    }

    
    private void Shoot()
    {
        GameObject bulletObj = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        Bullet bulletScript = bulletObj.GetComponent<Bullet>();
        bulletScript.SetTarget(_target);
        //Debug.Log("shoot!!");
    }
    private void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, range, (Vector2)transform.position, 0f, enemyMask);
        
        // Debug.Log("Hit count: " + hits.Length); // 디버깅용
        if (hits.Length > 0)
        {
            _target = hits[0].transform;
            // Debug.Log("Target found: " + _target.name); // 디버깅용
        }
    }

    private bool CheckTargetIsInRange()
    {
        
        return Vector2.Distance(_target.position, transform.position) <= range;
    }
    private void RotateTowardsTarget()
    {
        float angle = Mathf.Atan2(_target.position.y - transform.position.y, _target.position.x - transform.position.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f,0f,angle));
        turretRotationPoint.rotation = Quaternion.RotateTowards(turretRotationPoint.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, range);
    }


    

}