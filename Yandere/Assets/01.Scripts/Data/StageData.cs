using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageData", menuName = "Stage/StageData", order = 0)]
public class StageData : ScriptableObject {
    public int stageIndex;
    public string stageName;
    public string stageDesc;
    public Sprite stageImage;

    [Tooltip("초 단위로 작성")]
    public float clearTime;

    public List<WaveData> waveDatas;
    
}