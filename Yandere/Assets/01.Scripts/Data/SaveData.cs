using UnityEngine;

[System.Serializable]
public class SaveData
{
    [Header("Account Info")]
    public string playerId;
    public int accountLevel;
    public float currentExp;

    [Header("Currencies")]
    public float obsessionCrystals;
    public float premiumCurrency;

    [Header("Data Bundle")]
    public int[] facilityLevels;
    public SettingData settingData;
}

[System.Serializable]
public class SettingData
{
    [Range(0, 1)] public float masterVolume;
    [Range(0, 1)] public float bgmVolume;
    [Range(0, 1)] public float sfxVolume;
}