using UnityEditor.SceneManagement;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    [SerializeField] private StageManager stageManager;
    [SerializeField] private UI_Achievement ui_Achievement;

    public enum AchievementRank
    {
        First = 0,
        Second = 1,
        Third = 2,
        Fourth = 3
    }

    private void Awake()
    {
        if (stageManager == null) stageManager = StageManager.Instance;
        if (ui_Achievement == null) ui_Achievement = Instantiate(ui_Achievement);
        
        ValidateComponents();
    }

    private void ValidateComponents()
    {
        if (stageManager == null)
            Debug.LogError("StageManager is missing!");
        if (ui_Achievement == null)
            Debug.LogError("UI_Achievement is missing!");
    }

    public void CheckAchievements(StageData stageData)
    {
        if (stageManager == null || stageData == null)
        {
            Debug.LogWarning("Cannot check achievements: Required components are missing");
            return;
        }

        foreach (var achievement in stageData.achieveDatas)
        {
            switch ((AchievementRank)achievement.rank)
            {
                case AchievementRank.First:
                    FirstStarAchievement(achievement);
                    break;
                case AchievementRank.Second:
                    SecondStarAchievement(achievement);
                    break;
                case AchievementRank.Third:
                    ThirdStarAchievement(achievement);
                    break;
                case AchievementRank.Fourth:
                    //FourthStarAchievement(achievement);
                    break;
                default:
                    Debug.LogError($"Unknown achievement rank: {achievement.rank}");
                    break;
            }
        }
    }

    private void FirstStarAchievement(Achievement achievement)
    {
        // 스테이지 클리어
        if (!achievement.isCleared && stageManager.IsStageCleared)
        {
            achievement.isCleared = true;
            ShowAchievementPopup(achievement);
        }
    }


    private void SecondStarAchievement(Achievement achievement)
    {
        // 시간 제한 내 클리어
        if (!achievement.isCleared && stageManager.ElapsedTime <= stageManager.currentStageData.clearTime)
        {
            achievement.isCleared = true;
            ShowAchievementPopup(achievement);
        }
    }
    
    private void ThirdStarAchievement(Achievement achievement)
    {
        // 피격 없이 클리어
        if (!achievement.isCleared && stageManager.IsStageCleared && !stageManager.HasPlayerBeenHit)
        {
            achievement.isCleared = true;
            ShowAchievementPopup(achievement);
        }
    }

   /* private void FourthStarAchievement(Achievement achievement)
    {
        // 목표 킬 수 달성
        if (!achievement.isCleared && stageManager.KillCount >= achievement.targetKillCount)
        {
            achievement.isCleared = true;
            ShowAchievementPopup(achievement);
        }
    } */
    
    private void ShowAchievementPopup(Achievement achievement)
    {
        if (ui_Achievement != null)
        {
            ui_Achievement.ShowAchievement(achievement);
        }
        else
        {
            Debug.LogWarning("Cannot show achievement popup: UI_Achievement is missing");
        }
    }
    
}