using UnityEngine;

public class PassiveSkill : BaseSkill
{
    protected SkillData_Passive _passiveData => currentLevelData as SkillData_Passive;
    public SkillData_Passive PassiveData => _passiveData;

    public override void LevelUp()
    {
        Debug.Log("[Passive Skill] Level up");

        if (!SkillManager.Instance.equipedPassiveskills.Contains(this))
            SkillManager.Instance.equipedPassiveskills.Add(this);
        
        base.LevelUp();
        SkillManager.Instance.UpdatePassiveStat();
    }
}
