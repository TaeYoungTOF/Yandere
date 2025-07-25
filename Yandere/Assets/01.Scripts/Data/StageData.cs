using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Achievement
{
    [Range(0, 2)] public int rank;
    public bool isCleared; // 클리어 유무 
    [Multiline(3)] public string description;
    public int currentProgress;
    public int targetProgress;
    public AchievementCategory category;
    public ConditionType conditionType;
    public float conditionValue;
    public string title;
}

[CreateAssetMenu(fileName = "StageData", menuName = "Stage/StageData", order = 0)]
public class StageData : ScriptableObject {
    public int stageIndex;
    public string stageName;
    public string stageDesc;
    public Sprite stageImage;

    [Tooltip("초 단위로 작성")]
    public float clearTime;

    public List<Achievement> achieveDatas;

    public List<WaveData> waveDatas;
    
    // clearRewards 리스트
    
    
}