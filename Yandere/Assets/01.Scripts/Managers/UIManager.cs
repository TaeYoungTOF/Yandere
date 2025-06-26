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


    public void OpenPausePanel()
    {
        panelDict["InGame_Panel_Pause"].SetActive(true);
    }

    public void OpenDPSPanel()
    {
        panelDict["InGame_Panel_DPS"].SetActive(true);
    }
    public void OpenBackLobbyPanel()
    {
        panelDict["InGame_Panel_BackLobby"].SetActive(true);
    }

    public void OpenSettingPanel()
    {
        panelDict["InGame_Panel_Setting"].SetActive(true);
    }


    public void InGameUiCloseButton(int index)
    {
        switch (index)
        {
            case 1 :
                panelDict["InGame_Panel_Pause"].SetActive(false);
                break;
            case 2 :
                panelDict["InGame_Panel_BackLobby"].SetActive(false);
                Debug.Log("아직 미구현 입니다");
                break;
            case 3 :
                panelDict["InGame_Panel_BackLobby"].SetActive(false);
                break;
            case 4 :
                panelDict["InGame_Panel_Setting"].SetActive(false);
                break;
            
        }
    }


}
