using UnityEngine;
using UnityEngine.UI;

public class UI_Pause : ToggleableUI
{
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private GameObject _ReturnHomePanel;
    [SerializeField] private GameObject _AchievementPanel;
    
    [Header("Achivement Panel")]
    //[SerializeField] private Button _achievementButton;

    [Header("Pause Panel")]
    [SerializeField] private Button _settingButton;
    [SerializeField] private Button _homeButton;
    [SerializeField] private Button _backButton;

    [Header("Return Home Panel")]
    [SerializeField] private Button _returnHomeButton;
    [SerializeField] private Button _cancelButton;

    private void Start()
    {
        Init(_pausePanel);
        _pausePanel.SetActive(false);
        _ReturnHomePanel.SetActive(false);
        
        Init(_AchievementPanel);
        _AchievementPanel.SetActive(false);
        _ReturnHomePanel.SetActive(false);


        _settingButton.onClick.RemoveAllListeners();
        _settingButton.onClick.AddListener(OnClickSettingButton);

        _homeButton.onClick.RemoveAllListeners();
        _homeButton.onClick.AddListener(OnClickHomeButton);

        _backButton.onClick.RemoveAllListeners();
        _backButton.onClick.AddListener(OnClickBackButton);

        _returnHomeButton.onClick.RemoveAllListeners();
        _returnHomeButton.onClick.AddListener(GameManager.Instance.LoadTitleScene);

        _cancelButton.onClick.RemoveAllListeners();
        _cancelButton.onClick.AddListener(OnClickCancelButton);
    }

    public override UIState GetUIState()
    {
        return UIState.Pause;
    }

    public void OnClickSettingButton()
    {
        UIManager.Instance.SetUIState(UIState.Setting);
    }

    public void OnClickHomeButton()
    {
        _pausePanel.SetActive(false);
        _ReturnHomePanel.SetActive(true);
    }

    public void OnClickBackButton()
    {
        UIManager.Instance.SetUIState(UIState.None);
    }

    public void OnClickCancelButton()
    {
        _pausePanel.SetActive(true);
        _ReturnHomePanel.SetActive(false);
    }
}
