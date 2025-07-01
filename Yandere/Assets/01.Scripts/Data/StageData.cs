using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Achievement
{
    [Range(0, 2)] public int rank;
    public bool isCleared;
    [Multiline(3)] public string description;
}

[CreateAssetMenu(fileName = "StageData", menuName = "Stage/StageData", order = 0)]
public class StageData : ScriptableObject {
    public int stageIndex;

    [Tooltip("초 단위로 작성")]
    public float clearTime;

    public List<Achievement> achievements;

    public List<WaveData> spwanDatas;
    
    // clearRewards 리스트


    
}