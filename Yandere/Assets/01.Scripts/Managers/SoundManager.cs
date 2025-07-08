using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    
    // 사운드의 타입
    public enum EBgm
    {
        BGM_MAIN,
    }
    
    public enum ESfx
    {
        SFX_LEVELUP,
        SFX_CLCIK,
    }
    
    // audioclop 담을 수 있는 배열
    [SerializeField] private AudioClip[] bgms;
    [SerializeField] private AudioClip[] sfxs;
    
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
    
    // EBgm 열거형을 매개변수로 받아 해당되는 배경음악 클립을 재생
    public void PlayBGM(EBgm bgmidx)
    {
        //enum int형으로 형변환
        audioBgm.clip = bgms[(int)bgmidx];
        audioBgm.Play();
    }
    
    // 현재 재생 중인 배경 음악 정지
    public void StopBGM()
    {
        audioBgm.Stop();
    }
    
    // ESfx를 열거형을 매개변수로 받아 해당되는 효과음을 재생
    public void PlaySFX(ESfx esfx)
    {
        audioSfx.PlayOneShot(sfxs[(int)esfx]);
    }

    public void OnClickBack()
    {
        SoundManager.Instance.PlayBGM(SoundManager.EBgm.BGM_MAIN);
        
    }
}
