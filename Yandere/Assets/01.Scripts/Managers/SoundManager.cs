using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    
    // 사운드의 타입
    public enum EBgm
    {
        BGM_TITLE,
        BGM_MAIN,
    }
    
    public enum ESfx
    {
        SFX_BUTTON,
        SFX_ENDING,
        SFX_CLCIK,
    }
    
    // audioclop 담을 수 있는 배열
    [SerializeField] private AudioClip[] bgms;
    [SerializeField] private AudioMixerGroup[] sfxs;
    
    // 플레이 하는 audiosource
    [SerializeField] AudioSource audioBgm;
    [SerializeField] AudioSource audioSfx;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    
}
