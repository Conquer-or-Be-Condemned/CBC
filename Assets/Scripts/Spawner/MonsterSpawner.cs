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

        // 랜덤 스폰 위치 계산 (원형으로 스폰)
        float randomAngle = Random.Range(0f, 360f);
        Vector2 spawnOffset = new Vector2(
            Mathf.Cos(randomAngle * Mathf.Deg2Rad),
            Mathf.Sin(randomAngle * Mathf.Deg2Rad)
        ) * spawnRadius;
        
        Vector2 spawnPosition = (Vector2)transform.position + spawnOffset;

        // 몬스터 생성
        GameObject monster = Instantiate(monsterData.monsterPrefab, spawnPosition, Quaternion.identity);
        
        // Monster 컴포넌트 가져오기 및 플레이어 참조 설정
        Monster monsterComponent = monster.GetComponent<Monster>();
        if (monsterComponent != null)
        {
            // 플레이어 찾아서 할당
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                monsterComponent.player = player.transform;
            }
        }
    }

    // 디버그용 기즈모
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}