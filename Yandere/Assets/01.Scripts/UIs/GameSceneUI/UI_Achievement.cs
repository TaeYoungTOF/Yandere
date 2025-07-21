using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Achievement : ToggleableUI
{
    [Header("Achievement UI")]
    [SerializeField] private GameObject _achievementPanel;
    [SerializeField] private Button _backButton;
    
    [Header("Achivement UI ItemList")]
    // 각 업적 UI 요소들을 묶어서 관리합니다.
    [SerializeField] private List<AchievementUIItem> _uiItems;

    [Header("Achivement Icon Sprites")]
    [SerializeField] private Sprite unlockedSprite;
    [SerializeField] private Sprite lockedSprite;
    

    private void Start()
    {
        Init(_achievementPanel);
        _achievementPanel.SetActive(false);
        
        _backButton.onClick.RemoveAllListeners();
        _backButton.onClick.AddListener(OnClickbackButton);
    }
    
    public override void Show()
    {
        _achievementPanel.SetActive(true);
        // 여기에 게임 매니저 등에서 실제 업적 데이터를 가져와 UI를 업데이트하는 로직을 호출합니다.
        // 예시: UpdateAchievementUI(GameManager.Instance.GetAchievementsData());
    }

    public override void Hide()
    {
        _achievementPanel.SetActive(false);
    }
    
    /// <summary>
    /// 실제 게임 데이터로 모든 업적 UI를 업데이트합니다.
    /// </summary>
    /// <param name="achievement">게임의 모든 업적 데이터 리스트</param>
    public void UpdateAllAchievements(List<Achievement> achievementsData)
    {
        if (achievementsData == null) return;

        // UI 아이템 개수와 데이터 개수 중 작은 쪽을 기준으로 루프를 돕니다.
        int count = Mathf.Min(_uiItems.Count, achievementsData.Count);
        for (int i = 0; i < count; i++)
        {
            UpdateAchievementItem(_uiItems[i], achievementsData[i]);
        }
    }

    /// <summary>
    /// 하나의 업적 UI 아이템을 데이터에 맞게 업데이트합니다.
    /// </summary>
    /// <param name="item">업데이트할 UI 요소들이 담긴 객체</param>
    /// <param name="data">업데이트에 사용할 실제 데이터</param>
    private void UpdateAchievementItem(AchievementUIItem item, Achievement data)
    {
        bool isUnlocked = data.currentProgress >= data.targetProgress;

        // 1. 설명 텍스트 업데이트
        if (item.descriptionText != null)
        {
            item.descriptionText.text = data.description;
        }

        // 2. 진행률 텍스트 업데이트
        if (item.progressText != null)
        {
            item.progressText.text = $"{data.currentProgress} / {data.targetProgress}";
        }

        // 3. 진행률 슬라이더 업데이트
        if (item.progressBar != null)
        {
            item.progressBar.value = (float)data.currentProgress / data.targetProgress;
        }

        // 4. 별 아이콘 활성화/비활성화
        if (item.starIcon != null)
        {
            item.starIcon.SetActive(isUnlocked);
        }
    }
    
    /// <summary>
    /// 업적 달성 시 팝업과 함께 애니메이션을 보여주는 함수
    /// </summary>
    public void ShowAchievementUnlockAnimation(int achievementIndex, Achievement finalData)
    {
        if (achievementIndex < 0 || achievementIndex >= _uiItems.Count) return;

        AchievementUIItem item = _uiItems[achievementIndex];
        
        // 최종 상태로 UI 즉시 업데이트
        UpdateAchievementItem(item, finalData);

        // 슬라이더 애니메이션 재생
        if (item.progressBar != null)
        {
            StartCoroutine(AnimateSlider(item.progressBar, (float)(finalData.currentProgress - 1) / finalData.targetProgress, 1f));
        }
    }
    
    private IEnumerator AnimateSlider(Slider slider, float startValue, float endValue)
    {
        float elapsedTime = 0f;
        float duration = 0.5f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float normalizedTime = elapsedTime / duration;
            float currentValue = Mathf.Lerp(startValue, endValue, EaseOutCubic(normalizedTime));
            slider.value = currentValue;
            yield return null;
        }
        slider.value = endValue;
    }
    private void OnClickbackButton()
    {
        UIManager.Instance.SetUIState(UIState.None);
    }

    private float EaseOutCubic(float t)
    {
        return 1 - Mathf.Pow(1 - t, 3);
    }

    public override UIState GetUIState()
    {
        return UIState.Achievement;
    }
    
    
}
