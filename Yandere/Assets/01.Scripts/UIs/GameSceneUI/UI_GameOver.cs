using UnityEngine;

public class UI_GameOver : ToggleableUI
{
    [SerializeField] private GameObject _gameOverPanel;

    private void Start()
    {
        Init();
        _gameOverPanel.SetActive(false);
    }

    protected override UIState GetUIState()
    {
        return UIState.GameOver;
    }

}
