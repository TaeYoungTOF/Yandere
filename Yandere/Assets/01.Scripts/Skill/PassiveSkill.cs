using UnityEngine;

public class PassiveSkill : BaseSkill
{
    public override void LevelUp()
    {
        Debug.Log("[Passive Skill] Level up");

        if (!SkillManager.Instance.equipedPassiveskills.Contains(this))
            SkillManager.Instance.equipedPassiveskills.Add(this);

        base.LevelUp();
    }
}
