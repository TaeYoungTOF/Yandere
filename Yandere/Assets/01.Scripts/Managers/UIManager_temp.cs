using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager_temp : MonoBehaviour
{
    public static UIManager_temp Instance { get; private set; }

    [Header("Panels")]
    [SerializeField] private GameObject _stageSelectPanel;

    [Header("Buttons")]
    [SerializeField] private Button _gameStartButton;

    [Header("UI Texts")]
    [SerializeField] private TextMeshProUGUI text; // 예시 텍스트입니다. 삭제하셔도 됩니다.

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        _gameStartButton.onClick.RemoveAllListeners();
        _gameStartButton.onClick.AddListener(OnClickStartButton);

        _stageSelectPanel.SetActive(false);
    }

    public void OnClickStartButton()
    {
        _stageSelectPanel.SetActive(true);
    }
}
