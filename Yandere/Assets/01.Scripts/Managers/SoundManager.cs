using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    // ✅ 어디서든 공유되는 최신값
    public static float MasterVol = 1f;
    public static float BgmVol = 1f;
    public static float SfxVol = 1f;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void LoadVolumesEarly()
    {
        MasterVol = PlayerPrefs.GetFloat("MasterVolume", 1f);
        BgmVol    = PlayerPrefs.GetFloat("BGMVolume", 1f);
        SfxVol    = PlayerPrefs.GetFloat("SFXVolume", 1f);
    }
    
    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;
    
    [Header("Audio Data List")]
    [SerializeField] private List<SoundData> soundDataList;

    // 인스펙터에 보이되 실제 값은 정적값을 미러링
    public float masterVolume { get; private set; }
    public float bgmVolume    { get; private set; }
    public float sfxVolume    { get; private set; }
    
    private Dictionary<string, SoundData> soundDictionary;
    private Dictionary<SoundCategory, List<SoundData>> categorizedSFX = new();

    [Header("UI")]
    [SerializeField] private GameObject _settingPanel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            masterVolume = MasterVol;
            bgmVolume    = BgmVol;
            sfxVolume    = SfxVol;

            InitSoundDictionary();
            _settingPanel.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitSoundDictionary()
    {
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
    
    public void SetMasterVolume(float v)
    {
        MasterVol = masterVolume = v;
        PlayerPrefs.SetFloat("MasterVolume", v);
        PlayerPrefs.Save();
        UpdateBGMVolume();
    }
    public void SetBGMVolume(float v)
    {
        BgmVol = bgmVolume = v;
        PlayerPrefs.SetFloat("BGMVolume", v);
        PlayerPrefs.Save();
        UpdateBGMVolume();
    }
    public void SetSFXVolume(float v)
    {
        SfxVol = sfxVolume = v;
        PlayerPrefs.SetFloat("SFXVolume", v);
        PlayerPrefs.Save();
    }

    private void UpdateBGMVolume()
    {
        if (!bgmSource || !bgmSource.isPlaying) return;
        var currentBGM = soundDataList.Find(x => x.soundClips != null && x.soundClips.Contains(bgmSource.clip));
        if (currentBGM != null)
            bgmSource.volume = currentBGM.volume * masterVolume * bgmVolume;
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
