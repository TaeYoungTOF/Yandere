using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private WaveData _currentSpawnData;
    private Coroutine _spawnRoutine;

    [Header("Spawn Settings")]
    [SerializeField] private float _spawnRadius = 10f;
    [SerializeField] private List<EnemySpawnWeigth> _spawnWeights;
    [SerializeField] private float _spawnInterval;
    [SerializeField] private int _spawnAmount;

    public void SetSpawnData(WaveData spawnData)
    {
        _currentSpawnData = spawnData;
        _spawnWeights = _currentSpawnData.enemyList;
        _spawnInterval = _currentSpawnData.spawnInterval;
        _spawnAmount = _currentSpawnData.spawnAmount;
    }

    public IEnumerator HandleWave(WaveData spawnData)
    {
        SetSpawnData(spawnData);

        yield return null;

        switch (spawnData.eventType)
        {
            case EventType.StartWave:
                Debug.Log("[SpawnManager] StartWave");
                _spawnRoutine = StartCoroutine(SpawnRoutine(_spawnInterval, _spawnAmount));
                yield return _spawnRoutine;
                break;
            case EventType.AddStrongerEnemy:
                Debug.Log("[SpawnManager] AddStrongerEnemy");
                _spawnRoutine = StartCoroutine(SpawnRoutine(_spawnInterval, _spawnAmount));
                yield return _spawnRoutine;
                break;
            case EventType.AddEliteEnemy:
                Debug.Log("[SpawnManager] AddEliteEnemy");
                _spawnRoutine = StartCoroutine(SpawnRoutine(_spawnInterval, _spawnAmount));
                yield return _spawnRoutine;
                break;
            case EventType.AddRangeEnemy:
                Debug.Log("[SpawnManager] AddRangeEnemy");
                _spawnRoutine = StartCoroutine(SpawnRoutine(_spawnInterval, _spawnAmount));
                yield return _spawnRoutine;
                break;
            case EventType.AddBossEnemy:
                Debug.Log("[SpawnManager] AddBossEnemy");
                _spawnRoutine = StartCoroutine(SpawnRoutine(_spawnInterval, _spawnAmount));
                yield return _spawnRoutine;
                break;
            default:
                Debug.LogWarning("[SpawnManager] Unknown EventType");
                break;
        }
    }

    private IEnumerator SpawnRoutine(float spawnInterval, int spawnAmount)
    {
        int count = 0;
        
        while (true)
        {
            int totalWeight = 0;
            foreach (var entry in _spawnWeights)
                totalWeight += entry.spawnWeight;

            foreach (var entry in _spawnWeights)
            {
                float ratio = (float)entry.spawnWeight / totalWeight;
                count = Mathf.Clamp(count,1, Mathf.RoundToInt(spawnAmount * ratio));

                for (int i = 0; i < count; i++)
                {
                    InstantiateEnemy(entry);
                }
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void InstantiateEnemy(EnemySpawnWeigth entry)
    {
        var position = GetRandomSpawnPosition();
        var instance = Instantiate(entry.enemyPrefab, position, Quaternion.identity, gameObject.transform);

        if (instance.TryGetComponent<EnemyController>(out var controller))
        {
            var dropContext = new DropContext { dropTable = entry.dropTable };
            controller.SetDropContext(dropContext);
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        Transform playerTransform = StageManager.Instance.Player.transform;

        if (!playerTransform)
        {
            Debug.LogWarning("[SpawnManager] Player Transform is null.");
            return Vector3.zero;
        }

        Vector2 center = playerTransform.position;
        Vector2 direction = Random.insideUnitCircle.normalized;
        float distance = _spawnRadius + Random.Range(0f, 5f);
        
        Vector2 spawnPos = center + direction * distance;
        
        return new Vector3(spawnPos.x, spawnPos.y, 0);
    }

    public void StopSpawn()
    {
        if (_spawnRoutine != null)
        {
            StopCoroutine(_spawnRoutine);
            _spawnRoutine = null;
        }
    }
}
