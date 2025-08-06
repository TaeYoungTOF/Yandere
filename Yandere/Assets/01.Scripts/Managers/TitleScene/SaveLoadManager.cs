using UnityEngine;
using System;

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager Instance { get; private set; }
    
    private const int FacilityAmount = 11;
    
    [SerializeField] private SaveData saveData;         // 임시 필드

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (Instance != this)
            {
                Destroy(gameObject);
            }
        }
        
        // 임시 코드
        saveData = null;
    }

    public SaveData CreateSaveData()
    {
        var settingData = new SettingData()
        {
            masterVolume = SoundManager.Instance.masterVolume,
            bgmVolume = SoundManager.Instance.bgmVolume,
            sfxVolume = SoundManager.Instance.sfxVolume,
        };

        return new SaveData()
        {
            playerId = DataManager.Instance.playerId,
            accountLevel = DataManager.Instance.accountLevel,
            currentExp = DataManager.Instance.currentExp,
            
            obsessionCrystals = DataManager.Instance.obsessionCrystals,
            premiumCurrency = DataManager.Instance.premiumCurrency,
            
            facilityLevels = DataManager.Instance.facilityLevels,
            settingData = settingData
        };
    }

    public void RequireData()
    {
        if (saveData == null)
        {
            CreateNewData();
        }
        else
        {
            LoadSaveData(saveData);
        }
    }

    private void CreateNewData()
    {
        Debug.Log("[SaveLoadManager] Create");
        
        DataManager.Instance.playerId = Guid.NewGuid().ToString();
        DataManager.Instance.accountLevel = 1;
        DataManager.Instance.currentExp = 0;

        DataManager.Instance.obsessionCrystals = 0;
        DataManager.Instance.premiumCurrency = 0;
        
        DataManager.Instance.facilityLevels = new int[FacilityAmount];
        for (int i = 0; i < FacilityAmount; i++)
        {
            DataManager.Instance.facilityLevels[i] = 0;
        }

        SoundManager.Instance.masterVolume = 1f;
        SoundManager.Instance.bgmVolume = 1f;
        SoundManager.Instance.sfxVolume = 1f;
    }

    private void LoadSaveData(SaveData save)
    {
        Debug.Log("[SaveLoadManager] Load");
        
        DataManager.Instance.playerId = save.playerId;
        DataManager.Instance.accountLevel = save.accountLevel;
        DataManager.Instance.currentExp = save.currentExp;
        
        DataManager.Instance.obsessionCrystals = save.obsessionCrystals;
        DataManager.Instance.premiumCurrency = save.premiumCurrency;
        
        DataManager.Instance.facilityLevels = save.facilityLevels;

        SoundManager.Instance.masterVolume = save.settingData.masterVolume;
        SoundManager.Instance.bgmVolume = save.settingData.bgmVolume;
        SoundManager.Instance.sfxVolume = save.settingData.sfxVolume;
    }

    public void UpdateSaveData(SaveData save)
    {
        saveData = save;
    }
}
