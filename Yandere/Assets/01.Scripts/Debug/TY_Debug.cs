using UnityEngine;
using UnityEngine.UI;

public class TY_Debug : MonoBehaviour
{
    [SerializeField] private Button _levelUpButton;
    [SerializeField] private Button _stageClearButton;
    [SerializeField] private Button _loseHealthButton;

    private void Start()
    {
        _levelUpButton.onClick.RemoveAllListeners();
        _levelUpButton.onClick.AddListener(DebugLevelUp);

        _stageClearButton.onClick.RemoveAllListeners();
        _stageClearButton.onClick.AddListener(DebugStageClear);

        _loseHealthButton.onClick.RemoveAllListeners();
        _loseHealthButton.onClick.AddListener(DebugLoseHealth);
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

    public void DebugLoseHealth()
    {
        Debug.Log("[Debug] Lose 10 Health");
        
        StageManager.Instance.Player.TakeDamage(15);
    }

}
