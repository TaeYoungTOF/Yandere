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
    
    private Dictionary<string, SoundData> soundDictionary;

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
            bgmSource.volume = data.volume;
            bgmSource.loop = data.loop;
            bgmSource.Play();
        }
        
        else if (data.soundType == SoundType.SFX)
        {
            sfxSource.PlayOneShot(data.soundClip, data.volume);
        }
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }
}
