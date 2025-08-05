using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

public enum PoolType
{
    EnemyBullet,
    PlayerBullet,
    Enemy,
    Item,
    FieldObject,

    // Skills
    FireballProj = 101,
    BurstingGazeProj,
    ParchedLongingProj,
    RagingEmotionsProj,
    EtchedHatredProj,
    PouringAffectionProj,
    
    // UpgradeSkills
    BurningJealousy2Proj = 201,
    BurningJealousy2Proj2,
    BurstingGaze2Proj,
    BurstingGaze2Proj2,
    ParchedLonging2Proj,
    ParchedLonging2Proj2,
    RagingEmotions2Proj,
    EtchedHatred2Proj,
    PouringAffection2Proj,
    PouringAffection2Proj2,
    
    // EnemySkills
    Stage1BossSkillProj01 = 301,
    Stage1BossSkillProj02,
    Stage2BossSkillProj01,
    Stage2BossSkillProj02,
    Stage3BossSkillProj01,
    Stage3BossSkillProj02,
    Stage4BossSkillProj01,
    Stage4BossSkillProj02,
    
}

[System.Serializable]
public class PoolPrefabsEntry
{
    public PoolType poolType;
    public GameObject prefab;
    public int initialSize;
}

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance { get; private set; }

    [SerializeField] private List<PoolPrefabsEntry> poolPrefabs;

    [System.Serializable]
    private class PoolData
    {
        public List<GameObject> objects = new();
        public int currentIndex = 0;
        public Transform parent;
    }

    [SerializeField] private SerializedDictionary<PoolType, PoolData> _pools;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        _pools = new SerializedDictionary<PoolType, PoolData>();
        InitializePools();
    }

    private void InitializePools()
    {
        Transform skillParent = new GameObject("Skill").transform;
        Transform upgradeSkillParent = new GameObject("UpgradeSkill").transform;
        Transform enemySkillParent = new GameObject("EnemySkill").transform;

        skillParent.SetParent(transform);
        upgradeSkillParent.SetParent(transform);
        
        foreach (var entry in poolPrefabs)
        {
            Transform categoryParent = transform;

            if ((int)entry.poolType >= 101 && (int)entry.poolType < 201)
            {
                categoryParent = skillParent;
            }
            else if ((int)entry.poolType >= 201 && (int)entry.poolType < 301)
            {
                categoryParent = upgradeSkillParent;
            }
            else if ((int)entry.poolType >= 301 && (int)entry.poolType < 401)
            {
                categoryParent = enemySkillParent;
            }
            
            var parent = new GameObject($"ObjectPool_{entry.poolType}").transform;
            parent.SetParent(categoryParent);
            parent.position = Vector3.zero;

            var poolData = new PoolData
            {
                parent = parent,
                currentIndex = 0
            };

            for (int i = 0; i < entry.initialSize; i++)
            {
                GameObject obj = Instantiate(entry.prefab, parent);
                obj.SetActive(false);
                poolData.objects.Add(obj);
            }

            _pools[entry.poolType] = poolData;
        }
    }

    public GameObject GetFromPool(PoolType type, Vector3 position, Quaternion rotation)
    {
        if (!_pools.TryGetValue(type, out var pool))
        {
            Debug.LogWarning($"[pool] No pool found for {type}");
            return null;
        }

        if (pool.currentIndex >= pool.objects.Count)
        {
            Debug.LogWarning($"[pool] Pool overflow: {type}");
            pool.currentIndex = 0;
        }

        GameObject obj = pool.objects[pool.currentIndex];
        obj.transform.SetPositionAndRotation(position, rotation);
        obj.SetActive(true);
        pool.currentIndex++;

        return obj;
    }

    public void ReturnToPool(PoolType type, GameObject obj)
    {
        if (!_pools.TryGetValue(type, out var pool))
        {
            Debug.LogWarning($"[pool] No pool found for {type}");
            return;
        }

        pool.currentIndex = Mathf.Max(pool.currentIndex - 1, 0);
        obj.SetActive(false);
        obj.transform.SetParent(pool.parent);
    }

    /*
    public int GetActiveCount(PoolType type)
    {
        return _pools.TryGetValue(type, out var pool) ? pool.currentIndex : -1;
    }

    public int GetPoolSize(PoolType type)
    {
        return _pools.TryGetValue(type, out var pool) ? pool.objects.Count : -1;
    }*/
}
