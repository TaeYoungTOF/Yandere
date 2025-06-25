using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Panels")]
    [SerializeField] private GameObject[] uiPanels; // UI들은 딕셔너리로 관리해주세요. Addressable을 적용할 수도 있습니다.
    private Dictionary<string, GameObject> panelDict;

    [Header("UI Texts")]
    [SerializeField] private TextMeshProUGUI text; // 예시 텍스트입니다. 삭제하셔도 됩니다.

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        Init();
    }

    private void Init()
    {
        // UI 이름과 UI를 매핑
        panelDict = new Dictionary<string, GameObject>();
        foreach (var panel in uiPanels)
        {
            if (panel != null)
                panelDict[panel.name] = panel;
        }

    }


    public void OpenPausePanel(string panelName)
    {
        if (panelDict.ContainsKey(panelName))
        {
            panelDict[panelName].SetActive(true);
        }
    }


}
