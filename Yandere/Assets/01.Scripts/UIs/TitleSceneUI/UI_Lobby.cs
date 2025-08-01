using UnityEngine;
using UnityEngine.UI;

public class UI_Lobby : MonoBehaviour
{
    [SerializeField] private GameObject _stageSelectPanel;
    [SerializeField] private Button _stageSelectButton;

    private void Start()
    {
        _stageSelectButton.onClick.AddListener(OnClickStageSelectButton);
    }

    private void OnClickStageSelectButton()
    {
        SoundManager.Instance.Play("Lobby_StageSelect_Click01SFX");
        _stageSelectPanel.SetActive(true);
    }
}
