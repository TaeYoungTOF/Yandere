using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance { get; private set; }

    public List<BaseSkill> availableSkills;

    public List<ActiveSkill> equipedActiveSkills;
    public List<PassiveSkill> equipedPassiveSkills;

    public readonly int MaxLevel = 5;
    private bool _isFirstDraw;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        Init();
    }

    private void Init()
    {
        availableSkills = new List<BaseSkill>();

        BaseSkill[] skills = GetComponentsInChildren<BaseSkill>();

        foreach (BaseSkill skill in skills)
        {
            if (skill != null)
            {
                skill.Init();
                availableSkills.Add(skill);
            }
        }

        _isFirstDraw = true;
    }

    private void Update()
    {
        foreach (ActiveSkill skill in equipedActiveSkills)
        {
            skill.UpdateCooldown();
            skill.TryActivate();
        }
    }

    public List<BaseSkill> GetSkillDatas(int count)
    {
        List<BaseSkill> shuffledList = new List<BaseSkill>(availableSkills);

        if (_isFirstDraw)
        {
            shuffledList.RemoveAll(skill => skill is PassiveSkill);
            _isFirstDraw = false;
        }

        if (count > shuffledList.Count)
            count = shuffledList.Count;

        for (int i = shuffledList.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (shuffledList[i], shuffledList[j]) = (shuffledList[j], shuffledList[i]);
        }

        return shuffledList.GetRange(0, count);
    }
}
