using UnityEngine;

public class AchivemenetManger : MonoBehaviour
{
    private StageManager stageManager;
    
    public void CheckAchievements(StageData stageData)
    {
        foreach (var achievement in stageData.achieveDatas)
        {
            switch (achievement.rank)
            {
                case 0: // 첫번째 별 
                    FirstStarAchievement(achievement);
                    break;
                case 1: // 두번째 별
                    SeconedStarAchievement(achievement);
                    break;
                case 2: // 세번째 별
                    ThirdStarAchievement(achievement);
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

    private void SeconedStarAchievement(Achievement achievement)
    {
        // 시간 제한 내 클리어
        float clearTime = stageManager.CurrentClearTime;
        if (!achievement.isCleared && clearTime <= stageManager.currentStageData.clearTime)
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

    private void ShowAchievementPopup(Achievement achievement)
    {
        // UI_Achievement를 통해 업적 달성 팝업 표시
        var uiAchievement = FindObjectOfType<UI_Achievement>();
        if (uiAchievement != null)
        {
            uiAchievement.ShowAchievement(achievement);
        }
    }
}