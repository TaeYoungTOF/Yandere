using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
   [SerializeField] FacilityData facilityData;

   [Header("개인실 인포")]
   [SerializeField] private int currentLevel;                                                          // 개인실 현재 레벨
   [SerializeField] private int currentStack = 0;                                                      // 방치형 현재 스택
   [SerializeField] private int maxStack = 12;                                                         // 방치형 최대 스택
   [SerializeField] private float stackIntervalMinutes = 60f;                                          // 스택 보상 시간 (60초)
   [SerializeField] private float currentCost;

   private float timer = 0f;                                                                           //타이머

   [Header("⏱ 디버그용 리소스 재화")]
   [SerializeField] private float resourceGold;                                                        // 집착의 결정?
   [SerializeField] private float resourceCash;                                                        // 유료 재화?
   
   [Header("⏱ 디버그용")]
   [SerializeField] private string currentTimeFormatted;
   [SerializeField] private string timeUntilNextStack;
   [SerializeField] private float testPlayerResourceGold;

   [Header("UI 표시")]
   [SerializeField] private TextMeshProUGUI facilityNameText;                                          // UI쪽에서 시설 이름 텍스트표시
   [SerializeField] private TextMeshProUGUI stackCountText;                                            // UI쪽에서 현재 카운트 스택 텍스트표시
   [SerializeField] private TextMeshProUGUI timerText;                                                 // UI쪽에서 현재 남은 시간 텍스트표시
   [SerializeField] private TextMeshProUGUI levelText;                                                 // UI쪽에서 현재 레벨 텍스트표시
   [SerializeField] private TextMeshProUGUI levelDescriptionText;                                      // UI쪽에서 현재 보상정보 텍스트표시

   private void Start()
   {
      
      Init();
      RefreshUI();

      //stackCountText.text = $"{currentStack.ToString()} / {maxStack.ToString()}";
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
            RefreshUI();
            Debug.Log($"[테스트] 스택 증가! 현재 스택: {currentStack}");
            
            // TODO: 랜덤 사진 생성 & 보상 표시 등
         }

         timer = 0f;
      }
   }

   public void Init()
   {
      currentCost = facilityData.baseCost;
      currentLevel = 1;
      testPlayerResourceGold = 10000;
      facilityNameText.text = facilityData.facilityName;
      
      
   }

   public void PrivateRoomUpgradeButtonClick()
   {
      
      if (testPlayerResourceGold >= currentCost && currentLevel < facilityData.maxLevel)
      {
         SoundManagerTest.Instance.Play("LobbyClick01_SFX");
         testPlayerResourceGold -= currentCost;
         currentLevel++;
         currentCost = Mathf.FloorToInt(currentCost * facilityData.costMultiplier);
         RefreshUI();
      }
      else
      {
         SoundManagerTest.Instance.Play("LobbyClick02_SFX");
         Debug.Log("[개인실] 업그레이드 불가 (재화 또는 최대레벨 입니다)");
      }
   }
   
   public void ConsumePhoto()
   {
      if (currentStack > 0)
      {
         currentStack--;

         resourceGold += 100;
         RefreshUI();
         Debug.Log($"[테스트] 스택 소모! 남은 스택: {currentStack}");
         
         // TODO: 보상 지급
      }
      
      Debug.Log($"[테스트] 스택이 없습니다. 현재 스택: {currentStack}");
   }
   
   void RefreshUI()
   {
      stackCountText.text = $"{currentStack.ToString()} / {maxStack.ToString()}";
      levelText.text = $"Lv.{currentLevel.ToString()}";
      levelDescriptionText.text = $"1시간마다 + {100 + currentLevel * 10}";
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

   [Button]
   private void DebugTimer()
   {
      AddTestTime(3000);
   }
   
   // 디버그용 코드
   public void AddTestTime(float time)
   {
      timer += time;
      Debug.Log($"[디버그]{time}초 추가 됨. 현재 타이머: {timer}초");
   }
}
