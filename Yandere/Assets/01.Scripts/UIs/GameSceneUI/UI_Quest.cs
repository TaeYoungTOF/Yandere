using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class QuestPanel
{
    public GameObject starIcon;          // 완료 시 활성화될 별 아이콘
    public TMP_Text descriptionText;     // 업적 설명 텍스트
    public TMP_Text progressText;        // 진행률 텍스트 (예: "5/10")
    public Slider progressBar;           // 진행률 슬라이더 바
}

public class UI_Quest : ToggleableUI
{
    [Header("Quest UI")]
    [SerializeField] private GameObject _QuestPanel;
    [SerializeField] private Button _backButton;
    
    [Header("Quest UI ItemList")]
    [SerializeField] private List<QuestPanel> _panels;

    private void Start()
    {
        Init(_QuestPanel);
        _QuestPanel.SetActive(false);
        
        _backButton.onClick.AddListener(() => UIManager.Instance.SetUIState(UIState.None));

        for (int i = 0; i < _panels.Count; i++)
        {
            var quest = QuestManager.Instance.currentQuests[i];
            
            InitPanel(_panels[i], quest.description, quest.maxValue);
        }
    }

    public override UIState GetUIState()
    {
        return UIState.Achievement;
    }

    public override void UIAction()
    {
        QuestManager.Instance.UpdateValue();

        for (int i = 0; i < _panels.Count; i++)
        {
            var quest = QuestManager.Instance.currentQuests[i];
            
            _panels[i].starIcon.SetActive(quest.isCleared);
            SetPanel(_panels[i], quest.currentValue, quest.maxValue);
        }
    }

    private void InitPanel(QuestPanel questPanel, string description, int maxValue)
    {
        questPanel.starIcon.SetActive(false);
        questPanel.descriptionText.text = description;
        questPanel.progressText.text = $"0 / {maxValue}";
        questPanel.progressBar.value = 0;
    }

    private void SetPanel(QuestPanel questPanel, int curValue, int maxValue)
    {
        questPanel.progressText.text = $"{curValue} / {maxValue}";
        questPanel.progressBar.value = Mathf.Clamp((float)curValue / maxValue, 0, 1);
    }
}
