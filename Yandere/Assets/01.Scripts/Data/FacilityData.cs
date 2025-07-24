using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FacilityStatType
{
    None,
    Attack,
    HpMax,
    HpRegen,
    Defense,
    CritChance,
    CritDamage,
    Speed,
    ItemRange,
    Cooldown,
    SkillReroll
}

[CreateAssetMenu(fileName = "FacilityData", menuName = "Facility/FacilityData")]
public class FacilityData : ScriptableObject
{
    public string facilityName;                                     // 시설 이름
    public int requiredAccountLevel;                                // 필요 계정 레벨
    public FacilityStatType _FacilityStatType;                      // 증가 스탯
    public string statTargetText;                                   // 강화 대상 스탯 이름
    public int maxLevel;                                            // 최대 레벨
    public int basevalue;                                           // 강화 전 초기 수치
    public float valuePerLevel;                                     // 강화 레벨 당 스탯 증가량
    public int baseCost;                                            // 초기 기본 비용
    public float costMultiplier;                                    // 레벨업 시 비용 증가 비율
    
    
    [TextArea]
    public string description;
}
