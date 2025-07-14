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
        SceneManager.LoadScene("TitleScene");
        UIManager.Instance.SetUIState(UIState.Lobby);
        
    }

    public void OnClickBackButton()
    {
        UIManager.Instance.SetUIState(UIState.None);
    }

    public void OnClickCancelButton()
    {
        _pausePanel.SetActive(true);
    }
}
