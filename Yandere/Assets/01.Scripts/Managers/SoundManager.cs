using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    
    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private int sfxVoices = 8;                 // 동시 재생 가능한 SFX 소스 수
    private AudioSource[] sfxSource;
    private int sfxIndex = 0;
    
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
        // ▶ SFX용 다중 AudioSource 생성
        sfxVoices = Mathf.Max(2, sfxVoices); // 최소 2개 권장
        sfxSource = new AudioSource[sfxVoices];
        for (int i = 0; i < sfxVoices; i++)
        {
            var src = gameObject.AddComponent<AudioSource>();
            src.playOnAwake = false;
            src.loop = false;
            sfxSource[i] = src;
        }

        if (_settingPanel != null) _settingPanel.SetActive(false);
        
        // 🔊 BGM을 최우선으로
        if (bgmSource != null)
        {
            bgmSource.priority = 0;            // 0 = 최우선, 256 = 최하
            bgmSource.ignoreListenerPause = true; // (선택) 전체 일시정지와 분리
        }

        // 🔊 SFX는 BGM보다 낮게
        if (sfxSource != null)
        {
            foreach (var src in sfxSource)
                if (src != null) src.priority = 128; // 또는 192/256
        }
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
        
        else // SFX
        {
            PlaySFXData(data);
        }

        // else if (data.soundType == SoundType.SFX)
        // {
        //    sfxSource.clip = data.soundClips[0];
        //    sfxSource.volume = data.volume * masterVolume * sfxVolume;
        //    sfxSource.pitch = data.pitch;
        //    sfxSource.loop = data.loop;
        //    sfxSource.Play();
        //     //sfxSource.PlayOneShot(data.soundClips[0]);
        // }
    }
    private void PlaySFXData(SoundData data)
    {
        if (data.soundClips == null || data.soundClips.Count == 0) return;

        // 랜덤 클립 하나 선택
        var clip = data.soundClips[Random.Range(0, data.soundClips.Count)];

        // 순환으로 다음 소스 선택
        var src = sfxSource[sfxIndex];
        sfxIndex = (sfxIndex + 1) % sfxSource.Length;

        // 피치가 클립마다 다르면 PlayOneShot 전에 설정
        src.pitch = data.pitch;
        // loop는 PlayOneShot엔 의미 없음

        // 최종 볼륨
        float vol = data.volume * masterVolume * sfxVolume;

        // 겹쳐도 끊기지 않음
        src.PlayOneShot(clip, vol);
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
        if (!categorizedSFX.TryGetValue(category, out var list) || list.Count == 0)
        {
            Debug.LogWarning($"[SoundManager] 카테고리 '{category}'에 사운드가 없습니다.");
            return;
        }
        var data = list[Random.Range(0, list.Count)];
        PlaySFXData(data);
    }
   
}
