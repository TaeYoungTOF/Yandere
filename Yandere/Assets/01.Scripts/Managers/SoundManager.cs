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
        BGM_GAME,
    }
    
    public enum ESfx
    {
        SFX_BUTTON,
        SFX_ENDING,
        SFX_CLCIK,
    }

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
