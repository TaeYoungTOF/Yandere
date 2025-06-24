using System.Collections.Generic;
using UnityEngine;

public enum PoolType
{
    ProjectTile = 0,
    DefaultEnemy,
    BossEnemy,    
}

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance { get; private set; }

    private Dictionary<int, Queue<GameObject>> _pools = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (Instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    public void CreatePool(int key, GameObject prefab, int count)
    {
        if (_pools.ContainsKey(key)) return;

        Queue<GameObject> queue = new Queue<GameObject>();
        for (int i = 0; i < count; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            queue.Enqueue(obj);
        }

        _pools[key] = queue;
    }

    public GameObject GetFromPool(int key)
    {
        if (_pools.TryGetValue(key, out var queue))
        {
            if (queue.Count > 0)
            {
                GameObject obj = queue.Dequeue();
                return obj;
            }
        }

        Debug.LogWarning($"[ObjectPoolManager] No object available for key: {key}");
        return null;
    }

    public void ReturnToPool(int key, GameObject obj)
    {
        obj.SetActive(false);
        if (!_pools.ContainsKey(key))
            _pools[key] = new Queue<GameObject>();

        _pools[key].Enqueue(obj);
    }
}