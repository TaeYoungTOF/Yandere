using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RandomAchievementManager : MonoBehaviour
{
    public static RandomAchievementManager Instance;

    // 게임에 존재하는 모든 업적 리스트 (Unity 인스펙터에서 설정)
    public List<Achievement> allAchievements;

    // 현재 플레이어에게 할당된 3개의 랜덤 업적
    [HideInInspector]
    public List<Achievement> currentRandomAchievements;

    // 플레이어의 현재 진행 상황 (업적 달성 확인용)
    // 실제 게임에서는 PlayerStats 같은 별도 클래스에서 관리하는 것이 좋습니다.
    private Dictionary<ConditionType, float> playerProgress;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            playerProgress = new Dictionary<ConditionType, float>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // 게임 시작 시, 3개의 랜덤 업적을 할당합니다.
        AssignNewRandomAchievements();
    }

    /// <summary>
    /// 플레이어에게 새로운 랜덤 업적 3개를 할당합니다.
    /// </summary>
    public void AssignNewRandomAchievements()
    {
        currentRandomAchievements.Clear();

        // 'Random' 카테고리의 업적만 필터링합니다.
        var available = allAchievements.Where(ach => ach.category == AchievementCategory.Random).ToList();

        int count = Mathf.Min(3, available.Count);

        for (int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(0, available.Count);
            currentRandomAchievements.Add(available[randomIndex]);
            available.RemoveAt(randomIndex); // 중복 할당 방지
        }

        Debug.Log("=== 새로운 랜덤 업적이 할당되었습니다 ===");
        foreach (var ach in currentRandomAchievements)
        {
            Debug.Log($"- {ach.title}");
        }
        // UIManager.Instance.UpdateAchievementUI(currentRandomAchievements);
    }

    /// <summary>
    /// 플레이어의 행동에 따라 진행 상황을 업데이트하고 업적 달성을 확인합니다.
    /// </summary>
    /// <param name="type">어떤 종류의 행동인지</param>
    /// <param name="valueToAdd">증가시킬 값</param>
    public void UpdateProgress(ConditionType type, float valueToAdd)
    {
        // 진행 상황 업데이트
        if (!playerProgress.ContainsKey(type))
        {
            playerProgress[type] = 0;
        }
        playerProgress[type] += valueToAdd;

        // 현재 할당된 업적 중, 달성 가능한 것이 있는지 확인
        // ToList()로 복사본을 만들어 순회해야 안전하게 원본 리스트에서 제거할 수 있습니다.
        foreach (var ach in currentRandomAchievements.ToList())
        {
            if (ach.conditionType == type)
            {
                if (playerProgress[type] >= ach.conditionValue)
                {
                    UnlockAchievement(ach);
                }
            }
        }
    }

    /// <summary>
    /// 업적을 달성 처리하고 보상을 지급합니다.
    /// </summary>
    private void UnlockAchievement(Achievement achievement)
    {
        Debug.Log($"<color=yellow>업적 달성! [{achievement.title}]</color>");

        // 실제 보상 지급 로직
        // RewardManager.Instance.GrantReward(achievement.reward);

        // UI 업데이트
        // UIManager.Instance.ShowAchievementUnlockedPopup(achievement);

        // 달성된 업적을 현재 목록에서 제거
        currentRandomAchievements.Remove(achievement);

        // 만약 할당된 3개의 업적을 모두 완료했다면 새로운 업적을 할당할 수 있습니다.
        if (currentRandomAchievements.Count == 0)
        {
            Debug.Log("모든 할당된 업적을 완료했습니다! 새로운 업적을 할당합니다.");
            AssignNewRandomAchievements();
        }
    }
}