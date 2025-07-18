using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeSkillWrapper
{
    public int projectileCount;
    public float skillDamage;
    public float coolTime;
}

public abstract class UpgradeSkill : BaseSkill
{
    [SerializeField] protected int projectileCount;
    [SerializeField] protected float skillDamage;
    [SerializeField] protected float coolTime;
    
    [SerializeField] protected float coolDownTimer;

    public abstract void UpdateCooldown();
    public abstract void TryActivate();
    protected abstract void Activate();
}

public abstract class UpgradeSkill<T> : UpgradeSkill where T : UpgradeSkillWrapper, new()
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
        data.projectileCount = projectileCount + player.stat.ProjectileCount;;
        data.skillDamage = CalculateDamage(skillDamage);
        data.coolTime = coolTime * (1 - player.stat.CoolDown / 100f);
    }

    protected float CalculateDamage(float damage)
    {
        bool isCrit = Random.Range(0, 100) < player.stat.FinalCrit;    
        damage *= isCrit ? (1 + player.stat.FinalAtk / 100f) * (player.stat.FinalCritDmg / 100f) : 1 + player.stat.FinalAtk / 100f;

        return damage * StageManager.Instance.GlobalPlayerDamageMultiplier;
    }
    
}
