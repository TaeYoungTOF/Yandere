using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Button_Stage : MonoBehaviour
{
    [SerializeField] private Button _stageButton;
    [SerializeField] private TMP_Text _stageIndexText;
    public int stageIndex;
    public StageData stageData;

    private GameObject _stageSelectPanel;
    

    private void Awake()
    {
        _stageButton = GetComponent<Button>();
    }

    private void Start()
    {
        _stageButton.onClick.AddListener(LoadStage);
    }

    public void LoadStage()
    {
        GameManager.Instance.currentStageIndex = stageIndex;

        StageManager.Instance.currnetStageData = stageData;

        GameManager.Instance.SetStage();
        _stageSelectPanel.SetActive(false);
    }

    public void SetIndexText(int index)
    {
        if (_stageIndexText != null)
        {
            _stageIndexText.text = $"Stage {index}";
        }
    }

    public void SetStageSelectPanel(GameObject panel)
    {
        _stageSelectPanel = panel;
    }
}
