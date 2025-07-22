using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AccountSaveData
{
    public string playerId;
    public int accountLevel;
    public double totalPlayTime;

    public int obsessionCrystals;
    public int premiumCurrency;

    public FacilitySaveData facilitySaveData;
    public SettingData settingData;
}

[System.Serializable]
public class FacilitySaveData
{
    public int[] facilityLevels = new int[5];
}

[System.Serializable]
public class SettingData
{
    public float bgmVolume = 1f;
    public float sfxVolume = 1f;
    public bool isVibrationOn = true;
}

[System.Serializable]
public class SaveData
{
    public string lastPlayTime;
    public float gold = 0f;
    public int stage = 1;
}
