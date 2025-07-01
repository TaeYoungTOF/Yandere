using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public Player player;
    public List<BaseSkill> equippedSkills = new List<BaseSkill>();
    private Dictionary<BaseSkill, float> cooldownTimers = new Dictionary<BaseSkill, float>();
    public List<BaseSkill> allSkillsToReset;


    private void Awake()
    {
        if (allSkillsToReset == null || allSkillsToReset.Count == 0)
        {
            return;
        }

        foreach (var skill in allSkillsToReset)
        {
            skill.level = 1;
        }
    }

    void Update()
    {
        foreach (var skill in equippedSkills)
        {
            if (skill.skillType != SkillType.Active) continue;

            if (!cooldownTimers.ContainsKey(skill))
                cooldownTimers[skill] = 0f;

            cooldownTimers[skill] -= Time.deltaTime;

            ActiveSkill activeSkill = skill as ActiveSkill;
            if (activeSkill == null) continue;

            // 안전하게 인덱스 확보
            int levelIndex = Mathf.Clamp(activeSkill.level - 1, 0, activeSkill.levelStats.Count - 1);

            if (cooldownTimers[skill] <= 0f)
            {
                skill.Activate(player.transform);

                // 쿨다운 갱신
                cooldownTimers[skill] = activeSkill.levelStats[levelIndex].cooldown;
            }
        }
    }

    public void EquipSkill(BaseSkill skill)
    {
        if (!equippedSkills.Contains(skill))
        {
            equippedSkills.Add(skill);
            cooldownTimers[skill] = 0f; // 즉시 사용 가능
        }
    }
}
