using System;
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
        
        InitNewData();
    }
    
    public void GainExp(float amount)
    {
        currentExp += amount;

        while (currentExp >= requiredExp)
        {
            currentExp -= requiredExp;
            LevelUp();
        }
        UIManager_Title.Instance.UpdateUI();
    }

    private void LevelUp()
    {
        accountLevel++;
        requiredExp *= 1.3f;
    }

    public void AddObsessionCrystals(float amount)
    {
        obsessionCrystals += amount;
        UIManager_Title.Instance.UpdateUI();
    }

    public void AddPremiumCurrency(float amount)
    {
        premiumCurrency += amount;
        UIManager_Title.Instance.UpdateUI();
    }

    public void InitNewData()
    {
        playerId = Guid.NewGuid().ToString();
        accountLevel = 1;
        currentExp = 0;
        requiredExp = 100;

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

    [Button]
    private void Debug_GainExp()
    {
        GainExp(10);
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
