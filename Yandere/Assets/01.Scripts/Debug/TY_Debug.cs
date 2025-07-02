using NaughtyAttributes;
using UnityEngine;

public class TY_Debug : MonoBehaviour
{
    [Button]
    public void DebugLevelUp()
    {
        Debug.Log("[Debug] Gain 50 Exp");

        StageManager.Instance.Player.GainExp(50);
    }

    [Button]
    public void DebugStageClear()
    {
        Debug.Log("[Debug] Stag Clear");

        StageManager.Instance.StageClear();
    }

    [Button]
    public void DebugHealHealth()
    {
        Debug.Log("[Debug] Heal 100 Health");
        
        StageManager.Instance.Player.Heal(100);
    }

}
