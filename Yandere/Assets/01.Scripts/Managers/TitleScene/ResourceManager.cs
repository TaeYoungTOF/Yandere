using System;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

public class ResourceManager : Facility
{
   [Header("개인실 인포")]
   [SerializeField] private int currentStack = 0;                                                      // 방치형 현재 스택
   [SerializeField] private int maxStack = 12;                                                         // 방치형 최대 스택
   [SerializeField] private float stackIntervalMinutes = 60f;                                          // 스택 보상 시간 (60초)
   
   [Header("⏱ 디버그용")]
   [SerializeField] private string currentTimeFormatted;
   [SerializeField] private string timeUntilNextStack;

   [Header("UI 표시")]
   [SerializeField] private TextMeshProUGUI stackCountText;                                            // UI쪽에서 현재 카운트 스택 텍스트표시
   [SerializeField] private TextMeshProUGUI timerText;                                                 // UI쪽에서 현재 남은 시간 텍스트표시

   [SerializeField] private GameObject[] _photos = new GameObject[12];
   
   private float _timer = 0f;                                                                           //타이머

   private void Update()
   {
      _timer += Time.deltaTime;                                                      // timer를 델타타임 값을 계속 더해줌
      UpdateDebugInfo();
      
      float remain = Mathf.Max(0f, (stackIntervalMinutes * 60f) - _timer);
      TimeSpan remainingTime = TimeSpan.FromSeconds(remain);
      string formatted = string.Format("{0:D2}:{1:D2}", remainingTime.Minutes, remainingTime.Seconds);

      if (timerText != null)
         timerText.text = formatted;

      if (_timer >= stackIntervalMinutes * 60f)                                      // timer(0초)가 stackIntervalMinutes(즉 3600초)보다 크거나 같으면
      {
         SetPhoto();

         _timer = 0f;
      }
   }

   protected override void Init()
   {
      base.Init();
      
      currentLevel++;
      
      for (int i = 0; i < _photos.Length; i++)
      {
         _photos[i].SetActive(false);
      }
   }

   private void SetPhoto()
   {
      if (currentStack >= maxStack) return;
      
      currentStack++;                                                         // currentStack 값을 증가 시킴
      Debug.Log($"[테스트] 스택 증가! 현재 스택: {currentStack}");
      
      while (true)
      {
         int randomIndex = UnityEngine.Random.Range(0, _photos.Length);
         
         if (!_photos[randomIndex].activeSelf)
         {
            _photos[randomIndex].SetActive(true);
            break;
         }
      }
      
      UpdateUI();
   }
   
   public void ConsumePhoto()
   {
      if (currentStack > 0)
      {
         currentStack--;

         DataManager.Instance.AddObsessionCrystals(amount);
         UpdateUI();
         Debug.Log($"[테스트] 스택 소모! 획득 재화: {amount}, 남은 스택: {currentStack}");
      }
      else
      {
         Debug.Log($"[테스트] 스택이 없습니다. 현재 스택: {currentStack}");
      }
   }
   
   protected override void UpdateUI()
   {
      levelText.text = $"Lv.{currentLevel.ToString()}";
      stackCountText.text = $"{currentStack.ToString()} / {maxStack.ToString()}";
      levelDescriptionText.text = $"1시간마다 + {amount}";
   }
   
   
   
   // 디버그용 코드
   void UpdateDebugInfo()
   {
      DateTime now = DateTime.Now;
      currentTimeFormatted = now.ToString("yyyy-MM-dd HH:mm:ss");

      float remain = Mathf.Max(0f, (stackIntervalMinutes * 60f) - _timer);
      TimeSpan remainingTime = TimeSpan.FromSeconds(remain);
      timeUntilNextStack = remainingTime.ToString(@"hh\:mm\:ss");
   }

   [Button]
   private void DebugTimer()
   {
      AddTestTime(3000);
   }
   
   // 디버그용 코드
   public void AddTestTime(float time)
   {
      _timer += time;
      Debug.Log($"[디버그]{time}초 추가 됨. 현재 타이머: {_timer}초");
   }
}
