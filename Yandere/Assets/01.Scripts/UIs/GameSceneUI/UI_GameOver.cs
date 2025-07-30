using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameOver : ToggleableUI
{
    [SerializeField] private GameObject _gameOverPanel;

    [SerializeField] private Button _homeButton;
    [SerializeField] private Button _restartButton;
    
    [SerializeField] private TMP_Text _resultText;
    [SerializeField] private TMP_Text _timeText;
    [SerializeField] private TMP_Text _killText;
    [SerializeField] private TMP_Text _goldText;
    
    private StageManager _stageManager;

    private void Start()
    {
        Init(_gameOverPanel);
        _gameOverPanel.SetActive(false);

        _homeButton.onClick.AddListener(GameManager.Instance.LoadTitleScene);
        _restartButton.onClick.AddListener(GameManager.Instance.LoadGameScene);
        
        _stageManager = StageManager.Instance;
    }
    
    public override void UIAction()
    {
        _resultText.text = $"스테이지 {StageManager.Instance.currentStageData.stageIndex}\n 결과";
        _timeText.text = $"{Mathf.FloorToInt(_stageManager.ElapsedTime/60f):00}:{Mathf.FloorToInt(_stageManager.ElapsedTime%60f):00}";
        _killText.text = $"{_stageManager.KillCount}";
        _goldText.text = $"{_stageManager.GoldCount}";
        
        _gameOverPanel.SetActive(true);
    }

    public override UIState GetUIState()
    {
        return UIState.GameOver;
    }
}
