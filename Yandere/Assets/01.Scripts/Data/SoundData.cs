using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SoundType
{
    BGM,
    SFX
}

[CreateAssetMenu(fileName = "SoundData", menuName = "Audio/SoundData")]
public class SoundData : ScriptableObject
{
   public SoundType soundType;
   public string soundName;
   public AudioClip soundClip;
   [Range(0, 1)] public float volume = 1f;
   public bool loop = false;

}
