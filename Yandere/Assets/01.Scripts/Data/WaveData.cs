using System.Collections.Generic;
using UnityEngine;

public enum EventType
{
    StartWave,
    AddStrongerEnemy,
    AddEliteEnemy,
    AddRangeEnemy,
    AddBossEnemy,
}

[System.Serializable]
public class EnemySpawnWeigth
{
    public EnemyID id;
    [Range(1, 100)] public int spawnWeight;
    public DropTable dropTable;
}

[CreateAssetMenu(fileName = "WaveData", menuName = "Stage/WaveData", order = 1)]
public class WaveData : ScriptableObject {

    [Tooltip("스폰 이벤트 유형")]
    public EventType eventType;

    [Tooltip("스폰 이벤트 종료 시간")]
    public float endTime;

    [Tooltip("스폰 간격")]
    public float spawnInterval;

    [Tooltip("스폰 할 때마다 소환되는 enemy양")]
    public int spawnAmount;

    [Tooltip("생성되는 모든 enemy")]
    public List<EnemySpawnWeigth> enemyList;
}