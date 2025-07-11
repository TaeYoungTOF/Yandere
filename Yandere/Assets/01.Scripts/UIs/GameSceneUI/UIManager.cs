using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum UIState
{
    None,
    Pause,
    Lobby,
    Setting,
    SkillSelect,
    StageClear,
    GameOver,
    Achievement,
}

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Panels")]
    [SerializeField] private GameObject _joyStick;
    private Dictionary<System.Type, Component> _typedPanels = new();

    [SerializeField] private UIState _currentState;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

        SetUIState(UIState.None);
    }

    public void SetUIState(UIState state)
    {
        _currentState = state;

        StageManager.Instance.IsUIOpened = _currentState != UIState.None;
        _joyStick.SetActive(_currentState == UIState.None);

        foreach (var panel in _typedPanels.Values)
        {
            if (panel is ToggleableUI toggleUI)
            {
                toggleUI.SetActive(state);

                if (toggleUI.GetUIState() == _currentState)
                {
                    toggleUI.UIAction();
                }
            }
        }
    }

    public void RegisterPanel<T>(T panel) where T : Component
    {
        var type = panel.GetType();

        if (!_typedPanels.ContainsKey(type))
        {
            _typedPanels.Add(type, panel);
            //Debug.Log($"[UIManager] Register success {typedPanels[type].name}");
        }
        else
        {
            Debug.LogWarning($"[UIManager] Panel of type {type.Name} already registered.");
        }
    }

    public T GetPanel<T>() where T : Component
    {
        var type = typeof(T);
        if (_typedPanels.TryGetValue(type, out var panel))
            return panel as T;

        Debug.LogWarning($"[UIManager] Panel of type {type.Name} not found.");
        return null;
    }
}
