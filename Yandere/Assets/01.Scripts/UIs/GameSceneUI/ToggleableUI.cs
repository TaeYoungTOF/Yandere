using UnityEngine;

public abstract class ToggleableUI : MonoBehaviour
{
    [SerializeField] protected GameObject panel;
    
    private UIManager uiManager;
    protected StageManager _stageManager;

    private void Awake()
    {
        uiManager = UIManager.Instance;
        _stageManager = StageManager.Instance;
    }

    protected void Init()
    {

        uiManager.RegisterPanel(this);
    }

    public abstract UIState GetUIState();

    public virtual void UIAction()
    {
        
    }

    public void SetActive(UIState state)
    {
        panel.SetActive(GetUIState() == state);
    }

    protected void OnClickBackButton()
    {
        UIManager.Instance.SetUIState(UIState.None);
        SoundManager.Instance.Play("LobbyClick02_SFX");
    }
}
