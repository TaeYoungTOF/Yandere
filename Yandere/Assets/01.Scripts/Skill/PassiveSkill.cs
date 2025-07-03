using UnityEngine;

public enum StatType
{
    MoveSpeed,
    MaxHealth,
    AttackPower,
    Defense,
    CriticalChance,
    CriticalDamage,
    HealthRegen,
    PickupRange,
    CooldownReduction,
    SkillRange,
    LifeSteal,
    ExpGain,
    MinHitInterval,
    SkillDuration
}

[CreateAssetMenu(fileName = "NewPassiveSkill", menuName = "Skills/PassiveSkill")]
public class PassiveSkill : BaseSkill
{
    public StatType statType;
    public float value;

    public void ApplyPassive(Player player, int skillLevel)
    {
        value *= skillLevel;

        player.stat.ChangeStat(statType, value);
    }

    public void RemovePassive(Player player, int skillLevel)
    {
        value *= skillLevel;

        player.stat.ChangeStat(statType, -value);
    }
}