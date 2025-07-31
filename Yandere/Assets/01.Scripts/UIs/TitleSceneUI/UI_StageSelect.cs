using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_StageSelect : MonoBehaviour
{
    [SerializeField] private GameObject _stageSelectPanel;
    [SerializeField] private TMP_Text _stageName;
    [SerializeField] private TMP_Text _stageDesc;
    [SerializeField] private Image _stageImage;
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _prevButton;
    [SerializeField] private Button _nextButton;
    [SerializeField] private Button _backButton;
    
    private StageData _currentStageData;

    private void Start()
    {
        _startButton.onClick.AddListener(OnClickStartButton);
        _prevButton.onClick.AddListener(OnClickPrevButton);
        _nextButton.onClick.AddListener(OnClickNextButton);
        _backButton.onClick.AddListener(OnClickBackButton);

        UpdateUI();
    }

    private void UpdateUI()
    {
        _currentStageData = GameManager.Instance.currentStageData;
        
        _prevButton.gameObject.SetActive(_currentStageData.stageIndex > 0);
        _nextButton.gameObject.SetActive(_currentStageData.stageIndex < GameManager.Instance.MaxStageIndex - 1);

        _stageName.text = $"스테이지 {_currentStageData.stageIndex} {_currentStageData.stageName}";
        _stageDesc.text = _currentStageData.stageDesc;
        _stageImage.sprite = _currentStageData.stageImage;
    }

    private void OnClickStartButton()
    {
        SoundManager.Instance.Play("LobbyClick01_SFX");
        GameManager.Instance.LoadGameScene();
    }
    
    private void OnClickPrevButton()
    {
        if (_currentStageData.stageIndex < 1) return;
        
        SoundManager.Instance.Play("LobbyClick01_SFX");
        GameManager.Instance.SetStage(GameManager.Instance.stageDatas[_currentStageData.stageIndex - 1]);
        UpdateUI();
    }

    private void OnClickNextButton()
    {
        if (_currentStageData.stageIndex >= GameManager.Instance.MaxStageIndex) return;
        
        SoundManager.Instance.Play("LobbyClick01_SFX");
        GameManager.Instance.SetStage(GameManager.Instance.stageDatas[_currentStageData.stageIndex + 1]);
        UpdateUI();
    }

    private void OnClickBackButton()
    {
        SoundManager.Instance.Play("LobbyClick02_SFX");
        _stageSelectPanel.SetActive(false);
    }
    
    
    
    
    
    
    
    
    //==================================================================================
    /*private StageData[] GetSortedStageDatas()
    {
        StageData[] loadedDatas = GameManager.Instance.stageDatas;
        System.Array.Sort(loadedDatas, (a, b) => a.stageIndex.CompareTo(b.stageIndex));
        Debug.Log($"[UI_StageSelect] Loaded {loadedDatas.Length} StageData assets.");
        return loadedDatas;
    }

    private void InstantiateStageButtons(StageData[] stageDatas)
    {
        int maxStage = GameManager.Instance.MaxStageIndex;

        for (int i = 0; i < maxStage; i++)
        {
            GameObject buttonObj = Instantiate(_stageButtonPrefab, _contentParent);
            Button_Stage stageButton = buttonObj.GetComponent<Button_Stage>();
            stageButton.stageData = stageDatas[i];
            stageButton.SetIndexText(i + 1);
        }
    }*/
}
