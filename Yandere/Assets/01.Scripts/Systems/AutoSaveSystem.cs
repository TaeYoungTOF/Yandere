using UnityEngine;

public class AutoSaveSystem : MonoBehaviour
{
    public void AutoSave()
    {
        SaveLoadManager.Instance.Save();
        Debug.Log("[AutoSaveSystem] 자동 저장 완료");
    }

    public void SaveOnPauseOrQuit()
    {
        SaveLoadManager.Instance.Save();
        Debug.Log("[AutoSaveSystem] 앱 중단/종료 시 저장");
    }
}
