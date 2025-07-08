using UnityEngine;

public class ActiveSkill : BaseSkill
{
    protected SkillData_Active ActiveData => currentLevelData as SkillData_Active;

    private float _coolDownTimer;

    public virtual void UpdateCooldown()
    {
        if (_coolDownTimer > 0f)
        {
            _coolDownTimer -= Time.deltaTime;
        }
    }

    public virtual void TryActivate()
    {
        if (_coolDownTimer <= 0f)
        {
            Activate();
            _coolDownTimer = ActiveData.coolDown;
        }
    }

    protected virtual void Activate()
    {
        Debug.Log($"[ActiveSkill] {name} Activated.");
        UpdateAcitveData();
    }

    public override void LevelUp()
    {
        Debug.Log("[Active Skill] Level up");

        if (!SkillManager.Instance.equipedActiveSkills.Contains(this))
            SkillManager.Instance.equipedActiveSkills.Add(this);

        base.LevelUp();
    }

    private void UpdateAcitveData()
    {
        ActiveData.projectileCount += SkillManager.Instance.ProjectileCount;
        ActiveData.skillDamage *= 1 + (SkillManager.Instance.SkillDamage / 100);
        ActiveData.skillDuration *= 1 + (SkillManager.Instance.SkillDamage / 100);
        ActiveData.coolDown *= 1 - (SkillManager.Instance.CoolDown / 100);
        ActiveData.skillRange *= 1 + (SkillManager.Instance.SkillDamage / 100);
        ActiveData.crit += StageManager.Instance.Player.stat.criticalChance + SkillManager.Instance.Crit / 100;
    }
}
