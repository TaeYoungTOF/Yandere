using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab; 

    [Header("Spawn Settings")]

    [SerializeField] private List<Rect> spawnAreas; // 적을 생성할 영역 리스트
    //[SerializeField] private Color gizmoColor = new Color(1, 0, 0, 0.3f); // 기즈모 색상
    [SerializeField] private float spawnInterval = 0.2f;
    [SerializeField] private float waveInterval = 1f;
    [SerializeField] private float spawnRadius = 3f;

    private Transform playerTransform;

    public IEnumerator SpawnRoutine()
    {
        playerTransform = StageManager.Instance.Player.transform;

        while (true)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnEnemy()
    {
        /**
        GameObject enemy = ObjectPoolManager.Instance.GetFromPool((int)PoolType.DefaultEnemy);

        if (enemy != null)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();
            enemy.transform.position = spawnPosition;
            enemy.SetActive(true);

            /**@todo
            Enemy 초기화
        }*/
        Debug.Log("[SpawnManager] Spawn");
        Instantiate(enemyPrefab, GetRandomSpawnPosition(), Quaternion.identity);
    }

    private Vector3 GetRandomSpawnPosition()
    {
        if (playerTransform == null)
        {
            Debug.LogWarning("[SpawnManager] Player Transform is null.");
            return Vector3.zero;
        }

        Vector2 center = playerTransform.position;

        float x = Random.Range(-spawnRadius, spawnRadius) + center.x;
        float yOffset = Mathf.Sqrt(Mathf.Pow(spawnRadius, 2) - Mathf.Pow(x - center.x, 2));
        yOffset *= Random.Range(0, 2) == 0 ? -1 : 1;

        float y = center.y + yOffset;

        return new Vector3(x, y, 0);
    }

    /**
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = gizmoColor;

        foreach (var area in spawnAreas)
        {
            Vector3 center = new Vector3(area.center.x, area.center.y, 0f);
            Vector3 size = new Vector3(area.width, area.height, 0.1f);
            Gizmos.DrawCube(center, size);
        }
    }*/
}
