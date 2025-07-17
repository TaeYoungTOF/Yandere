using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    private StageManager stageManager;
    private UI_Achievement uiAchievement;

    private void Awake()
    {
        stageManager = StageManager.Instance;
        uiAchievement = FindObjectOfType<UI_Achievement>();
    }

    public void CheckAchievements(StageData stageData)
    {
        if (stageManager == null || stageData == null)
            return;

        foreach (var achievement in stageData.achieveDatas)
        {
            switch (achievement.rank)
            {
                case 0:
                    FirstStarAchievement(achievement);
                    break;
                case 1:
                    SecondStarAchievement(achievement);
                    break;
                case 2:
                    ThirdStarAchievement(achievement);
                    break;
            }
        }
    }
    
    private void FirstStarAchievement(Achievement achievement)
    {
        // 스테이지 클리어
        if (!achievement.isCleared && stageManager.StageClear)
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
        if (!achievement.isCleared && stageManager.StageClear && !stageManager.HasPlayerBeenHit)
        {
            achievement.isCleared = true;
            ShowAchievementPopup(achievement);
        }
    }

    

    private void ShowAchievementPopup(Achievement achievement)
    {
        if (uiAchievement != null)
        {
            uiAchievement.ShowAchievement(achievement);
        }
    }
}