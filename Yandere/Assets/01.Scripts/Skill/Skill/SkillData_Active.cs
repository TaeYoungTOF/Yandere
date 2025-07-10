using UnityEngine;

public class SkillData_Active : SkillData
{
    [Header("Active Skill")]
    [Tooltip("투사체 개수")] public int projectileCount;

    [Tooltip("스킬 데미지")] public float skillDamage;

    [Tooltip("스킬 지속시간")] public float skillDuration;

    [Tooltip("스킬 쿨타임")] public float coolDown;

    [Tooltip("스킬 범위")] public float skillRange;

    [Tooltip("크리티컬 확률")] public float crit;
}
