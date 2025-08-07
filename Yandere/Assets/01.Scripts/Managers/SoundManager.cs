using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    
    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;
    
    [Header("Audio Data List")]
    [SerializeField] private List<SoundData> soundDataList;
    
    public float masterVolume = 1f;
    public float bgmVolume = 1f;
    public float sfxVolume = 1f;
    
    private Dictionary<string, SoundData> soundDictionary;
    private Dictionary<SoundCategory, List<SoundData>> categorizedSFX = new();

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
            // 🔥 SFX만 분류
            if (sound.soundType == SoundType.SFX)
            {
                if (!categorizedSFX.ContainsKey(sound.soundCategory))
                    categorizedSFX[sound.soundCategory] = new List<SoundData>();

                categorizedSFX[sound.soundCategory].Add(sound);
            }
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
            bgmSource.clip = data.soundClips[0];
            bgmSource.volume = data.volume * masterVolume * bgmVolume;
            bgmSource.loop = data.loop;
            bgmSource.pitch = data.pitch;
            bgmSource.Play();
        }

        else if (data.soundType == SoundType.SFX)
        {
           sfxSource.clip = data.soundClips[0];
           sfxSource.volume = data.volume * masterVolume * sfxVolume;
           sfxSource.pitch = data.pitch;
           sfxSource.loop = data.loop;
           sfxSource.Play();
            //sfxSource.PlayOneShot(data.soundClips[0]);
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
            SoundData currentBGM = soundDataList.Find(x =>
                x.soundClips != null && x.soundClips.Contains(bgmSource.clip));

            if (currentBGM != null)
                bgmSource.volume = currentBGM.volume * masterVolume * bgmVolume;
        }
    }
    public void SetBGMVolume(float volume)
    {
        bgmVolume = volume;

        if (bgmSource.isPlaying)
        {
            SoundData currentBGM = soundDataList.Find(x =>
                x.soundClips != null && x.soundClips.Contains(bgmSource.clip));

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
        Play("LobbyClick01_SFX");
    }

    public void PlayRandomSFX(SoundCategory category)
    {
        if (!categorizedSFX.ContainsKey(category) || categorizedSFX[category].Count == 0)
        {
            Debug.LogWarning($"[SoundManager] 카테고리 '{category}'에 사운드가 없습니다.");
            return;
        }

        List<SoundData> list = categorizedSFX[category];
        SoundData selected = list[Random.Range(0, list.Count)];

        // ✅ 랜덤 클립 선택
        if (selected.soundClips == null || selected.soundClips.Count == 0)
        {
            Debug.LogWarning($"[SoundManager] '{category}' 카테고리의 SoundData에 클립이 없습니다.");
            return;
        }
        
        AudioClip clip = selected.soundClips[Random.Range(0, selected.soundClips.Count)];
        
        sfxSource.clip = clip;
        sfxSource.volume = selected.volume * masterVolume * sfxVolume;
        sfxSource.pitch = selected.pitch;
        sfxSource.loop = false;
        sfxSource.Play();
    }
   
}
