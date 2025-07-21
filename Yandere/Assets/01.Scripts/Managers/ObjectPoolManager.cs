using System.Collections.Generic;
using UnityEngine;

public enum PoolType
{
   EnemyBullet,
   PlayerBullet,
   Enemy,
   Item,
   FieldObject,
   
   //스킬
   FireballProj,
   BurstingGazeProj,
   ParchedLongingProj,
   RagingEmotionsProj,
   EtchedHatredProj,
   PouringAffectionProj,
}

[System.Serializable]
public class PoolPrefabsEntry
{
    public PoolType PoolType;
    public GameObject Prefab;
    public int initialSize;
}

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance { get; private set; }  
    
    [SerializeField] private List<PoolPrefabsEntry> poolPrefabs;
    private Dictionary<PoolType, Queue<GameObject>> poolDictionary;

    void Awake()
    {
        Instance = this;
        poolDictionary = new();

        foreach (var entry in poolPrefabs)
        {
            Queue<GameObject> queue = new();
            for (int i = 0; i < entry.initialSize; i++)
            {
                GameObject obj = Instantiate(entry.Prefab);
                obj.SetActive(false);
                obj.transform.SetParent(GetParentTransform(entry.PoolType));
                queue.Enqueue(obj);
            }
            
            poolDictionary[entry.PoolType] = queue;
        }
    }

    public GameObject GetFromPool(PoolType type, Vector3 position, Quaternion rotation)
    {
        if (poolDictionary.TryGetValue(type, out var queue) && queue.Count > 0)
        {
            var obj = queue.Dequeue();
            obj.transform.SetPositionAndRotation(position, rotation);
            obj.SetActive(true);
            
            obj.transform.SetParent(GetParentTransform(type));
            
            return obj;
        }
        
        var entry = poolPrefabs.Find(e => e.PoolType == type);
        if (entry != null)
        {
            var newObj = Instantiate(entry.Prefab, position, rotation);
            return newObj;
        }
        
        Debug.LogWarning($"[pool] No prefab found for {type}");
        return null;
            
    }

    public void ReturnToPool(PoolType type, GameObject obj)
    {
        obj.SetActive(false);
        if(!poolDictionary.ContainsKey(type))
            poolDictionary[type] = new Queue<GameObject>();
        
        poolDictionary[type].Enqueue(obj);
    }
   
    private Transform GetParentTransform(PoolType type)
    {
        string parentName = type switch
        {
            PoolType.Item => "ObjectPool_Items",
            PoolType.FieldObject => "ObjectPool_FieldObjects",
            PoolType.Enemy => "ObjectPool_Enemy",
            PoolType.PlayerBullet => "ObjectPool_PlayerBullets",
            PoolType.EnemyBullet => "ObjectPool_EnemyBullets",
            PoolType.FireballProj => "FireballProj",
            PoolType.BurstingGazeProj => "BurstingGazeProj",
            PoolType.ParchedLongingProj => "ParchedLongingProj",
            PoolType.RagingEmotionsProj => "RagingEmotionsProj",
            PoolType.EtchedHatredProj => "EtchedHatredProj",
            PoolType.PouringAffectionProj => "PouringAffectionProj",
            _ => "PooledObjects"
        };

        GameObject parent = GameObject.Find(parentName);
        if (parent == null)
        {
            parent = new GameObject(parentName);
            parent.transform.position = Vector3.zero;
        }

        return parent.transform;
    }
}