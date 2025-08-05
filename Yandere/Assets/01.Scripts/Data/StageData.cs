using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnEnemyEntry
{
    public EnemyID id;
    public GameObject enemyPrefab;
    public int initialSize;
}

[CreateAssetMenu(fileName = "StageData", menuName = "Stage/StageData", order = 0)]
public class StageData : ScriptableObject {
    public int stageIndex;
    public string stageName;
    public string stageDesc;
    public Sprite stageImage;

    [Header("Spawn Area")]
    public Vector2 spawnAreaMin;
    public Vector2 spawnAreaMax;

    [Tooltip("초 단위로 작성")]
    public float clearTime;

    public List<SpawnEnemyEntry> enemyList;

    public List<WaveData> waveDatas;
}

public enum EnemyID
{
    // Stage 0
    
    // Stage 1
    StudentMan,
    StudentGirl,
    Teacher,
    
    // Stage 2
    
    // Stage 3
    
    // Stage 4
}
