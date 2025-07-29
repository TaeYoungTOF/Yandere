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
    public float requiredExp;

    [Header("Currencies")]
    public float obsessionCrystals;
    public float premiumCurrency;

    [Header("Facilities")]
    //public FacilitySaveData facilitySaveData;
    private int _facilityAmount = 11;
    public int[] facilityLevels;
    public float[] inGameDatas;

    [Header("Settings")]
    public SettingData settingData;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        
        InitNewData();
        
        facilityLevels = new int[_facilityAmount];
        inGameDatas = new float[_facilityAmount];
        for (int i = 0; i < _facilityAmount; i++)
        {
            facilityLevels[i] = 0; // 시설 레벨 0으로 초기화 (필요시 1로)
            inGameDatas[i] = 0;    // 인게임 데이터 0으로 초기화 (초기값 필요시 변경)
        }
    }

    private void InitNewData()
    {
        playerId = Guid.NewGuid().ToString();
        accountLevel = 1;
        currentExp = 0;
        requiredExp = 100;

        obsessionCrystals = 0;
        premiumCurrency = 0;

        //facilitySaveData = new FacilitySaveData();
        settingData = new SettingData();
    }
    
    public void GainExp(float amount)
    {
        currentExp += amount;

        while (currentExp >= requiredExp)
        {
            currentExp -= requiredExp;
            LevelUp();
        }
    }

    private void LevelUp()
    {
        accountLevel++;
        requiredExp *= 1.3f;
    }

    public void SetData(int index, int facilityLevel, float value)
    {
        facilityLevels[index] = facilityLevel;
        inGameDatas[index] = value;
    }

    public void CalculateReward(float exp, float gold)
    {
        obsessionCrystals += gold;
        GainExp(exp);
    }

    public AccountSaveData CreateSaveData()
    {
        return new AccountSaveData()
        {
            playerId = this.playerId,
            accountLevel = this.accountLevel,
            obsessionCrystals = this.obsessionCrystals,
            premiumCurrency = this.premiumCurrency,
            //facilitySaveData = this.facilitySaveData,
            settingData = this.settingData
        };
    }

    public void LoadFromSave(AccountSaveData save)
    {
        this.playerId = save.playerId;
        this.accountLevel = save.accountLevel;
        this.obsessionCrystals = save.obsessionCrystals;
        this.premiumCurrency = save.premiumCurrency;
        //this.facilitySaveData = save.facilitySaveData ?? new FacilitySaveData();
        this.settingData = save.settingData ?? new SettingData();
    }

    [Button]
    private void Debug_GainExp()
    {
        GainExp(50);
        UIManager_Title.Instance.UpdateUI();
    }
}
