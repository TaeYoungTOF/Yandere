using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Achievement : ToggleableUI
{
    [Header("Achievement UI")]
    [SerializeField] private GameObject _achievementPanel;
    [SerializeField] private List<Image> achievementImages;
    [SerializeField] private List<Slider> achievementSliders;
    
    [Header("Achievement Images")]
    [SerializeField] private Sprite unlockedSprite;
    [SerializeField] private Sprite lockedSprite;
    
    private void Awake()
    {
        // UI Manager에 이 패널 등록
        UIManager.Instance.RegisterPanel(this);
        
        // 초기에는 패널을 비활성화
        if (_achievementPanel != null)
            _achievementPanel.SetActive(false);
    }
    
    
    
    private void OnEnable()
    {
        // UI가 활성화될 때 게임 상태 설정
        gameObject.SetActive(true);
        Debug.Log("업적 UI 활성화");
    }

    public void OpenAchievementUI()
    {
        // UI 상태를 Achievement로 설정
        UIManager.Instance.SetUIState(UIState.Achievement);
        Debug.Log("업적 UI 열기 시도");
    }
    
    public void CloseAchievementUI()
    {
        // UI 상태를 None으로 변경
        UIManager.Instance.SetUIState(UIState.None);
        Debug.Log("업적 UI 닫기");
    }


    public void ShowAchievement(Achievement achievement)
    {
        int rank = achievement.rank;
        if (rank >= 0 && rank < achievementImages.Count)
        {
            // 이미지 변경
            if (achievementImages[rank] != null)
            {
                achievementImages[rank].sprite = unlockedSprite;
            }
            
            // 슬라이더 값 변경 (UnscaledTime 사용)
            if (achievementSliders[rank] != null)
            {
                StartCoroutine(AnimateSliderUnscaled(achievementSliders[rank]));
            }
        }
    }
    
    private System.Collections.IEnumerator AnimateSliderUnscaled(Slider slider)
    {
        float startValue = slider.value;
        float endValue = 0f;
        float elapsedTime = 1f;
        float duration = 0.5f;
        
        while (elapsedTime < duration)
        {
            // Time.timeScale의 영향을 받지 않는 deltaTime 사용
            elapsedTime += Time.unscaledDeltaTime;
            float normalizedTime = elapsedTime / duration;
            
            float currentValue = Mathf.Lerp(startValue, endValue, EaseOutCubic(normalizedTime));
            slider.value = currentValue;
            
            yield return null;
        }
        
        slider.value = endValue;
    }

    public void UpdateAchievementUI(StageData stageData)
    {
        if (stageData == null || stageData.achieveDatas == null) return;
        
        for (int i = 0; i < stageData.achieveDatas.Count && i < achievementImages.Count; i++)
        {
            UpdateAchievementItem(i, stageData.achieveDatas[i].isCleared);
        }
    }

    private void UpdateAchievementItem(int index, bool isCleared)
    {
        if (index < 0 || index >= achievementImages.Count) return;

        // 이미지 업데이트
        if (achievementImages[index] != null)
        {
            achievementImages[index].sprite = isCleared ? unlockedSprite : lockedSprite;
        }
        
        // 슬라이더 업데이트 (즉시 변경)
        if (achievementSliders[index] != null)
        {
            achievementSliders[index].value = isCleared ? 1f : 0f;
        }
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