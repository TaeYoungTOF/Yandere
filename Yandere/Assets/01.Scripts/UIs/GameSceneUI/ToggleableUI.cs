using System;
using UnityEngine;

public abstract class ToggleableUI : MonoBehaviour
{
    protected UIManager uiManager;
    protected GameObject panel;

    public virtual void Init(GameObject panel)
    {
        uiManager = UIManager.Instance;

        uiManager.RegisterPanel(this);

        this.panel = panel;
    }

    public abstract UIState GetUIState();

    public virtual void UIAction()
    {
        
    }

    public void SetActive(UIState state)
    {
        panel.SetActive(GetUIState() == state);
    }
}
