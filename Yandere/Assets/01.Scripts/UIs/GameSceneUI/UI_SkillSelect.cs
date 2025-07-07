using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

public class UI_SkillSelect : ToggleableUI
{
    [SerializeField] private GameObject _skillSelectPanel;
    [SerializeField] private List<Button_Skill> _skillButtons;

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
        List<BaseSkill> options = SkillManager.Instance.GetSkillDatas(3);
        
        SetButtons(options);
    }
    
    public void SetButtons(List<BaseSkill> options)
    {
        for (int i = 0; i < _skillButtons.Count; i++)
        {
            if (i < options.Count)
            {
                 _skillButtons[i].Setup(options[i]);
            }
            else
            {
                _skillButtons[i].gameObject.SetActive(false);
            }
        }
    }
}
