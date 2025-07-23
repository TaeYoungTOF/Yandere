using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Facility : MonoBehaviour
{

    [SerializeField] private FacilityData _facilityData;
    
    [Header("시설 인포")]
    [SerializeField] private int currentLevel;
    [SerializeField] private float currentCost;
    
    [Header("⏱ 디버그용")]
    [SerializeField] private float testPlayerResourceGold;
    
    [Header("UI 표시")]
    [SerializeField] private TextMeshProUGUI facilityNameText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI levelDescriptionText;
    
    void Start()
    {
        Init();
        RefreshUI();
    }
    
    void Init()
    {
        currentLevel = 0;
        currentCost = _facilityData.baseCost;
        facilityNameText.text = _facilityData.facilityName;
        testPlayerResourceGold = 10000;
    }

    
    public void UpgradeButtonClick()
    {
        if (testPlayerResourceGold >= currentCost && currentLevel < _facilityData.maxLevel)
        {
            SoundManagerTest.Instance.Play("LobbyClick01_SFX");
            testPlayerResourceGold -= currentCost;
            currentLevel++;
            currentCost = Mathf.FloorToInt(currentCost * _facilityData.costMultiplier);
            RefreshUI();
            
            // TODO : 플레이어 능력치 올려주는 기능 등 추가
        }
        else
        {
            SoundManagerTest.Instance.Play("LobbyClick02_SFX");
            Debug.Log("[기록실] 업그레이드 불가 (재화 또는 최대레벨 입니다)");
        } 
    }

    void RefreshUI()
    {
        levelText.text = $"Lv.{currentLevel.ToString()}";
        levelDescriptionText.text = $"{_facilityData.statTargetText} + {10 + currentLevel}%";
    }
    
}
