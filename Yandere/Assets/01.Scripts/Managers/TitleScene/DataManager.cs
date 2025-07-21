using System;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }

    [Header("Player Info")]
    public string playerId;
    public int accountLevel;
    public double totalPlayTime; // 누적 접속 시간 (초)

    [Header("Currencies")]
    public int obsessionCrystals; // 집착의 결정 (기본 재화)
    public int premiumCurrency;   // 유료 재화 (ex. 다이아몬드)

    [Header("Facility Upgrade State")]
    public FacilitySaveData facilitySaveData;

    [Header("Settings")]
    public SettingData settingData;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void InitNewData()
    {
        playerId = Guid.NewGuid().ToString();
        accountLevel = 1;
        totalPlayTime = 0;

        obsessionCrystals = 0;
        premiumCurrency = 0;

        facilitySaveData = new FacilitySaveData();
        settingData = new SettingData();
    }

    public AccountSaveData CreateSaveData()
    {
        return new AccountSaveData()
        {
            playerId = this.playerId,
            accountLevel = this.accountLevel,
            totalPlayTime = this.totalPlayTime,
            obsessionCrystals = this.obsessionCrystals,
            premiumCurrency = this.premiumCurrency,
            facilitySaveData = this.facilitySaveData,
            settingData = this.settingData
        };
    }

    public void LoadFromSave(AccountSaveData save)
    {
        this.playerId = save.playerId;
        this.accountLevel = save.accountLevel;
        this.totalPlayTime = save.totalPlayTime;
        this.obsessionCrystals = save.obsessionCrystals;
        this.premiumCurrency = save.premiumCurrency;
        this.facilitySaveData = save.facilitySaveData ?? new FacilitySaveData();
        this.settingData = save.settingData ?? new SettingData();
    }
}
