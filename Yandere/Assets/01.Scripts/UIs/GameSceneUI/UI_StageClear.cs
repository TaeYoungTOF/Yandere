using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_StageClear : ToggleableUI
{
    [SerializeField] private GameObject _stageClearPanel;
    [SerializeField] private GameObject _stageSelcetPanel;
    [SerializeField] private Button _homeButton;
    [SerializeField] private Button _nextButton;
    [SerializeField] private Button _backButton;
    [SerializeField] private TMP_Text _clearText;

    private void Start()
    {
        Init(_stageClearPanel);
        _stageClearPanel.SetActive(false);
        Init(_stageSelcetPanel);
        _stageSelcetPanel.SetActive(false);

        _homeButton.onClick.RemoveAllListeners();
        _nextButton.onClick.RemoveAllListeners();
        _backButton.onClick.RemoveAllListeners();
        
        _homeButton.onClick.AddListener(GameManager.Instance.LoadTitleScene);
        _nextButton.onClick.AddListener(GameManager.Instance.LoadNextStage);
        _backButton.onClick.AddListener(OnClickBackButton);
    }

    public override void Show()
    {
        throw new System.NotImplementedException();
    }

    public override void Hide()
    {
        throw new System.NotImplementedException();
    }

    public override UIState GetUIState()
    {
        return UIState.StageClear;
    }
    
    public override void UIAction()
    {
        _clearText.text = $"Stage {StageManager.Instance.currentStageData.stageIndex} Clear !!";

        if (StageManager.Instance.currentStageData.stageIndex == GameManager.Instance.MaxStageIndex)
        {
            _nextButton.gameObject.SetActive(false);
        }
        else
        {
            _nextButton.gameObject.SetActive(true);
        }
    }

    public void OnClickBackButton()
    {
        _stageSelcetPanel.SetActive(true);
    }
}
