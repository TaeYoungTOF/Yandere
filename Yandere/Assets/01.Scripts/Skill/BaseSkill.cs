using UnityEngine;

public enum SkillId
{
    // 액티브 스킬
    Fireball = 1,
    BurstingGaze,
    ParchedLonging,
    RagingEmotions,
    EtchedHatred,
    PouringAffection,
    
    // 패시브 스킬
    ProjectileCount = 101,
    SkillDamage,
    SkillDuration,
    CoolDown,
    SkillRange,
    Crit,
    
    // 업그레이드 스킬
    BurningJeolousy2 = 201,
    BurstingGaze2,
    ParchedLonging2,
    RagingEmotions2,
    EtchedHatred2,
    PouringAffection2,
}

public abstract class BaseSkill : MonoBehaviour
{
    private static int _maxLevel;

    public SkillId skillId;
    public int level = 0;
    
    public Sprite SkillIcon { get; private set; }

    public SkillData[] skillDatas = new SkillData[_maxLevel];
    public SkillData currentLevelData;
    public SkillData nextLevelData;
    
    protected Player player;

    public void Init()
    {
        player = StageManager.Instance.Player;
        _maxLevel = SkillManager.Instance.MaxLevel;
        
        nextLevelData = skillDatas[0];
        
        SkillIcon = skillDatas[0].skillIcon;
    }

    public virtual void LevelUp()
    {
        level ++;
        currentLevelData = nextLevelData;

        if (level < _maxLevel)
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
