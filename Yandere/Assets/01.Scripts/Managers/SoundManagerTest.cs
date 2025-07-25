using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerTest : MonoBehaviour
{
    public static SoundManagerTest Instance { get; private set; }
    
    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;
    
    [Header("Audio Data List")]
    [SerializeField] private List<SoundData> soundDataList;
    
    public float masterVolume = 1f;
    public float bgmVolume = 1f;
    public float sfxVolume = 1f;
    
    private Dictionary<string, SoundData> soundDictionary;

    [Header("UI")]
    [SerializeField] private GameObject _settingPanel;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        soundDictionary = new Dictionary<string, SoundData>();
        foreach (var sound in soundDataList)
        {
            if (!soundDictionary.ContainsKey(sound.soundName))
                soundDictionary.Add(sound.soundName, sound);
        }
        
        _settingPanel.SetActive(false);
    }

    public void Play(string soundName)
    {
        if (!soundDictionary.ContainsKey(soundName))
        {
            Debug.LogWarning($"[SoundManager] '{name}'가 없습니다.");
            return;
        }
        SoundData data = soundDictionary[soundName];

        if (data.soundType == SoundType.BGM)
        {
            bgmSource.clip = data.soundClip;
            bgmSource.volume = data.volume * masterVolume * bgmVolume;
            bgmSource.loop = data.loop;
            bgmSource.Play();
        }
        
        else if (data.soundType == SoundType.SFX)
        {
            sfxSource.PlayOneShot(data.soundClip, data.volume * masterVolume * sfxVolume);
        }
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }
    
    public void SetMasterVolume(float volume)
    {
        masterVolume = volume;
        
        if (bgmSource.isPlaying)
        {
            SoundData currentBGM = soundDataList.Find(x => x.soundClip == bgmSource.clip);
            if (currentBGM != null)
                bgmSource.volume = currentBGM.volume * masterVolume * bgmVolume;
        }
    }
    public void SetBGMVolume(float volume)
    {
        bgmVolume = volume;

        // 🔍 현재 재생 중인 BGM의 SoundData 찾아서 반영
        if (bgmSource.isPlaying)
        {
            SoundData currentBGM = soundDataList.Find(x => x.soundClip == bgmSource.clip);
            if (currentBGM != null)
            {
                float dataVol = currentBGM.volume;
                float finalVolume = dataVol * masterVolume * bgmVolume;
                bgmSource.volume = finalVolume;
            }
        }
    }
    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
    }

    public void OpenSettingPanel()
    {
        _settingPanel.SetActive(true);
    }
}
