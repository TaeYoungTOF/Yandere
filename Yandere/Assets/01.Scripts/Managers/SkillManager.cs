using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public Player player;
    public List<BaseSkill> equippedSkills = new List<BaseSkill>();
    private Dictionary<BaseSkill, float> cooldownTimers = new Dictionary<BaseSkill, float>();

    void Update()
    {
        foreach (var skill in equippedSkills)
        {
            if (skill.skillType == SkillType.Active)
            {
                if (!cooldownTimers.ContainsKey(skill))
                {
                    cooldownTimers[skill] = 0f;
                }
                cooldownTimers[skill] -= Time.deltaTime;

                ActiveSkill activeSkill = skill as ActiveSkill;
                if (cooldownTimers[skill] <= 0f)
                {
                    skill.Activate(player.transform);
                    cooldownTimers[skill] = activeSkill.cooldown;
                }
            }
        }
    }

    public void EquipSkill(BaseSkill skill)
    {
        equippedSkills.Add(skill);
        if (skill.skillType == SkillType.Passive)
            skill.OnEquip(player.transform);
        if (skill.skillType == SkillType.Active)
            cooldownTimers[skill] = 0f;
    }
}
