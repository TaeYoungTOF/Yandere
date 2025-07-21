using UnityEngine;

/// <summary>
/// 업적의 종류를 정의합니다.
/// Random: 게임 중 무작위로 할당될 수 있는 업적
/// General: 특정 스테이지나 조건에 고정적으로 연결된 업적
/// </summary>
public enum AchievementCategory
{
    Random,
    General
}

/// <summary>
/// 업적 달성 조건의 종류를 구체적으로 정의합니다.
/// 사진에 나온 모든 조건을 포함하도록 확장합니다.
/// </summary>
public enum ConditionType
{
    // 생존
    SurviveDuration,          // 특정 시간 동안 생존
    SurviveWithoutTakingDamage, // 피해 없이 특정 시간 생존

    // 전투
    TotalMonsterKills,        // 누적 몬스터 처치
    EliteMonsterKills,        // 누적 엘리트 몬스터 처치
    ClearWithHealthPercentage, // 특정 체력 비율 이상으로 클리어
    ClearWithFewRecoveryItems, // 회복 아이템 적게 사용하고 클리어

    // 성장
    AcquireAllActiveSkills,   // 모든 액티브 스킬 슬롯 채우기
    AcquireAllPassiveSkills,  // 모든 패시브 스킬 슬롯 채우기
    EvolveSkill,              // 스킬 진화

    // 기타
    StayStill,                // 가만히 있기
    GainExperience           // 경험치 오브젝트 획득
}


/// <summary>
/// 개별 업적의 정보를 담는 데이터 클래스입니다.
/// </summary>
[System.Serializable]
public class AchievementData
{
    public int id;
    public string title;
    [TextArea]
    public string description;
    public AchievementCategory category;

    // 업적 달성 조건
    public ConditionType conditionType;
    public float conditionValue; // 조건 목표치 (예: 120초, 1000마리)

    // 보상 정보
    public RewardData reward;
}

/// <summary>
/// 보상 데이터 구조입니다.
/// </summary>
[System.Serializable]
public class RewardData
{
    public int gold;
    public int gem;
}