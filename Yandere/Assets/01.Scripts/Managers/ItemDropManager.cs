using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DropContext
{
    public Vector3 position;
    public DropTable dropTable;
}

public class ItemDropManager : MonoBehaviour
{
    // 임시 디버깅 코드
    [SerializeField] private GameObject _defaultDropItemPrefab;

    public void HandleDrop(DropContext context)
    {
        if (context == null || context.dropTable == null)
        {
            Debug.LogWarning("[ItemDropManager] DropContext나 DropTable이 null입니다.");
            return;
        }

        foreach (var entry in context.dropTable.entries)
        {
            float roll = Random.Range(0f, 100f);
            if (roll <= entry.probability)
            {
                Instantiate(entry.itemPrefab, context.position, Quaternion.identity);
                Debug.Log($"[ItemDropManager] {entry.itemPrefab.name} 드롭됨 (확률 {entry.probability}%)");
            }
        }

        // 임시 디버깅 코드
        Instantiate(_defaultDropItemPrefab, context.position, Quaternion.identity);
    }
}
