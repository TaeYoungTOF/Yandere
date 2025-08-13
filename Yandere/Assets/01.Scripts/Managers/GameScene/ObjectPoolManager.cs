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
    Stage1BossSkillPattern1Proj01 = 301,
    Stage1BossSkillPattern1Proj02,
    Stage1BossSkillPattern1Proj03,
    Stage1BossSkillPattern2Proj01,
    Stage1BossSkillPattern3Proj00,
    Stage1BossSkillPattern3Proj01,
    Stage1BossSkillPattern3Proj02,
    Stage2BossSkillPattern1Proj01,
    Stage2BossSkillPattern1Proj02,
    Stage2BossSkillPattern1Proj03,
    Stage2BossSkillPattern2Proj01,
    Stage2BossSkillPattern2Proj02,
    Stage2BossSkillPattern3Proj01,
    Stage3BossSkillPattern1Proj01,
    Stage3BossSkillPattern1Proj02,
    Stage3BossSkillPattern2Proj01,
    Stage3BossSkillPattern3Proj01,
    Stage3BossSkillPattern3Proj02,
    Stage4BossSkillPattern1Proj01,
    Stage4BossSkillPattern1Proj02,
    Stage4BossSkillPattern1Proj03,
    Stage4BossSkillPattern2Proj01,
    Stage4BossSkillPattern3Proj01,
    Stage4BossSkillPattern3Proj02,
    Stage4BossSkillPattern3Proj03,
    Stage4BossSkillPattern3Proj04,
    EnemyDashSkill,
    EnemyChargeSkill,
    EnemyDashWarningEffect,
    EnemyEAttackGrenadeProj01,
    EnemyEAttackGrenadeProj02,
    EnemyEAttackGrenadeProj03
    
    
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
        public int currentIndex;
        public Transform parent;
    }

    [SerializeField] private SerializedDictionary<PoolType, PoolData> _pools;
    [SerializeField] private SerializedDictionary<EnemyID, PoolData> _enemyPools;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (_pools == null) _pools = new SerializedDictionary<PoolType, PoolData>();
        if (_enemyPools == null) _enemyPools = new SerializedDictionary<EnemyID, PoolData>();

        InitializePools();
        InitializeEnemyPool(StageManager.Instance.currentStageData.enemyList);
    }

    private void InitializePools()
    {
        Transform skillParent = new GameObject("Skill").transform;
        Transform upgradeSkillParent = new GameObject("UpgradeSkill").transform;
        Transform enemySkillParent = new GameObject("EnemySkill").transform;

        skillParent.SetParent(transform);
        upgradeSkillParent.SetParent(transform);
        enemySkillParent.SetParent(transform);
        
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
            Debug.LogWarning($"[Pool] No pool found for {type}");
            return null;
        }

        /*if (pool.currentIndex >= pool.objects.Count)
        {
            pool.currentIndex = 0;
        }

        GameObject obj = pool.objects[pool.currentIndex];
        pool.currentIndex++;
        
        // ⛑ 안전 경고: 아직 비활성화되지 않았는데 재사용하려는 경우
        if (obj.activeSelf)
        {
            Debug.LogWarning($"[Pool] Active object ({type}) at index {pool.currentIndex}. Consider increasing pool size.");
        }*/

        var obj = RentNextInactive(pool, type.ToString());
        if (!obj) return null;
        
        obj.transform.SetPositionAndRotation(position, rotation);
        obj.SetActive(true);

        return obj;
    }

    public void ReturnToPool(PoolType type, GameObject obj)
    {
        if (!_pools.TryGetValue(type, out var pool))
        {
            Debug.LogWarning($"[pool] No pool found for {type}");
            return;
        }

        obj.SetActive(false);
        obj.transform.SetParent(pool.parent);
    }

    private void InitializeEnemyPool(List<SpawnEnemyEntry> enemies)
    {
        Transform parent = new GameObject("Enemy").transform;
        parent.SetParent(transform);
        parent.position = Vector3.zero;

        foreach (var entry in enemies)
        {
            var poolData = new PoolData
            {
                parent = parent,
                currentIndex = 0
            };

            for (int i = 0; i < entry.initialSize; i++)
            {
                GameObject obj = Instantiate(entry.enemyPrefab, parent);
                obj.SetActive(false);
                poolData.objects.Add(obj);
            }

            _enemyPools[entry.id] = poolData;
        }
    }

    public GameObject GetEnemyFromPool(EnemyID type, Vector3 position, Quaternion rotation)
    {
        if (!_enemyPools.TryGetValue(type, out var pool))
        {
            Debug.LogWarning($"[pool] No pool found for {type}");
            return null;
        }

        var obj = RentNextInactive(pool, type.ToString());
        if (!obj) return null;
        
        
        obj.transform.SetPositionAndRotation(position, rotation);
        obj.SetActive(true);

        return obj;
    }

    public void ReturnEnemyToPool(EnemyID type, GameObject obj)
    {
        if (!_enemyPools.TryGetValue(type, out var pool))
        {
            Debug.LogWarning($"[pool] No pool found for {type}");
            return;
        }

        obj.SetActive(false);
        obj.transform.SetParent(pool.parent);
    }

    // 공용 헬퍼: 한 바퀴 안에서 비활성 객체 탐색
    private static GameObject RentNextInactive(PoolData pool, string typeLabel)
    {
        if (pool == null || pool.objects == null || pool.objects.Count == 0)
        {
            Debug.LogWarning($"[Pool] '{typeLabel}' pool is empty.");
            return null;
        }

        int checkedCount = 0;
        while (checkedCount < pool.objects.Count)
        {
            if (pool.currentIndex >= pool.objects.Count)
                pool.currentIndex = 0;

            var candidate = pool.objects[pool.currentIndex];
            pool.currentIndex = (pool.currentIndex + 1) % pool.objects.Count;

            if (!candidate.activeSelf)
                return candidate;

            checkedCount++;
        }

        Debug.LogWarning($"[Pool] No inactive object available for {typeLabel}. Consider increasing pool size.");
        return null; // 필요 시 여기서 '가장 오래된 객체' 강제 재사용으로 바꿀 수 있음
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
