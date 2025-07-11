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
                var ItemGO = Instantiate(entry.itemPrefab, context.position, Quaternion.identity);
                if (ItemGO.TryGetComponent<Item>(out var item))
                        item.SetPickupDelay(entry.pickupDelay);
                
                Debug.Log($"[ItemDropManager] {entry.itemPrefab.name} 드롭됨 (확률 {entry.probability}%)");
            }
        }
    }
}
