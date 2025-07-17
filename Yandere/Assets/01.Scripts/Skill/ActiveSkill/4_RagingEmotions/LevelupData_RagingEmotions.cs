using UnityEngine;

[CreateAssetMenu(fileName = "RagingEmotions_", menuName = "Skill/SkillData/RagingEmotions", order = 4)]
public class LevelupData_RagingEmotions : SkillData_Active
{
    [Header("Upgradable")]
    public float skillDuration;
    public float knockbackDistance;
}
