using NaughtyAttributes;
using UnityEngine;

public class TY_Debug : MonoBehaviour
{
    [Button]
    public void DebugLevelUp()
    {
        Debug.Log("[Debug] Level Up");

        StageManager.Instance.Player.LevelUp();
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

    [Button]
    public void AddTimeScale()
    {
        StageManager.Instance.timeScale += 1;
    }

    [Button]
    public void ReduceTimeScale()
    {
        StageManager.Instance.timeScale -= 1;
    }



}
