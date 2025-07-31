using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SoundType
{
    BGM,
    SFX
}

public enum SoundCategory
{
    None,
    Fireball,
    BurstingGaze,
    ParchedLonging,
    RagingEmotions,
    EtchedHatred,
    PouringAffection
}

[CreateAssetMenu(fileName = "SoundData", menuName = "Audio/SoundData")]
public class SoundData : ScriptableObject
{
   public SoundType soundType;
   public SoundCategory soundCategory;
   public string soundName;
   //public AudioClip soundClip;
   [Range(0, 1)] public float volume = 1f;
   public bool loop = false;
   
   [Header("🎵 여러 클립 등록")]
   public List<AudioClip> soundClips = new(); // ✅ 변경: 단일 클립 → 리스트

}
