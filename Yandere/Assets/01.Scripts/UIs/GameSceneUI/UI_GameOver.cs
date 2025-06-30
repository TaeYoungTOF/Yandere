using UnityEngine;
using UnityEngine.UI;

public class UI_GameOver : ToggleableUI
{
    [SerializeField] private GameObject _gameOverPanel;

    [SerializeField] private Button _homeButton;
    [SerializeField] private Button _restartButton;

    private void Start()
    {
        Init(_gameOverPanel);
        _gameOverPanel.SetActive(false);

        _homeButton.onClick.RemoveAllListeners();
        _homeButton.onClick.AddListener(GameManager.Instance.LoadTitleScene);

        _restartButton.onClick.RemoveAllListeners();
        _restartButton.onClick.AddListener(GameManager.Instance.LoadGameScene);
    }

    public override UIState GetUIState()
    {
        return UIState.GameOver;
    }
}
