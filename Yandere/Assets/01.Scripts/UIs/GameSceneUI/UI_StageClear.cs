using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_StageClear : ToggleableUI
{
    [SerializeField] private GameObject _stageClearPanel;
    [SerializeField] private Button _homeButton;
    [SerializeField] private Button _nextButton;
    [SerializeField] private TMP_Text _clearText;

    private void Start()
    {
        Init();
        _stageClearPanel.SetActive(false);
    }

    protected override UIState GetUIState()
    {
        return UIState.StageClear;
    }

    public void CallStageClearUI()
    {
        _stageClearPanel.SetActive(true);

        _homeButton.onClick.RemoveAllListeners();
        _nextButton.onClick.RemoveAllListeners();
        _homeButton.onClick.AddListener(LoadTitleScene);
        _nextButton.onClick.AddListener(LoadNextStage);

        _clearText.text = $"Stage {GameManager.Instance.currentStageData.stageIndex} Clear!!";
    }

    private void LoadTitleScene()
    {
        GameManager.Instance.LoadTitleScene();

        _stageClearPanel.SetActive(false);
    }

    private void LoadNextStage()
    {
        GameManager.Instance.LoadNextStage();

        _stageClearPanel.SetActive(false);

    }
}
