using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class UpgradeSkillWrapper
{
    public int projectileCount;
    public float skillDamage;
    public float coolTime;
}

public abstract class UpgradeSkill : BaseSkill
{
    [SerializeField] private GameObject basicActSkillGo;
    [SerializeField] private GameObject basicPasSkillGo;
    
    private ActiveSkill basicActSkill;
    private PassiveSkill basicPasSkill;

    private void Start()
    {
        basicActSkill = basicActSkillGo.GetComponent<ActiveSkill>();
        basicPasSkill = basicPasSkillGo.GetComponent<PassiveSkill>();

        Init();

        SkillManager.Instance.availableUpgradeSkills.Add(this);
    }

    public bool IsUpgradable()
    {
        if (basicActSkill == null || basicPasSkill == null)
            return false;

        bool actCondition = basicActSkill.level == SkillManager.Instance.MaxLevel;
        bool pasCondition = SkillManager.Instance.equipedPassiveSkills.Contains(basicPasSkill);

        return actCondition && pasCondition;
    }
    
    protected SkillData_Upgrade UpgradeData => currentLevelData as SkillData_Upgrade;
    [SerializeField] protected float coolDownTimer;

    public abstract void UpdateCooldown();
    public abstract void TryActivate();
    protected abstract void Activate();

    public override void LevelUp()
    {
        level ++;
        currentLevelData = nextLevelData;
        
        SkillManager.Instance.equipedUpgradeSkills.Add(this);
        SkillManager.Instance.availableUpgradeSkills.Remove(this);
        SkillManager.Instance.equipedActiveSkills.Remove(basicActSkill);
    }
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
        data.projectileCount = UpgradeData.projectileCount + player.stat.ProjectileCount;;
        data.skillDamage = CalculateDamage(UpgradeData.skillDamage);
        data.coolTime = UpgradeData.coolTime * (1 - player.stat.CoolDown / 100f);
    }

    protected float CalculateDamage(float damage)
    {
        bool isCrit = Random.Range(0, 100) < player.stat.FinalCrit;    
        damage *= isCrit ? (1 + player.stat.FinalAtk / 100f) * (player.stat.FinalCritDmg / 100f) : 1 + player.stat.FinalAtk / 100f;

        return damage * StageManager.Instance.GlobalPlayerDamageMultiplier;
    }
    
}
