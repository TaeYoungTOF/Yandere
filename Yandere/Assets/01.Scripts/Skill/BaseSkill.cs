using UnityEngine;

public enum SkillId
{
    // 액티브 스킬
    Fireball = 1,
    
    // 패시브 스킬
    ProjectileCount = 101,
    SkillDamage,
    SkillDuration,
    CoolDown,
    SkillRange,
    Crit,
}


public class BaseSkill : MonoBehaviour
{
    public SkillId skillId;
    public int level = 0;

    public SkillData[] skillDatas = new SkillData[SkillManager.maxLevel];
    public SkillData currentLevelData;
    public SkillData nextLevelData;

    public void Init()
    {
        nextLevelData = skillDatas[0];
    }

    public virtual void LevelUp()
    {
        level ++;
        currentLevelData = nextLevelData;

        if (level < SkillManager.maxLevel)
        {
            nextLevelData = skillDatas[level];
        }
        else
        {
            nextLevelData = null;
            SkillManager.Instance.availableSkills.Remove(this);
        }
    }
}
