using UnityEngine;

public class UI_StageSelect : MonoBehaviour
{
    [SerializeField] private GameObject _stageButtonPrefab;
    [SerializeField] private Transform _contentParent;
    

    private void Start()
    {
        StageData[] sortedStageDatas = GetSortedStageDatas();
        InstantiateStageButtons(sortedStageDatas);
    }

    private StageData[] GetSortedStageDatas()
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
    }
}
