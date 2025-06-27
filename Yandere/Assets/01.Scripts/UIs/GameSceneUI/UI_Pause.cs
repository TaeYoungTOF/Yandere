using UnityEngine;

public class UI_Pause : ToggleableUI
{
    [SerializeField] private GameObject _pausePanel;

    private void Start()
    {
        Init();
        _pausePanel.SetActive(false);
    }

    protected override UIState GetUIState()
    {
        return UIState.Pause;
    }


}
