using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Pause : ToggleableUI
{
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
    
    private SkillManager _skillManager;
    
    private void Start()
    {
        Init();
        
        _skillManager = SkillManager.Instance;

        _settingButton.onClick.AddListener(OnClickSettingButton);
        _homeButton.onClick.AddListener(OnClickHomeButton);
        _backButton.onClick.AddListener(OnClickBackButton);
        _confirmHomeButton.onClick.AddListener(OnClickConfirmHomeButton);
        _confirmCancelButton.onClick.AddListener(OnClickConfirmCancelButton);
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
        InstantiateUpgradeSkillSlots();
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
        List<ActiveSkill> skills = _skillManager.equipedActiveSkills;

        foreach (ActiveSkill activeSkill in skills)
        {
            GameObject slotGo = Instantiate(_skillSlot,  _activeSkillParent);
            Button_SkillSlot slot = slotGo.GetComponent<Button_SkillSlot>();
            slot.SetSkillSlot(activeSkill.SkillIcon, activeSkill.level);
        }
    }

    private void InstantiatePassiveSkillSlots()
    {
        List<PassiveSkill> skills = _skillManager.equipedPassiveSkills;

        foreach (PassiveSkill passiveSkill in skills)
        {
            GameObject slotGo = Instantiate(_skillSlot,  _passiveSkillParent);
            Button_SkillSlot slot = slotGo.GetComponent<Button_SkillSlot>();
            slot.SetSkillSlot(passiveSkill.SkillIcon, passiveSkill.level);
        }
    }

    private void InstantiateUpgradeSkillSlots()
    {
        if (_skillManager.equipedUpgradeSkills.Count > 0)
        {
            List<UpgradeSkill> upgrades = _skillManager.equipedUpgradeSkills;

            foreach (UpgradeSkill upgrade in upgrades)
            {
                GameObject slotGo = Instantiate(_skillSlot,  _activeSkillParent);
                Button_SkillSlot slot = slotGo.GetComponent<Button_SkillSlot>();
                slot.SetSkillSlot(upgrade.SkillIcon, _skillManager.MaxLevel);
            }
        }
    }

    private void OnClickSettingButton()
    {
        SoundManager.Instance.OpenSettingPanel();
    }

    private void OnClickHomeButton()
    {
        ActiveConfirmPanel();
    }

    private void ActiveConfirmPanel()
    {
        _confirmPanel.SetActive(true);
        SoundManager.Instance.Play("LobbyClick01_SFX");
    }

    private void OnClickConfirmHomeButton()
    {
        GameManager.Instance.LoadScene(SceneName.TitleScene);
        SoundManager.Instance.Play("LobbyClick02_SFX");
    }

    private void OnClickConfirmCancelButton()
    {
        _confirmPanel.SetActive(false);
        SoundManager.Instance.Play("LobbyClick02_SFX");
    }
}
