using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_StageClear : ToggleableUI
{
    [SerializeField] private GameObject _stageClearPanel;
    [SerializeField] private Button _homeButton;
    [SerializeField] private Button _backButton;
    [SerializeField] private TMP_Text _clearText;

    private void Start()
    {
        Init(_stageClearPanel);
        _stageClearPanel.SetActive(false);

        _homeButton.onClick.RemoveAllListeners();
        _backButton.onClick.RemoveAllListeners();
        
        _homeButton.onClick.AddListener(GameManager.Instance.LoadTitleScene);
        _backButton.onClick.AddListener(OnClickBackButton);
    }

    public override UIState GetUIState()
    {
        return UIState.StageClear;
    }
    
    public override void UIAction()
    {
        _clearText.text = $"Stage {StageManager.Instance.currentStageData.stageIndex} Clear !!";
        
        _stageClearPanel.SetActive(true);

        /*if (StageManager.Instance.currentStageData.stageIndex == GameManager.Instance.MaxStageIndex)
        {
            _nextButton.gameObject.SetActive(false);
        }
        else
        {
            _nextButton.gameObject.SetActive(true);
        }*/
    }

    public void OnClickBackButton()
    {
        GameManager.Instance.LoadTitleScene();
    }
}
