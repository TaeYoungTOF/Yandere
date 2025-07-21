using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Button_Stage : MonoBehaviour
{
    [SerializeField] private Button _stageButton;
    [SerializeField] private TMP_Text _stageIndexText;
    public StageData stageData;    

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
        SoundManagerTest.Instance.Play("LobbyClick01_SFX");
        GameManager.Instance.SetStage(stageData);
        GameManager.Instance.LoadGameScene();
    }

    public void SetIndexText(int index)
    {
        if (_stageIndexText != null)
        {
            _stageIndexText.text = $"Stage {index}";
        }
    }
}
