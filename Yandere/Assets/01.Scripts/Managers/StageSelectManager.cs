using System.Collections.Generic;
using UnityEngine;

public class StageSelectManager : MonoBehaviour
{
    [SerializeField] private GameObject _stageButtonPrefab;
    [SerializeField] private Transform _contentParent;
    [SerializeField] private List<StageData> _stageDatas;
    

    private void Start()
    {
        LoadAllStageData();
        InstantiateStageButtons();
    }

    private void LoadAllStageData()
    {
        _stageDatas.Clear();
        StageData[] loadedDatas = GameManager.Instance.stageDatas;

        // 선택적으로 정렬 (stageIndex 순으로)
        System.Array.Sort(loadedDatas, (a, b) => a.stageIndex.CompareTo(b.stageIndex));

        _stageDatas.AddRange(loadedDatas);
        Debug.Log($"[StageSelectManager] Loaded {_stageDatas.Count} StageData assets.");
    }

    private void InstantiateStageButtons()
    {
        int maxStage = GameManager.Instance.MaxStageIndex;

        for (int i = 0; i < maxStage; i++)
        {
            GameObject buttonObj = Instantiate(_stageButtonPrefab, _contentParent);
            Button_Stage stageButton = buttonObj.GetComponent<Button_Stage>();
            stageButton.stageIndex = i + 1;
            stageButton.stageData = _stageDatas[i];
            stageButton.SetIndexText(i + 1); // 텍스트 설정
            stageButton.SetStageSelectPanel(gameObject); // 패널 참조 전달
        }
    }
}
