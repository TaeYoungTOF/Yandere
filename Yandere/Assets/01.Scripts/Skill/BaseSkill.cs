using UnityEngine;

public enum SkillId
{
    // 액티브 스킬
    Fireball = 1,
    BurstingGaze,
    
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
    public const int maxLevel = 5;

    public SkillId skillId;
    public int level = 0;

    public SkillData[] skillDatas = new SkillData[maxLevel];
    public SkillData currentLevelData;
    public SkillData nextLevelData;
    
    protected Player player;

    public void Init()
    {
        player = StageManager.Instance.Player;
        
        nextLevelData = skillDatas[0];
    }

    public virtual void LevelUp()
    {
        level ++;
        currentLevelData = nextLevelData;

        if (level < maxLevel)
        {
            nextLevelData = skillDatas[level];
        }
        else
        {
            nextLevelData = null;
            SkillManager.Instance.availableSkills.Remove(this);
        }
    }

    protected float CalculateDamage(float damage)
    {
        bool isCrit = Random.Range(0, 100) < player.stat.FinalCrit;    
        damage *= isCrit ? (1 + player.stat.FinalAtk / 100f) * (player.stat.FinalCritDmg / 100f) : 1 + player.stat.FinalAtk / 100f;

        return damage * StageManager.Instance.GlobalPlayerDamageMultiplier;
    }
}
