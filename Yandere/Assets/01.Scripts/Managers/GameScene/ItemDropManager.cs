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
        // if (context == null || context.dropTable == null)
        // {
        //     Debug.LogWarning("[ItemDropManager] DropContext나 DropTable이 null입니다.");
        //     return;
        // }
        //
        // foreach (var entry in context.dropTable.entries)
        // {
        //     float roll = Random.Range(0f, 100f);
        //     if (roll <= entry.probability)
        //     {
        //         
        //         var ItemGO =
        //             ObjectPoolManager.Instance.GetFromPool(PoolType.Item, context.position, Quaternion.identity);
        //         if (ItemGO.TryGetComponent<Item>(out var item))
        //         {
        //             if (entry.itemData == null)
        //             {
        //                 Debug.LogError("[ItemDropManager] DropTable의 itemData가 null입니다.");
        //                 return;
        //             }
        //
        //             item.Initialize(entry.itemData);
        //             item.SetPickupDelay(entry.pickupDelay);
        //         }
        //         else
        //         {
        //             Debug.LogError("[ItemDropManager] ObjectPool에서 꺼낸 프리팹에 'Item' 컴포넌트가 없습니다.");
        //         }     
        //     }
        // }
        
        if (context == null || context.dropTable == null)
        {
            Debug.LogWarning("[ItemDropManager] DropContext나 DropTable이 null입니다.");
            return;
        }

        DropTable dropTable = context.dropTable;

        switch (dropTable.dropMode)
        {
            case DropMode.DropAll:
                foreach (var entry in dropTable.entries)
                {
                    float roll = Random.Range(0f, 100f);
                    if (roll <= entry.probability)
                    {
                        SpawnItem(entry, context.position);
                    }
                }
                break;

            case DropMode.DropOne:
                float totalProbability = 0f;
                foreach (var entry in dropTable.entries)
                    totalProbability += entry.probability;

                if (totalProbability <= 0f)
                {
                    Debug.LogWarning("[ItemDropManager] DropOne 모드인데 총 확률이 0입니다.");
                    return;
                }

                float randomPoint = Random.Range(0f, totalProbability);
                float cumulative = 0f;

                foreach (var entry in dropTable.entries)
                {
                    cumulative += entry.probability;
                    if (randomPoint <= cumulative)
                    {
                        SpawnItem(entry, context.position);
                        break;
                    }
                }
                break;
        }
        
    } 
    private void SpawnItem(DropEntry entry, Vector3 position)
    {
        if (entry.itemData == null)
        {
            Debug.LogError("[ItemDropManager] DropTable의 itemData가 null입니다.");
            return;
        }

        GameObject itemGO = ObjectPoolManager.Instance.GetFromPool(PoolType.Item, position, Quaternion.identity);

        if (itemGO.TryGetComponent<Item>(out var item))
        {
            item.Initialize(entry.itemData);
            item.SetPickupDelay(entry.pickupDelay);
        }
        else
        {
            Debug.LogError("[ItemDropManager] ObjectPool에서 꺼낸 프리팹에 'Item' 컴포넌트가 없습니다.");
        }
    }
}
