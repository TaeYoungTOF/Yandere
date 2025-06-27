using UnityEngine;
using UnityEngine.UI;

public class TY_Debug : MonoBehaviour
{
    [SerializeField] private Button _levelUpButton;
    [SerializeField] private Button _stageClearButton;

    private void Start()
    {
        _levelUpButton.onClick.AddListener(DebugLevelUp);
        _stageClearButton.onClick.AddListener(DebugStageClear);
    }

    public void DebugLevelUp()
    {
        Debug.Log("[Debug] Gain 50 Exp");

        StageManager.Instance.Player.GainExp(50);
    }

    public void DebugStageClear()
    {
        Debug.Log("[Debug] Stag Clear");

        StageManager.Instance.StageClear();
    }

}
