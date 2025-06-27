using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_StageClear : MonoBehaviour
{
    [SerializeField] private GameObject StageClearPanel;
    [SerializeField] private Button _homeButton;
    [SerializeField] private Button _nextButton;
    [SerializeField] private TMP_Text _clearText;

    public void CallStageClearUI()
    {
        StageClearPanel.SetActive(true);

        _homeButton.onClick.RemoveAllListeners();
        _nextButton.onClick.RemoveAllListeners();
        _homeButton.onClick.AddListener(LoadTitleScene);
        _nextButton.onClick.AddListener(LoadNextStage);

        _clearText.text = $"Stage {GameManager.Instance.currentStageData.stageIndex} Clear!!";
    }

    private void LoadTitleScene()
    {
        GameManager.Instance.LoadTitleScene();

        StageClearPanel.SetActive(false);
    }

    private void LoadNextStage()
    {
        GameManager.Instance.LoadNextStage();

        StageClearPanel.SetActive(false);

    }
}
