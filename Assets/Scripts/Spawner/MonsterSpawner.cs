using UnityEngine;
using System.Collections;

[System.Serializable]
public class MonsterSpawnData
{
    public GameObject monsterPrefab;
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
    [SerializeField] private float spawnRadius = 1f;   // 스폰 범위
    [SerializeField] private float spawnZPosition = -2f;  // Z축 고정값 추가

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
        Debug.Log($"MonsterSpawner starting at position: {transform.position}");
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
        if (monsterData.monsterPrefab == null)
        {
            Debug.LogWarning("Monster prefab is null!");
            return;
        }

        // 스폰 위치 계산
        float randomOffset = Random.Range(-spawnRadius, spawnRadius);
        Vector3 spawnPosition = new Vector3(
            transform.position.x + randomOffset,
            transform.position.y,
            spawnZPosition
        );

        // 몬스터 생성
        GameObject monster = Instantiate(monsterData.monsterPrefab, spawnPosition, Quaternion.identity);
    
        // 스케일 명시적으로 설정 (프리팹에서 보이는 0.5994 값 사용)
        Vector3 desiredScale = new Vector3(0.5994f, 0.5994f, 0.5994f);
        monster.transform.localScale = desiredScale;
    
        // 디버그 로그 추가
        Debug.Log($"Monster spawned with scale: {monster.transform.localScale}");
    
        // 플레이어 찾아서 할당
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Monster monsterComponent = monster.GetComponent<Monster>();
            if (monsterComponent != null)
            {
                monsterComponent.player = player.transform;
                Debug.Log($"Player reference set for monster at {monster.transform.position}");
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
        
        // 스폰 영역 시각화
        Vector3 spawnAreaStart = new Vector3(transform.position.x - spawnRadius, transform.position.y, spawnZPosition);
        Vector3 spawnAreaEnd = new Vector3(transform.position.x + spawnRadius, transform.position.y, spawnZPosition);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(spawnAreaStart, spawnAreaEnd);
    }
}