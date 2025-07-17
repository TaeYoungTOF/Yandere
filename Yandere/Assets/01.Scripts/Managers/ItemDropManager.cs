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
                
                var ItemGO =
                    ObjectPoolManager.Instance.GetFromPool(PoolType.Item, context.position, Quaternion.identity);
                if (ItemGO.TryGetComponent<Item>(out var item))
                {
                    if (entry.itemData == null)
                    {
                        Debug.LogError("[ItemDropManager] DropTable의 itemData가 null입니다.");
                        return;
                    }

                    item.Initialize(entry.itemData);
                    item.SetPickupDelay(entry.pickupDelay);
                }
                else
                {
                    Debug.LogError("[ItemDropManager] ObjectPool에서 꺼낸 프리팹에 'Item' 컴포넌트가 없습니다.");
                }     
            }
        }
    }   
}
