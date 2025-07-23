using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Popup : MonoBehaviour
{
    [SerializeField] private GameObject _levelupPanel;
    [SerializeField] private GameObject _infoPanel;
    [SerializeField] private GameObject _lackPanel;

    public void CallLevelupPanel()
    {
        _levelupPanel.SetActive(true);
    }

    public void CallInfoPanel()
    {
        _infoPanel.SetActive(true);
    }

    public void CallLackPanel()
    {
        _lackPanel.SetActive(true);
    }
}
