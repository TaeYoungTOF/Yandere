using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Spawn Settings")]

    [SerializeField] private List<Rect> spawnAreas; // 적을 생성할 영역 리스트
    [SerializeField] private Color gizmoColor = new Color(1, 0, 0, 0.3f); // 기즈모 색상
    [SerializeField] private float spawnInterval = 0.2f;
    [SerializeField] private float waveInterval = 1f;

    private void Start()
    {
        // StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnEnemy()
    {
        GameObject enemy = ObjectPoolManager.Instance.GetFromPool((int)PoolType.DefaultEnemy);

        if (enemy != null)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();
            enemy.transform.position = spawnPosition;
            enemy.SetActive(true);

            /**@todo
            Enemy 초기화*/
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        if (spawnAreas.Count == 0)
            return transform.position;

        Rect area = spawnAreas[Random.Range(0, spawnAreas.Count)];
        float x = Random.Range(area.xMin, area.xMax);
        float y = Random.Range(area.yMin, area.yMax);

        return new Vector3(x, y, 0f); // 2D 기준
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = gizmoColor;

        foreach (var area in spawnAreas)
        {
            Vector3 center = new Vector3(area.center.x, area.center.y, 0f);
            Vector3 size = new Vector3(area.width, area.height, 0.1f);
            Gizmos.DrawCube(center, size);
        }
    }
}
