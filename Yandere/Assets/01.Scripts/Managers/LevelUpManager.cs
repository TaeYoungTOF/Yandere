using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpManager : MonoBehaviour
{
    public SkillSelectUI skillSelectUI;
    public List<BaseSkill> allSkills;

    public void OnLevelUp()
    {
        var options = GetRandomSkillOptions(3);
        skillSelectUI.Show(options);
    }

    private List<BaseSkill> GetRandomSkillOptions(int count)
    {
        List<BaseSkill> available = new List<BaseSkill>();

        foreach (var skill in allSkills)
        {
            if (!SkillManagerInstance().equippedSkills.Contains(skill) || skill.level < 5)
                available.Add(skill);
        }

        List<BaseSkill> result = new List<BaseSkill>();
        for (int i = 0; i < count; i++)
        {
            if (available.Count == 0) break;

            int rand = Random.Range(0, available.Count);
            result.Add(available[rand]);
            available.RemoveAt(rand);
        }
        return result;
    }

    private SkillManager SkillManagerInstance()
    {
        return FindObjectOfType<SkillManager>();  // 인스턴스 가져오기
    }
}
