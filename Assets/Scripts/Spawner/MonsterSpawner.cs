using UnityEngine;
using System.Collections;

[System.Serializable]
public class MonsterSpawnData
{
    public GameObject monsterPrefab;
    public string monsterType;
    public int spawnCount;
}

public class MonsterSpawner : MonoBehaviour
{
    [Header("Monster Prefabs")]
    [SerializeField] private MonsterSpawnData defaultMonster;
    [SerializeField] private MonsterSpawnData tankerMonster;
    [SerializeField] private MonsterSpawnData assassinMonster;
    [SerializeField] private MonsterSpawnData adMonster;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private float spawnAngle = 180f; // 기본값: 아래 방향
    [SerializeField] private float spawnRadius = 1f;   // 스폰 범위
    
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;     // 이동 속도

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            // 각 몬스터 타입별로 스폰
            for (int i = 0; i < defaultMonster.spawnCount; i++)
                SpawnMonster(defaultMonster);
            
            for (int i = 0; i < tankerMonster.spawnCount; i++)
                SpawnMonster(tankerMonster);
            
            for (int i = 0; i < assassinMonster.spawnCount; i++)
                SpawnMonster(assassinMonster);
            
            for (int i = 0; i < adMonster.spawnCount; i++)
                SpawnMonster(adMonster);

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnMonster(MonsterSpawnData monsterData)
    {
        if (monsterData.monsterPrefab == null) return;

        // 스폰 위치 계산
        float randomOffset = Random.Range(-spawnRadius, spawnRadius);
        Vector2 spawnPosition = transform.position + new Vector3(randomOffset, 0, 0);

        // 몬스터 생성
        GameObject monster = Instantiate(monsterData.monsterPrefab, spawnPosition, Quaternion.identity);
        
        // 이동 방향 설정
        float angleInRadians = spawnAngle * Mathf.Deg2Rad;
        Vector2 direction = new Vector2(Mathf.Sin(angleInRadians), -Mathf.Cos(angleInRadians));
        
        // 몬스터에 이동 컴포넌트 추가
        MonsterMovement movement = monster.AddComponent<MonsterMovement>();
        movement.Initialize(direction, moveSpeed);
    }
}