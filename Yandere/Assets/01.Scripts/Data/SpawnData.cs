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

[CreateAssetMenu(fileName = "SpawnData", menuName = "SpawnData", order = 0)]
public class SpawnData : ScriptableObject {

    [Tooltip("스폰 이벤트 유형")]
    public EventType eventType;

    [Tooltip("스폰 이벤트 발생 시간")]
    public float time;

    [Tooltip("스폰 간격")]
    public float spawnInterval;

    [Tooltip("스폰 할 때마다 소환되는 enemy양")]
    public int spawnAmount;

    [Tooltip("생성되는 모든 enemy")]
    public List<EnemyData> enemyList;
}