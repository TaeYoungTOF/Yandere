using UnityEngine;

public class DropContext
{
    public Vector3 Position;
    public DropTable DropTable;
}

public class ItemDropManager : MonoBehaviour
{
    public static ItemDropManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void HandleDrop(DropContext context)
    {
        if (context.DropTable == null || context.DropTable.entries.Count == 0)
            return;

        foreach (var entry in context.DropTable.entries)
        {
            if (Random.value <= entry.probability)
            {
                SpawnItem(entry.itemData, context.Position);
            }
        }
    }

    private void SpawnItem(ItemData itemData, Vector3 pos)
    {
        GameObject go = Instantiate(itemData.prefab, pos, Quaternion.identity);
        // 또는 오브젝트 풀링 사용 가능:
        // GameObject go = ObjectPool.Instance.Get(itemData.prefab);
        // go.transform.position = pos;
    }
}
