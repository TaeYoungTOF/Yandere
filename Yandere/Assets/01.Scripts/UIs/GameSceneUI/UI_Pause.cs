using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_Pause : ToggleableUI
{
    [SerializeField] private GameObject _pausePanel;
    
    [Header("Pause Panel")]
    [SerializeField] private Button _settingButton;
    [SerializeField] private Button _homeButton;
    [SerializeField] private Button _backButton;
    [SerializeField] private GameObject _confirmPanel;
    [SerializeField] private Button _confirmHomeButton;
    [SerializeField] private Button _confirmCancelButton;

    [Header("Equipped Skills")]
    [SerializeField] private Transform _activeSkillParent;
    [SerializeField] private Transform _passiveSkillParent;
    [SerializeField] private GameObject _skillSlot;
    
    private void Start()
    {
        Init(_pausePanel);
        _pausePanel.SetActive(false);
        

        _settingButton.onClick.RemoveAllListeners();
        _settingButton.onClick.AddListener(OnClickSettingButton);

        _homeButton.onClick.RemoveAllListeners();
        _homeButton.onClick.AddListener(OnClickHomeButton);

        _backButton.onClick.RemoveAllListeners();
        _backButton.onClick.AddListener(OnClickBackButton);
        
        _confirmHomeButton.onClick.RemoveAllListeners();
        _confirmHomeButton.onClick.AddListener(OnClickConfirmHomeButtonButton);
        
        _confirmCancelButton.onClick.RemoveAllListeners();
        _confirmCancelButton.onClick.AddListener(OnClickconfirmCancelButton);
    }
    
    public override UIState GetUIState()
    {
        return UIState.Pause;
    }

    public override void UIAction()
    {
        ClearSkillSlots();
        InstantiateActiveSkillSlots();
        InstantiatePassiveSkillSlots();
    }
    
    private void ClearSkillSlots()
    {
        foreach (Transform child in _activeSkillParent)
            Destroy(child.gameObject);
        foreach (Transform child in _passiveSkillParent)
            Destroy(child.gameObject);
    }

    private void InstantiateActiveSkillSlots()
    {
        List<ActiveSkill> skills = SkillManager.Instance.equipedActiveSkills;

        foreach (ActiveSkill activeSkill in skills)
        {
            GameObject slotGo = Instantiate(_skillSlot,  _activeSkillParent);
            Button_SkillSlot slot = slotGo.GetComponent<Button_SkillSlot>();
            slot.SetSkillSlot(activeSkill.skillIcon, activeSkill.level);
        }
    }

    private void InstantiatePassiveSkillSlots()
    {
        List<PassiveSkill> skills = SkillManager.Instance.equipedPassiveSkills;

        foreach (PassiveSkill passiveSkill in skills)
        {
            GameObject slotGo = Instantiate(_skillSlot,  _passiveSkillParent);
            Button_SkillSlot slot = slotGo.GetComponent<Button_SkillSlot>();
            slot.SetSkillSlot(passiveSkill.skillIcon, passiveSkill.level);
        }
    }

    private void OnClickSettingButton()
    {
        UIManager.Instance.SetUIState(UIState.Setting);
    }

    private void OnClickHomeButton()
    {
        ActiveConfirmPanel();
    }

    private void OnClickBackButton()
    {
        UIManager.Instance.SetUIState(UIState.None);
    }

    private void ActiveConfirmPanel()
    {
        _confirmPanel.SetActive(true);
    }

    private void OnClickConfirmHomeButtonButton()
    {
        SceneManager.LoadScene("TitleScene");
        UIManager.Instance.SetUIState(UIState.Lobby);
    }

    private void OnClickconfirmCancelButton()
    {
        _confirmPanel.SetActive(false);
    }
}
