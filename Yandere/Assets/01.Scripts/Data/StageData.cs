using System.Collections.Generic;
using UnityEngine;

public enum StageAchievement
{
    none,
    star1,
    star2,
    star3,
}

[CreateAssetMenu(fileName = "StageData", menuName = "StageData", order = 0)]
public class StageData : ScriptableObject {
    public int stageIndex;
    public int maxWave;
    public List<GameObject> monsterPrefabs;
    public StageAchievement stageAchievement = StageAchievement.none;

    // clearRewards 리스트


    
}