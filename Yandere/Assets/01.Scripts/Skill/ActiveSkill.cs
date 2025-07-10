using UnityEngine;

public class AcviteDataWapper
{
    public int projectileCount;
    public float skillDamage;
    public float coolTime;
}

public abstract class ActiveSkill : BaseSkill
{
    protected SkillData_Active ActiveData => currentLevelData as SkillData_Active;
    protected float coolDownTimer;

    public abstract void UpdateCooldown();

    public abstract void UpdateActiveData();

    public abstract void TryActivate();

    protected abstract void Activate();

    public override void LevelUp()
    {
        Debug.Log("[Active Skill] Level up");

        if (!SkillManager.Instance.equipedActiveSkills.Contains(this))
            SkillManager.Instance.equipedActiveSkills.Add(this);

        base.LevelUp();
    }
}
