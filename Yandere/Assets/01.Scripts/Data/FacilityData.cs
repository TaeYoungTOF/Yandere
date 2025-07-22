using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FacilityData", menuName = "Facility/FacilityData")]
public class FacilityData : ScriptableObject
{
    public string facilityName;                                     // 시설 이름
    public int requiredAccountLevel;                                // 필요 계정 레벨
    public string statTarget;                                       // 강화 대상
    public int maxLevel;                                            // 최대 레벨
    public float valuePerLevel;                                     // 레벨당 증가량
    public int baseCost;                                            // 기본 비용
    public float costMultiplier;                                    // 레벨업 시 비용 증가 비율
    
    [TextArea]
    public string description;
}
