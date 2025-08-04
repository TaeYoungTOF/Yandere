using System.Collections.Generic;
using UnityEngine;

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

    public List<WaveData> waveDatas;
    
}