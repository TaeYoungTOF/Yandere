using System.Collections.Generic;
using UnityEngine;

public class UI_SkillSelect : ToggleableUI
{
    [SerializeField] private GameObject _skillSelectPanel;
    [SerializeField] private List<Button_Skill> _skillButtons;
    [SerializeField] private List<BaseSkill> _allSkills;


    private void Start()
    {
        Init(_skillSelectPanel);
        _skillSelectPanel.SetActive(false);
    }

    public override UIState GetUIState()
    {
        return UIState.SkillSelect;
    }

    public override void UIAction()
    {        
        var options = GetRandomSkillOptions(3);
        Show(options);
    }

    private List<BaseSkill> GetRandomSkillOptions(int count)
    {
        List<BaseSkill> available = new List<BaseSkill>();
        foreach (var skill in _allSkills)
        {
            if (!FindObjectOfType<SkillManager>().equippedSkills.Contains(skill) || skill.level < 5)
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

    public void Show(List<BaseSkill> options)
    {
        for (int i = 0; i < _skillButtons.Count; i++)
        {
            if (i < options.Count)
                _skillButtons[i].Setup(options[i]);
            else
                _skillButtons[i].gameObject.SetActive(false);
        }
    }
}
