using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum UIState
{
    None,
    Pause,
    SkillSelect,
    StageClear,
    GameOver,
}

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Panels")]
    private Dictionary<System.Type, Component> typedPanels = new();

    [Header("UI Texts")]
    [SerializeField] private TextMeshProUGUI text; // 예시 텍스트입니다. 삭제하셔도 됩니다.

    private UIState _currentState = UIState.None;

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
    }

    public void SetUIState(UIState state)
    {
        _currentState = state;

        foreach (var panel in typedPanels.Values)
        {
            if (panel is ToggleableUI toggleUI)
            {
                toggleUI.SetActive(state);
            }
        }
    }

    public void RegisterPanel<T>(T panel) where T : Component
    {
        var type = panel.GetType();

        if (!typedPanels.ContainsKey(type))
        {
            typedPanels.Add(type, panel);
            Debug.Log($"[UIManager] Register success {typedPanels[type].name}");
        }
        else
        {
            Debug.LogWarning($"[UIManager] Panel of type {type.Name} already registered.");
        }
    }

    public T GetPanel<T>() where T : Component
    {
        var type = typeof(T);
        if (typedPanels.TryGetValue(type, out var panel))
            return panel as T;

        Debug.LogWarning($"[UIManager] Panel of type {type.Name} not found.");
        return null;
    }
}
