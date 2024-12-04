using System.Collections;
using UnityEngine;

[System.Serializable]
public class MonsterSpawnData
{
    public GameObject monsterPrefab;
    public int spawnCount;
}

public class MonsterSpawner : MonoBehaviour
{
    [Header("Monster Prefabs")] [SerializeField]
    private MonsterSpawnData[] monsterSpawnDataArray; // 여러 몬스터 데이터를 관리할 배열

    [Header("Spawn Settings")] [SerializeField]
    private float spawnInterval = 2f;

    [SerializeField] private float spawnRadius = 1f; // 스폰 범위
    [SerializeField] private float spawnZPosition = -2f; // Z축 고정값 추가

    [Header("Spawner Info")] public int id;

    [Header("For Alert")] public GameObject alert;

    [Header("Handlers")] 
    public bool workable = false;
    public bool isWorking = false;
    private Coroutine spawnCoroutine;

    private void Start()
    {
        isWorking = false;
    }

    private void FixedUpdate()
    {
        // 게임 상태와 관련된 로직이 필요할 경우 GeneralManager와 연동 가능
        if (workable)
        {
            if (!isWorking)
            {
                if (GeneralManager.Instance.inGameManager.isWave && !GeneralManager.Instance.inGameManager.spawnEnd)
                {
                    spawnCoroutine = StartCoroutine(SpawnRoutine());
                    isWorking = true;
                }
            }

            if (GeneralManager.Instance.inGameManager.spawnEnd)
            {
                isWorking = false;
                StopCoroutine(spawnCoroutine);
            }
        }
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            // 스폰 배열에서 무작위로 선택하여 몬스터를 스폰
            foreach (var spawnData in monsterSpawnDataArray)
            {
                for (int i = 0; i < spawnData.spawnCount; i++)
                {
                    SpawnMonster(spawnData);
                }
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnMonster(MonsterSpawnData spawnData)
    {
        if (spawnData.monsterPrefab == null)
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
        GameObject monster = Instantiate(spawnData.monsterPrefab, spawnPosition, Quaternion.identity);

        // 몬스터에 플레이어 참조 연결
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Monster monsterComponent = monster.GetComponent<Monster>();
            if (monsterComponent != null)
            {
                monsterComponent.player = player.transform;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // 스폰 영역 시각화
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }

    public void ShowAlert()
    {
        alert.GetComponent<Animator>().SetBool("visible", true);
    }

    public void HideAlert()
    {
        alert.GetComponent<Animator>().SetBool("visible", false);
    }

    public int GetId()
    {
        return id % 100;
    }

    public int GetWaveId()
    {
        return id / 100;
    }

    public void SetWorkable(bool work)
    {
        workable = work;
    }

}
