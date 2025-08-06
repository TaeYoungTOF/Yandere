using System;
using System.Collections;
using NaughtyAttributes;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }

    [Header("Player Info")]
    public string playerId;
    public int accountLevel;
    public float currentExp;
    public float RequiredExp => 70 + accountLevel * 30;

    [Header("Currencies")]
    public float obsessionCrystals;
    public float premiumCurrency;

    [Header("Facilities")]
    public int[] facilityLevels;
    public float[] inGameDatas;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        
        SaveLoadManager.Instance.RequireData();

        SetInGameData(11);
    }
    
    private void GainExp(float amount)
    {
        currentExp += amount;

        while (currentExp >= RequiredExp)
        {
            currentExp -= RequiredExp;
            LevelUp();
        }
    }

    private void LevelUp()
    {
        accountLevel++;
    }

    private void SetInGameData(int amount)
    {
        inGameDatas = new float[amount];
        for (int i = 0; i < amount; i++)
        {
            inGameDatas[i] = 0;    // 인게임 데이터 0으로 초기화 (초기값 필요시 변경)
        }
        
    }

    public void SetFacilityData(int index, int facilityLevel, float value)
    {
        facilityLevels[index] = facilityLevel;
        inGameDatas[index] = value;
    }

    public void CalculateReward(float exp, float gold)
    {
        obsessionCrystals += gold;
        GainExp(exp);
    }

    public void LoadFromSave(SaveData save)
    {
        this.playerId = save.playerId;
        this.accountLevel = save.accountLevel;
        this.obsessionCrystals = save.obsessionCrystals;
        this.premiumCurrency = save.premiumCurrency;
        //this.facilitySaveData = save.facilitySaveData ?? new FacilitySaveData();
    }

    [Button]
    private void Debug_GainExp()
    {
        GainExp(100);
        UIManager_Title.Instance.UpdateUI();
    }
}
