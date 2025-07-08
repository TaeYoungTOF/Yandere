using System.Collections.Generic;
using UnityEngine;

public class UI_SkillSelect : ToggleableUI
{
    [SerializeField] private GameObject _skillSelectPanel;
    [SerializeField] private GameObject _skillSelectButton;
    [SerializeField] private Transform _contentParent;

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
        
        InstantiateSkillButtons(options);
    }
    
    public void InstantiateSkillButtons(List<BaseSkill> options)
    {
        for (int i = 0; i < options.Count; i++)
        {
            GameObject buttonObj = Instantiate(_skillSelectButton, _contentParent);
            Button_Skill skillButton = buttonObj.GetComponent<Button_Skill>();
            skillButton.Setup(options[i]);
        }
    }

    public void DestroyButtons()
    {
        for (int i = _contentParent.childCount - 1; i >= 0; i--)
        {
            Destroy(_contentParent.GetChild(i).gameObject);
        }
    }
}
