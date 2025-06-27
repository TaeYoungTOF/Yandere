using UnityEngine;

public abstract class ToggleableUI : MonoBehaviour
{
    protected UIManager uiManager;

    public virtual void Init()
    {
        uiManager = UIManager.Instance;

        uiManager.RegisterPanel(this);
    }

    protected abstract UIState GetUIState();

    public void SetActive(UIState state)
    {
        gameObject.SetActive(GetUIState() == state);
    }
}
