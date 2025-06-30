using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Achievement
{
    [Range(0, 2)] public int rank;
    public bool isCleared;
    public string description;
}

[CreateAssetMenu(fileName = "StageData", menuName = "StageData", order = 0)]
public class StageData : ScriptableObject {
    public int stageIndex;

    [Tooltip("초 단위로 작성")]
    public float clearTime;

    public List<Achievement> achievements;

    public List<SpawnData> spwanDatas; // 코루틴 등록해서 비동기 호출

    // clearRewards 리스트


    
}