using UnityEngine;

public abstract class ToggleableUI : MonoBehaviour, IBaseUI
{
    protected UIManager uiManager;

    public void Init()
    {
        uiManager = UIManager.Instance;

        uiManager.RegisterPanel(this);
        gameObject.SetActive(false);
    }

    protected abstract UIState GetUIState();

    public void SetActive(UIState state)
    {
        gameObject.SetActive(GetUIState() == state);
    }
}
