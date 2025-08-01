using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_StageClear : ToggleableUI
{
    [SerializeField] private GameObject _stageClearPanel;
    [SerializeField] private Button _homeButton;
    [SerializeField] private Button _advertiseButton;
    [SerializeField] private Button _backButton;
    
    [SerializeField] private TMP_Text _clearText;
    [SerializeField] private TMP_Text _timeText;
    [SerializeField] private TMP_Text _expText;
    [SerializeField] private TMP_Text _killText;
    [SerializeField] private TMP_Text _goldText;
    
    //[Header("Quest UI")]
    [SerializeField] private GameObject[] _starIcons;
    [SerializeField] private QuestPanel[] _questPanels;
    
    private StageManager _stageManager;

    private void Start()
    {
        Init(_stageClearPanel);
        _stageClearPanel.SetActive(false);
        foreach (var icon in _starIcons)
        {
            icon.SetActive(false);
        }

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

        UpdateQuestUI();
        
        _stageClearPanel.SetActive(true);
    }

    private void UpdateQuestUI()
    {
        var currentQuests = QuestManager.Instance.currentQuests;

        for (int i = 0; i < QuestManager.Instance.ReturnClearedQuest(); i++)
        {
            _starIcons[i].SetActive(true);
        }

        for (int i = 0; i < _questPanels.Length; i++)
        {
            _questPanels[i].starIcon.SetActive(currentQuests[i].isCleared);
            _questPanels[i].descriptionText.text = currentQuests[i].description;
            _questPanels[i].progressText.text = $"{currentQuests[i].currentValue} / {currentQuests[i].maxValue}";
            _questPanels[i].progressBar.value = Mathf.Clamp((float)currentQuests[i].currentValue / currentQuests[i].maxValue, 0, 1);
        }
    }

    private void OnClickBackButton()
    {
        GameManager.Instance.LoadTitleScene();
    }
}
