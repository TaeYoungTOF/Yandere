using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SkillSelect : ToggleableUI
{
    [SerializeField] private GameObject _skillSelectPanel;
    [SerializeField] private GameObject _skillSelectButton;
    [SerializeField] private Transform _contentParent;

    // 임시
    [SerializeField] private Button _goldButton;

    private void Start()
    {
        Init(_skillSelectPanel);
        _skillSelectPanel.SetActive(false);

        _goldButton.onClick.RemoveAllListeners();
        _goldButton.onClick.AddListener(OnClickGoldButton);
        _goldButton.gameObject.SetActive(false);
    }

    public override void Show()
    {
        throw new System.NotImplementedException();
    }

    public override void Hide()
    {
        throw new System.NotImplementedException();
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
            _goldButton.gameObject.SetActive(true);
        }        
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

    private void OnClickGoldButton()
    {
        UIManager.Instance.GetPanel<UI_GameHUD>().UpdateGold(10);
        UIManager.Instance.SetUIState(UIState.None);

        _goldButton.gameObject.SetActive(false);
    }
}
