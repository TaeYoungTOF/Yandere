using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject_Spawner : MonoBehaviour
{
    [Header("스폰 설정")]
    [SerializeField] private GameObject Field_ItemObjectPrefab; // 항아리 프리팹
    [SerializeField] private int maxItemObjectPrefabCount = 10; // 동시에 존재 가능한 최대 개수
    [SerializeField] private float spawnInterval = 5f; // 스폰 간격 (초)

    [Header("스폰 범위")]
    [SerializeField] private Vector2 spawnAreaMin; // 좌하단 좌표
    [SerializeField] private Vector2 spawnAreaMax; // 우상단 좌표

    private List<GameObject> spawnedItemObjectPrefab = new List<GameObject>();

    private void Start()
    {
        Debug.Log("[Spawner] Start 호출됨"); // ✅ 디버그 추가
        SpawnItemObjectPrefab(); // ▶ 처음에 한 개 생성
        StartCoroutine(SpawnJarRoutine()); // ▶ 이후 일정 시간마다 계속 생성
    }

    private IEnumerator SpawnJarRoutine()
    {
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
        Vector3 spawnPos = GetRandomPosition();
        GameObject ItemObjectPrefab = Instantiate(Field_ItemObjectPrefab, spawnPos, Quaternion.identity);
        ItemObjectPrefab.transform.SetParent(transform);
        spawnedItemObjectPrefab.Add(ItemObjectPrefab);
    }

    private Vector3 GetRandomPosition()
    {
        float x = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
        float y = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
        return new Vector3(x, y, 0);
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        // 사각형 네 꼭짓점 구해서 선으로 그림
        Vector3 bottomLeft = new Vector3(spawnAreaMin.x, spawnAreaMin.y, 0);
        Vector3 topLeft = new Vector3(spawnAreaMin.x, spawnAreaMax.y, 0);
        Vector3 topRight = new Vector3(spawnAreaMax.x, spawnAreaMax.y, 0);
        Vector3 bottomRight = new Vector3(spawnAreaMax.x, spawnAreaMin.y, 0);

        Gizmos.DrawLine(bottomLeft, topLeft);
        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
    }
}
