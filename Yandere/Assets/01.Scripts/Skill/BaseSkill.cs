using UnityEngine;

public enum SkillId
{
    // 액티브 스킬
    Fireball = 1,
    BurstingGaze,
    ParchedLonging,
    
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
    private static int maxLevel;

    public SkillId skillId;
    public int level = 0;
    
    public Sprite skillIcon { get; private set; }

    public SkillData[] skillDatas = new SkillData[maxLevel];
    public SkillData currentLevelData;
    public SkillData nextLevelData;
    
    protected Player player;

    public void Init()
    {
        player = StageManager.Instance.Player;
        maxLevel = SkillManager.Instance.MaxLevel;
        
        nextLevelData = skillDatas[0];
        
        skillIcon = skillDatas[0].skillIcon;
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
