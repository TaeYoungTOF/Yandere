using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private WaveData _currentSpawnData;
    private Coroutine _spawnRoutine;
    
    [Header("Object Settings")]
    [SerializeField] private int maxItemObjectPrefabCount = 10; // 동시에 존재 가능한 최대 개수
    [SerializeField] private float spawnInterval = 5f; // 스폰 간격 (초)
    private List<GameObject> spawnedItemObjectPrefab = new List<GameObject>();

    [Header("Spawn Settings")]
    [SerializeField] private float _spawnRadius = 10f;
    [SerializeField] private List<EnemySpawnWeigth> _spawnWeights;
    [SerializeField] private float _spawnInterval;
    [SerializeField] private int _spawnAmount;
    [SerializeField] private Vector2 _spawnAreaMin; // 좌하단 좌표
    [SerializeField] private Vector2 _spawnAreaMax; // 우상단 좌표

    public void SetSpawnArea(Vector2 spawnAreaMin, Vector2 spawnAreaMax)
    {
        _spawnAreaMin = spawnAreaMin;
        _spawnAreaMax = spawnAreaMax;
    }

    private void SetSpawnData(WaveData spawnData)
    {
        _currentSpawnData = spawnData;
        _spawnWeights = _currentSpawnData.enemyList;
        _spawnInterval = _currentSpawnData.spawnInterval;
        _spawnAmount = _currentSpawnData.spawnAmount;
    }

    public IEnumerator HandleWave(WaveData waveData)
    {
        SetSpawnData(waveData);

        yield return null;

        switch (waveData.eventType)
        {
            case EventType.StartWave:
            case EventType.AddStrongerEnemy:
            case EventType.AddEliteEnemy:
            case EventType.AddRangeEnemy:
                _spawnRoutine = StartCoroutine(SpawnRoutine(_spawnInterval, _spawnAmount));
                yield return _spawnRoutine;
                break;
            case EventType.AddBossEnemy:
                _spawnRoutine = StartCoroutine(SpawnBoss());
                yield return _spawnRoutine;
                break;
            default:
                Debug.LogWarning("[SpawnManager] Unknown EventType");
                break;
        }
    }

    private IEnumerator SpawnRoutine(float spawnInterval, int spawnAmount)
    {
        while (true)
        {
            for (int i = 0; i < spawnAmount; i++)
            {
                var entry = GetWeightedRandomEntry();
                InstantiateEnemy(entry);
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private IEnumerator SpawnBoss()
    {
        var entry = GetWeightedRandomEntry();
        InstantiateEnemy(entry);

        yield return null;
    }
    
    private EnemySpawnWeigth GetWeightedRandomEntry()
    {
        int totalWeight = 0;
        foreach (var entry in _spawnWeights)
            totalWeight += entry.spawnWeight;

        int rand = Random.Range(0, totalWeight);
        int cumulative = 0;

        foreach (var entry in _spawnWeights)
        {
            cumulative += entry.spawnWeight;
            if (rand < cumulative)
                return entry;
        }

        // fallback, theoretically unreachable
        return _spawnWeights[0];
    }

    private void InstantiateEnemy(EnemySpawnWeigth entry)
    {
        var position = GetRandomSpawnPosition();
        //GameObject instance = ObjectPoolManager.Instance.GetFromPool(PoolType.Enemy, position, Quaternion.identity);
        var instance = Instantiate(entry.enemyPrefab, position, Quaternion.identity, gameObject.transform);

        if (instance.TryGetComponent<EnemyController>(out var controller))
        {
            var dropContext = new DropContext { dropTable = entry.dropTable };
            controller.SetDropContext(dropContext);
        }
    }

    public IEnumerator SpawnJarRoutine()
    {
        yield return null; // 1프레임 대기
        
        SpawnItemObjectPrefab(); // ▶ 처음에 한 개 생성
        
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            // 현재 개수가 최대보다 적으면 생성
            if (spawnedItemObjectPrefab.Count < maxItemObjectPrefabCount)
            {
                SpawnItemObjectPrefab();
            }

            // 리스트에서 null(파괴된) 오브젝트 정리
            spawnedItemObjectPrefab.RemoveAll(jar => jar == null);
        }
    }

    private void SpawnItemObjectPrefab()
    {
        Vector3 spawnPos = GetRandomSpawnPosition();
        GameObject ItemObjectPrefab = ObjectPoolManager.Instance.GetFromPool(PoolType.FieldObject, spawnPos, Quaternion.identity);
        ItemObject item = ItemObjectPrefab.GetComponent<ItemObject>();
        item.Init();
        
        spawnedItemObjectPrefab.Add(ItemObjectPrefab);
    }

    private Vector3 GetRandomSpawnPosition()
    {
        Transform playerTransform = StageManager.Instance.Player.transform;

        if (!playerTransform)
        {
            Debug.LogWarning("[SpawnManager] Player Transform is null.");
            return Vector3.zero;
        }

        const int maxAttempts = 50; // 무한 루프 방지
        const float colliderCheckRadius = 0.5f; // 충돌 검사 범위 (적 크기에 맞춰 조절)
        int attempt = 0;

        while (attempt < maxAttempts)
        {
            attempt++;

            float randomX = Random.Range(_spawnAreaMin.x, _spawnAreaMax.x);
            float randomY = Random.Range(_spawnAreaMin.y, _spawnAreaMax.y);
            Vector2 spawnPos = new Vector2(randomX, randomY);

            // 해당 위치에 Collider가 있는지 검사
            LayerMask mapLayer = LayerMask.GetMask("Map");
            if (Physics2D.OverlapCircle(spawnPos, colliderCheckRadius, mapLayer) != null)
            {
                continue;
            }

            // 플레이어와의 거리 검사
            if (Vector2.Distance(spawnPos, playerTransform.position) < _spawnRadius)
            {
                continue;
            }

            return new Vector3(spawnPos.x, spawnPos.y, 0);
        }

        // fallback
        Debug.LogWarning("[SpawnManager] Failed to find Spawn Position");
        return playerTransform.position + Vector3.right * _spawnRadius;
    }

    public void StopSpawn()
    {
        if (_spawnRoutine != null)
        {
            StopCoroutine(_spawnRoutine);
            _spawnRoutine = null;
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        // 사각형 네 꼭짓점 구해서 선으로 그림
        Vector3 bottomLeft = new Vector3(_spawnAreaMin.x, _spawnAreaMin.y, 0);
        Vector3 topLeft = new Vector3(_spawnAreaMin.x, _spawnAreaMax.y, 0);
        Vector3 topRight = new Vector3(_spawnAreaMax.x, _spawnAreaMax.y, 0);
        Vector3 bottomRight = new Vector3(_spawnAreaMax.x, _spawnAreaMin.y, 0);

        Gizmos.DrawLine(bottomLeft, topLeft);
        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
    }
}
