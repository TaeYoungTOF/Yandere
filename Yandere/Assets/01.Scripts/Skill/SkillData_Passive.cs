using UnityEngine;

[CreateAssetMenu(fileName = "NewPassiveSkill", menuName = "Skill/SkillData/PassiveSkill", order = 0)]
public class SkillData_Passive : SkillData
{
    [Header("Passive Skill")]
    public float value;
}