using System;
using UnityEngine;
using NaughtyAttributes;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }
    
    [Header("개인실 인포")]
    [SerializeField] private FacilityMain facilityMain;
    [SerializeField] private int currentStack = 0;                                                      // 방치형 현재 스택
    [SerializeField] private int maxStack = 12;                                                         // 방치형 최대 스택
    [SerializeField] private float stackIntervalMinutes = 60f;                                          // 스택 보상 시간 (60초)
   
    [Header("⏱ 디버그용")]
    [SerializeField] private string currentTimeFormatted;
    [SerializeField] private string timeUntilNextStack;
    
    private float _timer = 0f;                                                                           //타이머

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Update()
    {
        _timer += Time.deltaTime;                                                      // timer를 델타타임 값을 계속 더해줌
      
        float remain = Mathf.Max(0f, (stackIntervalMinutes * 60f) - _timer);
        TimeSpan remainingTime = TimeSpan.FromSeconds(remain);
        string formatted = string.Format("{0:D2}:{1:D2}", remainingTime.Minutes, remainingTime.Seconds);
        
        UpdateDebugInfo(remainingTime);

        facilityMain.UpdateTimeUI(formatted);
        
        if (_timer >= stackIntervalMinutes * 60f)                                      // timer(0초)가 stackIntervalMinutes(즉 3600초)보다 크거나 같으면
        {
            if (currentStack >= maxStack) return;
      
            currentStack++;                                                         // currentStack 값을 증가 시킴
            Debug.Log($"[테스트] 스택 증가! 현재 스택: {currentStack}");

            facilityMain.SetPhoto();
            facilityMain.UpdateStackUI(currentStack, maxStack);

            _timer = 0f;
        }
    }

    public void UseStack(float reward)
    {
        if (currentStack > 0)
        {
            currentStack--;

            AddObsessionCrystals(reward);
            Debug.Log($"[테스트] 스택 소모! 획득 재화: {reward}, 남은 스택: {currentStack}");
        }
        else
        {
            Debug.Log($"[테스트] 스택이 없습니다. 현재 스택: {currentStack}");
        }
    }

    private void AddObsessionCrystals(float amount)
    {
        DataManager.Instance.obsessionCrystals += amount;
        UIManager_Title.Instance.UpdateUI();
    }

    public void UseObsessionCrystals(float amount)
    {
        DataManager.Instance.obsessionCrystals -= amount;
        UIManager_Title.Instance.UpdateUI();
    }

    private void AddPremiumCurrency(float amount)
    {
        DataManager.Instance.premiumCurrency += amount;
        UIManager_Title.Instance.UpdateUI();
    }
    
    
    
   
    // ============================================디버그용 코드===============================================
    private void UpdateDebugInfo(TimeSpan remainingTime)
    {
        DateTime now = DateTime.Now;
        currentTimeFormatted = now.ToString("yyyy-MM-dd HH:mm:ss");
        
        timeUntilNextStack = remainingTime.ToString(@"hh\:mm\:ss");
    }

    [Button]
    private void Debug_Add3000Seconds()
    {
        AddTestTime(3000);
    }
   
    // 디버그용 코드
    private void AddTestTime(float time)
    {
        _timer += time;
        Debug.Log($"[디버그]{time}초 추가 됨. 현재 타이머: {_timer}초");
    }

    [Button]
    private void Debug_AddObsessionCrystals()
    {
        AddObsessionCrystals(100);
    }

    [Button]
    private void Debug_AddPremiumCurrency()
    {
        AddPremiumCurrency(100);
    }
}
