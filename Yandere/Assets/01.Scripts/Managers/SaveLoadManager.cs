using UnityEngine;
using System;
using System.IO;

[System.Serializable]
public class SaveData
{
    public string lastPlayTime;
    public float gold = 0f;
    public int stage = 1;

    // IUpgradble[] 정보 저장
}

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager Instance { get; private set; }

    private string SavePath => Path.Combine(Application.persistentDataPath, "save.json");

    public SaveData CurrentData { get; private set; }

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

        Load();
    }

    public void Save()
    {
        CurrentData.lastPlayTime = DateTime.Now.ToBinary().ToString();

        string json = JsonUtility.ToJson(CurrentData, true);
        File.WriteAllText(SavePath, json);
        Debug.Log("[SaveManager] 게임 저장 완료");
    }

    public void Load()
    {
        if (File.Exists(SavePath))
        {
            string json = File.ReadAllText(SavePath);
            CurrentData = JsonUtility.FromJson<SaveData>(json);
            Debug.Log("[SaveManager] 게임 불러오기 완료");
        }
        else
        {
            CurrentData = new SaveData(); // 최초 실행
            Debug.Log("[SaveManager] 저장 데이터 없음. 새로 생성");
        }
    }

    public DateTime GetLastPlayTime()
    {
        if (!string.IsNullOrEmpty(CurrentData.lastPlayTime))
        {
            long binary = Convert.ToInt64(CurrentData.lastPlayTime);
            return DateTime.FromBinary(binary);
        }

        return DateTime.Now;
    }
}
