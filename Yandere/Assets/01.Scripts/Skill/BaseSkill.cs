using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSkill : ScriptableObject
{
    [Header("Basic Info")]
    public string skillName;
    public Sprite skillIcon;
    public string description;
    public SkillType skillType;
    public int level = 1;

    [Header("Upgrade")]
    public float levelDamageBonus;
    public float levelCooldownReduction;

    public virtual void LevelUp()
    {
        level++;
    }

    // 공통 인터페이스
    public virtual void Activate(Transform caster) { }
    public virtual void OnEquip(Transform caster) { }
    public virtual void OnUnequip(Transform caster) { }

}
