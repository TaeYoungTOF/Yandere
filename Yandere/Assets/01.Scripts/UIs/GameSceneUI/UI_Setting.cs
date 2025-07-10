using UnityEngine;
using UnityEngine.UI;

public class UI_Setting : ToggleableUI
{
    [SerializeField] private GameObject _settingPanel;
    [SerializeField] private Button _backButton;

    void Start()
    {
        Init(_settingPanel);
        _settingPanel.SetActive(false);
        
        _backButton.onClick.RemoveAllListeners();
        _backButton.onClick.AddListener(OnClickReturnButton);
    }

    public override UIState GetUIState()
    {
        return UIState.Setting;
    }

    public void OnClickReturnButton()
    {
        UIManager.Instance.SetUIState(UIState.Pause);
    }
}
