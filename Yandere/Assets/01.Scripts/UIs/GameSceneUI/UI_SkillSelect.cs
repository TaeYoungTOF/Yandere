using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SkillSelect : ToggleableUI
{
    [SerializeField] private GameObject _skillSelectPanel;
    [SerializeField] private GameObject _skillSelectButton;
    [SerializeField] private Transform _contentParent;

    [SerializeField] private GameObject _hpButton;
    [SerializeField] private GameObject _goldButton;

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

        if (options.Count > 0)
        {
            InstantiateSkillButtons(options);
        }
        else
        {
            InstantiateHpAndGoldButton();
        }        
    }
    
    private void InstantiateSkillButtons(List<BaseSkill> options)
    {
        for (int i = 0; i < options.Count; i++)
        {
            GameObject buttonObj = Instantiate(_skillSelectButton, _contentParent);
            Button_Skill skillButton = buttonObj.GetComponent<Button_Skill>();
            skillButton.Setup(options[i]);
        }
    }

    private void InstantiateHpAndGoldButton()
    {
        GameObject hpButtonObj = Instantiate(_hpButton, _contentParent);
        Button hpButton = hpButtonObj.GetComponent<Button>();
        hpButton.onClick.AddListener(OnClickHpButton);
        
        GameObject goldButtonObj = Instantiate(_goldButton, _contentParent);
        Button goldButton = goldButtonObj.GetComponent<Button>();
        goldButton.onClick.AddListener(OnClickGoldButton);
    }

    public void DestroyButtons()
    {
        for (int i = _contentParent.childCount - 1; i >= 0; i--)
        {
            Destroy(_contentParent.GetChild(i).gameObject);
        }
    }

    private void OnClickHpButton()
    {
        StageManager.Instance.Player.stat.ChangeCurrentHp(50);
        UIManager.Instance.SetUIState(UIState.None);
    }
    
    private void OnClickGoldButton()
    {
        StageManager.Instance.ChangeGoldCount(50);
        UIManager.Instance.SetUIState(UIState.None);
    }
}
