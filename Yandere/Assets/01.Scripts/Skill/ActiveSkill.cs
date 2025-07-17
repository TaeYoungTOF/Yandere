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
    [SerializeField] protected float coolDownTimer;

    public abstract void UpdateCooldown();
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

public abstract class ActiveSkill<T> : ActiveSkill where T : AcviteDataWapper, new()
{
    [SerializeField] protected T data = new T();

    public override void UpdateCooldown()
    {
        if (coolDownTimer > 0f)
        {
            coolDownTimer -= Time.deltaTime;
        }
    }
    
    public override void TryActivate()
    {
        if (coolDownTimer <= 0f)
        {
            UpdateActiveData();
            Activate();
            coolDownTimer = data.coolTime;
        }
    }
    
    public virtual void UpdateActiveData()
    {
        data.projectileCount = ActiveData.projectileCount + player.stat.ProjectileCount;;
        data.skillDamage = CalculateDamage(ActiveData.skillDamage);
        data.coolTime = ActiveData.coolTime * (1 - player.stat.CoolDown / 100f);
    }

    protected float CalculateDamage(float damage)
    {
        bool isCrit = Random.Range(0, 100) < player.stat.FinalCrit;    
        damage *= isCrit ? (1 + player.stat.FinalAtk / 100f) * (player.stat.FinalCritDmg / 100f) : 1 + player.stat.FinalAtk / 100f;

        return damage * StageManager.Instance.GlobalPlayerDamageMultiplier;
    }
}
