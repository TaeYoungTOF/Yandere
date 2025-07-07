using UnityEngine;

public enum SkillType
{
    Active,
    Passive
}

public class BaseSkill : ScriptableObject
{
    public string skillName;
    public Sprite skillIcon;
    public string description;
    public SkillType skillType;
    public int level = 1;

    public float levelDamageBonus;
    public float levelCooldownReduction;

    public virtual void LevelUp() { level++; }
    public virtual void Activate(Transform caster) { }
    public virtual void OnEquip(Transform caster) { }
    public virtual void OnUnequip(Transform caster) { }

}
