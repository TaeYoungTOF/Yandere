using UnityEngine;

[CreateAssetMenu(fileName = "BurstingGaze_", menuName = "Skill/SkillData/BurstingGaze", order = 2)]
public class LevelupData_BurstingGaze : SkillData_Active
{
    [Header("Upgradable")]
    [Range (0, 90)] public float angle;
}
