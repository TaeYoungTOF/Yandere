using UnityEngine;

public class PassiveSkill : BaseSkill
{
    protected SkillData_Passive _passiveData => currentLevelData as SkillData_Passive;
    public SkillData_Passive PassiveData => _passiveData;

    public override void LevelUp()
    {
        if (!SkillManager.Instance.equipedPassiveSkills.Contains(this))
            SkillManager.Instance.equipedPassiveSkills.Add(this);
        
        base.LevelUp();
        UpdatePassiveStat(skillId, _passiveData.value);
    }

    private void UpdatePassiveStat(SkillId id, float value)
    {
        switch ((int)id)
        {
            case 101:
                player.stat.GetBonusProjectileCount((int)value);
                break;
            case 102:
                player.stat.GetBonusAtkPer(value);
                break;
            case 103:
                player.stat.GetBonusSkillDuration(value);
                break;
            case 104:
                player.stat.GetBonusCoolDown(value);
                break;
            case 105:
                player.stat.GetBonusSkillRange(value);
                break;
            case 106:
                player.stat.GetBonusCrit(value);
                break;
            default:
                Debug.Log("[SkillManager] Unknown Passive Type");
                break;
        }

        player.stat.UpdateStats();
    }
}
