using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance { get; private set; }

    public const int maxLevel = 5;

    public List<BaseSkill> availableSkills;

    public List<ActiveSkill> equipedActiveSkills;
    public List<PassiveSkill> equipedPassiveskills;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        Init();
    }

    /**@todo GameManager 혹은 StageData에서 사용가능한 스킬목록 가져와서 시작될 때 _availableSkils 갱신*/
    private void Init()
    {
        foreach (var skill in availableSkills)
        {
            skill.Init();
        }
    }

    public List<BaseSkill> GetSkillDatas(int count)
    {
        if (count > availableSkills.Count)
            count = availableSkills.Count;

        List<BaseSkill> shuffledList = new List<BaseSkill>(availableSkills);

        for (int i = 0; i < shuffledList.Count; i++)
        {
            int randIndex = Random.Range(i, shuffledList.Count);
            (shuffledList[i], shuffledList[randIndex]) = (shuffledList[randIndex], shuffledList[i]);
        }

        List<BaseSkill> levelupDatas = shuffledList.GetRange(0, count);
        return levelupDatas;
    }

    private void Update()
    {
        foreach (var skill in equipedActiveSkills)
        {
            skill.UpdateCooldown();
            skill.TryActivate();
        }
    }
}
