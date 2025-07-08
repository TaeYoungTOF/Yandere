using UnityEngine;
using UnityEngine.UI;

public class UI_Achievement : ToggleableUI
{
    [SerializeField] private GameObject _achievementPanel;
    [SerializeField] private Button _backButton;
    
    private void Start()
    {
        Init(_achievementPanel);
        _achievementPanel.SetActive(false);

        _backButton.onClick.RemoveAllListeners();
        _backButton.onClick.AddListener(OnClickBackButton);
    }

    public override UIState GetUIState()
    {
        return UIState.Achievement;
    }

    private void OnClickBackButton()
    {
        UIManager.Instance.SetUIState(UIState.None);
    }
}
