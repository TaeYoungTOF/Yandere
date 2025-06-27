using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SkillSelect : ToggleableUI
{
    [SerializeField] private GameObject _skillSelectPanel;
    [SerializeField] private List<SkillButton> skillButtons;

    private void Start()
    {
        Init();
        _skillSelectPanel.SetActive(false);
    }

    protected override UIState GetUIState()
    {
        return UIState.SkillSelect;
    }

    public void Show(List<BaseSkill> options)
    {
        Debug.Log($"SkillSelectPanel 활성화 시도 전 상태: {gameObject.activeSelf}, 부모 상태: {transform.parent?.gameObject.activeSelf}");
        _skillSelectPanel.SetActive(true);

        for (int i = 0; i < skillButtons.Count; i++)
        {
            if (i < options.Count)
                skillButtons[i].Setup(options[i]);
            else
                skillButtons[i].gameObject.SetActive(false);
        }
        Time.timeScale = 0f;
    }

    public void Hide()
    {
        _skillSelectPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}
