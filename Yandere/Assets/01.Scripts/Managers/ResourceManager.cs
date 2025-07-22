using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
   public int currentStack = 0;                                                     // 방치형 현재 스택
   public int maxStack = 12;                                                        // 방치형 최대 스택
   public float stackIntervalMinutes = 60f;                                         // 스택 보상 시간 (60초)

   private float timer = 0f;                                                        //타이머

   [Header("리소스 재화")]
   [SerializeField] private float resourceGold;

   [SerializeField] private float resourceCash;
   
   [Header("⏱ 디버그용")]
   [SerializeField] private string currentTimeFormatted;
   [SerializeField] private string timeUntilNextStack;

   [Header("디버그용 UI 표시")]
   [SerializeField] private TextMeshProUGUI stackCountText;
   [SerializeField] private TextMeshProUGUI timerText;

   private void Start()
   {
      stackCountText.text = $"{currentStack.ToString()} / {maxStack.ToString()}";
   }

   private void Update()
   {
      timer += Time.deltaTime;                                                      // timer를 델타타임 값을 계속 더해줌
      UpdateDebugInfo();
      
      float remain = Mathf.Max(0f, (stackIntervalMinutes * 60f) - timer);
      TimeSpan remainingTime = TimeSpan.FromSeconds(remain);
      string formatted = string.Format("{0:D2}:{1:D2}", remainingTime.Minutes, remainingTime.Seconds);

      if (timerText != null)
         timerText.text = formatted;

      if (timer >= stackIntervalMinutes * 60f)                                      // timer(0초)가 stackIntervalMinutes(즉 3600초)보다 크거나 같으면
      {
         if (currentStack < maxStack)                                               // currentStack이 maxStack보다 작을 경우
         {
            currentStack++;                                                         // currentStack 값을 증가 시킴
            RefreshStackCountText();
            Debug.Log($"[테스트] 스택 증가! 현재 스택: {currentStack}");
            
            // TODO: 랜덤 사진 생성 & 보상 표시 등
         }

         timer = 0f;
      }
   }
   public void ConsumePhoto()
   {
      if (currentStack > 0)
      {
         currentStack--;

         resourceGold += 100;
         RefreshStackCountText();
         Debug.Log($"[테스트] 스택 소모! 남은 스택: {currentStack}");
         
         // TODO: 보상 지급
      }
      
      Debug.Log($"[테스트] 스택이 없습니다. 현재 스택: {currentStack}");
   }
   
   void RefreshStackCountText()
   {
      stackCountText.text = $"{currentStack.ToString()} / {maxStack.ToString()}";
   }
   
   
   // 디버그용 코드
   void UpdateDebugInfo()
   {
      DateTime now = DateTime.Now;
      currentTimeFormatted = now.ToString("yyyy-MM-dd HH:mm:ss");

      float remain = Mathf.Max(0f, (stackIntervalMinutes * 60f) - timer);
      TimeSpan remainingTime = TimeSpan.FromSeconds(remain);
      timeUntilNextStack = remainingTime.ToString(@"hh\:mm\:ss");
   }
   
   // 디버그용 코드
   public void AddTestTime(float time)
   {
      timer += time;
      Debug.Log($"[디버그]{time}초 추가 됨. 현재 타이머: {timer}초");
   }
   
 
}
