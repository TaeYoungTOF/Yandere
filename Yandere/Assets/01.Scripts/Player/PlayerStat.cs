using UnityEngine;

[System.Serializable]
public class PlayerStat
{    
    /**
    외부 참조 스탯
    CurrentHp

    GetBonus****() 메서드 호출 후 반드시 UpdateStats() 호출    
    */

    // 스탯 계산식
    public void UpdateStats()
    {
        _finalAtk = baseAtk * (1 + _bonusAtkPer / 100) * (1 + _frenzePer / 100);
        _finalCrit = baseCrit + _bonusCrit;
        _finalCritDmg = baseCritDmg + _bonusCritDmg;

        _finalHp = baseHp + _bonusHp;
        _finalDef = baseDef + _bonusDef;
        _finalHpRegen = baseHpRegen + _bonusHpRegen;

        _finalMoveSpeed = baseMoveSpeed + _bonusMoveSpeed;
        _finalPickupRadius = basePickupRadius * (1 + _bonusPickupRadius);
        _finalSkillRange = baseSkillRange * (1 + _bonusSkillRange);
        _finalSkillDuration = baseSkillDuration * (1 + _bonusskillDuration); 
    }

    public void ResetStats()
    {
        _bonusAtkPer = 0;
        _bonusCrit = 0;
        _bonusCritDmg = 0;

        _bonusHp = 0;
        _bonusDef = 0;
        _bonusHpRegen = 0;

        level = 0;
        currentExp = 0f;
        requiredExp = 5f;

        _bonusMoveSpeed = 0;
        _bonusPickupRadius = 0;
        _bonusSkillRange = 0;

        UpdateStats();
        _currentHp = FinalHp;
    }

    #region Attack Stats================================
    private const float baseAtk = 10;
    private float _bonusAtkPer; // ex) 50 => 50%
    private float _frenzePer; // ex) 50 => 50%
    [SerializeField] private float _finalAtk;
    public float FinalAtk => _finalAtk;
    public void GetBonusAtkPer(float amount) => _bonusAtkPer += amount;
    //===================================================
    private const float baseCrit = 5;
    private float _bonusCrit;
    [SerializeField] private float _finalCrit;
    public float FinalCrit => _finalCrit;
    public void GetBonusCrit(float amount) => _bonusCrit += amount;
    //===================================================
    private const float baseCritDmg = 120;
    private float _bonusCritDmg;
    [SerializeField] private float _finalCritDmg;
    public float FinalCritDmg => _finalCritDmg;
    public void GetBonusCritDmg(float amount) => _bonusCritDmg += amount;
    #endregion========================================

    #region Defense Stats=============================
    private const float baseHp = 100;
    private float _bonusHp;
    [SerializeField] private float _finalHp;
    public float FinalHp => _finalHp;
    [SerializeField] private float _currentHp;
    public float CurrentHp => _currentHp;
    public void GetBonusHp(float amount)
    {
        _bonusHp += amount;
        _currentHp += amount;
    }
    public void ChangeCurrentHp(float amount)
    {
        _currentHp = Mathf.Clamp(_currentHp + amount, 0, _finalHp);
    }
    //=====================================================
    private const float baseDef = 5;
    private float _bonusDef;
    [SerializeField] private float _finalDef;
    public float FinalDef => _finalDef;
    public void GetBounusDef(float amount) => _bonusDef += amount;
    //=====================================================
    private const float baseHpRegen = 0.5f;
    private float _bonusHpRegen;
    [SerializeField] private float _finalHpRegen;
    public float FinalHpRegen => _finalHpRegen;
    public void GetBonusHpRegen(float amount) => _bonusHpRegen += amount;
    #endregion

    #region Exp stats==========================================
    public int level;
    public float currentExp;
    public float requiredExp;
    public float expGain;

    #endregion=================================================

    #region Utility Stats with base============================
    private const float baseMoveSpeed = 4;
    private float _bonusMoveSpeed;
    [SerializeField] private float _finalMoveSpeed;
    public float FinalMoveSpeed => _finalMoveSpeed;
    public void GetBonusMoveSpeed(float amount) => _bonusMoveSpeed += amount;
    //=========================================================
    private const float basePickupRadius = 3;
    private float _bonusPickupRadius;
    [SerializeField] private float _finalPickupRadius;
    public float FinalPickupRadius => _finalPickupRadius;
    public void GetBonusPickupRadius(float amount) => _bonusPickupRadius += amount / 100f;
    //=========================================================
    private const float baseSkillRange = 1;
    private float _bonusSkillRange;
    [SerializeField] private float _finalSkillRange;
    public float FinalSkillRange => _finalSkillRange;
    public void GetBonusSkillRange(float amount) => _bonusSkillRange += amount / 100f;
    //========================================================
    private const float baseSkillDuration = 1;
    private float _bonusskillDuration;
    [SerializeField] float _finalSkillDuration;
    public float FinalSkillDuration => _finalSkillDuration;
    public void GetBonusSkillDuration(float amount) => _bonusskillDuration += amount / 100f;
    #endregion=================================================

    #region Utility Stats without base=========================
    private const float maxCoolDown = 70;
    [SerializeField] private float _coolDown = 0;
    public float CoolDown => _coolDown;
    public void GetBonusCoolDown(float amount) => _coolDown = Mathf.Clamp(_coolDown + amount, 0, maxCoolDown);
    //==========================================================
    [SerializeField] private int _projectileCount;
    public int ProjectileCount => _projectileCount;
    public void GetBonusProjectileCount(int amount) => _projectileCount += amount;
    //===========================================================

    #endregion
}
