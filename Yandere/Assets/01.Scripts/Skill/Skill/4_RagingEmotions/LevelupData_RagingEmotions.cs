using UnityEngine;

[CreateAssetMenu(fileName = "LevelupData_RagingEmotions", menuName = "Skill/SkillData/RagingEmotions", order = 4)]
public class LevelupData_RagingEmotions : SkillData_Active
{
    [Header("Upgradable")]
    public float skillDuration;
    public float knockbackDistance;
}
