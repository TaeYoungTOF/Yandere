using UnityEngine;

public class SkillData : ScriptableObject
{
    [Range(1, 5)] public int level;
    public string skillName;
    public Sprite skillIcon;
    
    [Multiline(3)] public string levelupTooltip;
}