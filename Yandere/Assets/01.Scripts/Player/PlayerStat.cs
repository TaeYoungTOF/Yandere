using UnityEngine;

[System.Serializable]
public class PlayerStat
{
    // 기본 능력치
    public float moveSpeed;            // 이동속도
    public float maxHealth;            // 최대 체력
    public float currentHealth;        // 현재 체력
    public float attackPower;          // 공격력
    public float defense;              // 방어력

    // 치명타 관련
    public float criticalChance;       // 치명타 확률 (0.1 = 10%)
    public float criticalDamage;       // 치명타 배수 (1.5 = 150%)

    // 보조 능력치
    public float healthRegen;          // 초당 체력 회복
    public float pickupRange;          // 오브젝트 획득 반경
    public float cooldownReduction;    // 스킬 쿨다운 감소 비율
    public float skillRange;           // 스킬 범위 배율
    public float lifeSteal;            // 흡혈 (공격 시 체력 회복 비율)
    public float expGain;              // 경험치 획득량 배율
    public float obsessionGauge;       // 집착 게이지

    // 경험치 관련
    public int level;
    public float currentExp;
    public float requiredExp;

    // 특수 조건
    public float minHitInterval;       // 최소 피격 시간 간격
    public float skillDuration;        // 스킬 지속 시간

    public void ResetStat()
    {
        // 기본 능력치
        moveSpeed = 5f;
        maxHealth = 100f;
        currentHealth = maxHealth;
        attackPower = 10f;
        defense = 5f;

        // 치명타
        criticalChance = 0.1f;
        criticalDamage = 1.5f;

        // 보조 능력치
        healthRegen = 0f;
        pickupRange = 2f;
        cooldownReduction = 0f;
        skillRange = 1f;
        lifeSteal = 0f;
        expGain = 1f;
        obsessionGauge = 0f;

        // 경험치 관련
        level = 0;
        currentExp = 0f;
        requiredExp = 100f;

        // 특수 조건
        minHitInterval = 0.5f;
        skillDuration = 5f;
    }
}
