using UnityEngine;

public class UI_Achievement : ToggleableUI
{
    [SerializeField] private GameObject _achievementPanel;
    
    private void Start()
    {
        Init(_achievementPanel);
        _achievementPanel.SetActive(false);
    }

    public override UIState GetUIState()
    {
        return UIState.Achievement;
    }
}
