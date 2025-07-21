using UnityEngine;

[CreateAssetMenu(fileName = "NewUpgradeSkill", menuName = "Skill/SkillData/UpgradeSkill", order = -1)]
public class SkillData_Upgrade : SkillData
{
    [Header("Upgrade Skill")]
    [Tooltip("투사체 개수")] public int projectileCount;

    [Tooltip("스킬 데미지")] public float skillDamage;

    [Tooltip("스킬 쿨타임")] public float coolTime;
}
