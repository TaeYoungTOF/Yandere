using UnityEngine;
using UnityEngine.UI;

public class TY_Debug : MonoBehaviour
{
    [SerializeField] private Button _levelUpButton;
    [SerializeField] private Button _stageClearButton;
    [SerializeField] private GameObject _skillSelectPanel;

    private void Start()
    {
        _levelUpButton.onClick.AddListener(DebugLevelUp);
        _stageClearButton.onClick.AddListener(DebugStageClear);
    }

    public void DebugLevelUp()
    {
        Debug.Log("[Debug] Level Up");

        StageManager.Instance.PlayerLevelUp();

        _skillSelectPanel.SetActive(true);
    }

    public void DebugStageClear()
    {
        Debug.Log("[Debug] Stag Clear");

        StageManager.Instance.StageClear();
    }

}
