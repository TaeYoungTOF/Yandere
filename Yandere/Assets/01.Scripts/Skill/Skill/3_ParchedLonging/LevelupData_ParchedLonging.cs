using UnityEngine;

[CreateAssetMenu(fileName = "LevelupData_ParchedLonging", menuName = "Skill/SkillData/ParchedLonging", order = 3)]
public class LevelupData_ParchedLonging : SkillData_Active
{
    [Header("Upgradable")]
    public float duration;
    public float explosionRange;
}
