 using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager_temp : MonoBehaviour
{
    public static UIManager_temp Instance { get; private set; }

    [Header("Panels")]
    [SerializeField] private GameObject _stageSelectPanel;
    [SerializeField] private GameObject _settingPanel;

    [Header("Buttons")]
    [SerializeField] private Button _gameStartButton;
    [SerializeField] private Button _SettingButton;

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
        
        _SettingButton.onClick.RemoveAllListeners();
        _SettingButton.onClick.AddListener(OnClickSettingButton);

        _stageSelectPanel.SetActive(false);
    }

    public void OnClickStartButton()
    {
        SoundManagerTest.Instance.Play("LobbyClick01_SFX");
        _stageSelectPanel.SetActive(true);
    }

    public void OnClickSettingButton()
    {
        SoundManagerTest.Instance.Play("LobbyClick01_SFX");
        _settingPanel.SetActive(true);
    }
}
