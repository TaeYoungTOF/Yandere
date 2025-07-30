using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_StageClear : ToggleableUI
{
    [SerializeField] private GameObject _stageClearPanel;
    [SerializeField] private Button _homeButton;
    [SerializeField] private Button _advetiseButton;
    [SerializeField] private Button _backButton;
    
    [SerializeField] private TMP_Text _clearText;
    [SerializeField] private TMP_Text _timeText;
    [SerializeField] private TMP_Text _expText;
    [SerializeField] private TMP_Text _killText;
    [SerializeField] private TMP_Text _goldText;
    
    private StageManager _stageManager;

    private void Start()
    {
        Init(_stageClearPanel);
        _stageClearPanel.SetActive(false);

        _homeButton.onClick.RemoveAllListeners();
        _backButton.onClick.RemoveAllListeners();
        
        _homeButton.onClick.AddListener(GameManager.Instance.LoadTitleScene);
        _backButton.onClick.AddListener(OnClickBackButton);
        
        _stageManager = StageManager.Instance;
    }

    public override UIState GetUIState()
    {
        return UIState.StageClear;
    }
    
    public override void UIAction()
    {
        _clearText.text = $"스테이지 {StageManager.Instance.currentStageData.stageIndex}\n 클리어";
        _timeText.text = $"{Mathf.FloorToInt(_stageManager.ElapsedTime/60f):00}:{Mathf.FloorToInt(_stageManager.ElapsedTime%60f):00}";
        _expText.text = $"{_stageManager.Exp}";
        _killText.text = $"{_stageManager.KillCount}";
        _goldText.text = $"{_stageManager.GoldCount}";
        
        _stageClearPanel.SetActive(true);
    }

    public void OnClickBackButton()
    {
        GameManager.Instance.LoadTitleScene();
    }
}
