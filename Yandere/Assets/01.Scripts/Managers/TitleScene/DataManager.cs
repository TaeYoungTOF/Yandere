using System;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }

    [Header("Player Info")]
    public string playerId;
    public int accountLevel;

    [Header("Currencies")]
    public int obsessionCrystals;
    public int premiumCurrency;

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
        this.obsessionCrystals = save.obsessionCrystals;
        this.premiumCurrency = save.premiumCurrency;
        this.facilitySaveData = save.facilitySaveData ?? new FacilitySaveData();
        this.settingData = save.settingData ?? new SettingData();
    }
}
