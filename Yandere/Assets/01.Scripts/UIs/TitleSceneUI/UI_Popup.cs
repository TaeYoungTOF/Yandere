using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Popup : MonoBehaviour
{
    [SerializeField] private GameObject _popUpPanel;
    
    [Header("Upgrade Pop-up")]
    [SerializeField] private GameObject _upgradePanel;
    [SerializeField] private TMP_Text _lvTitleText;
    [SerializeField] private TMP_Text _beforeText;
    [SerializeField] private TMP_Text _afterText;
    [SerializeField] private TMP_Text _descriptionText;
    [SerializeField] private TMP_Text _costText;
    [SerializeField] private Button _confirmButton;
    [SerializeField] private Button _upgradeBackButton;
    
    [Header("Info Pop-up")]
    [SerializeField] private GameObject _infoPanel;
    [SerializeField] private TMP_Text _infoTitleText;
    [SerializeField] private TMP_Text _infoText;
    
    [Header("Alarm Pop-up")]
    [SerializeField] private GameObject _lackResourcePanel;
    [SerializeField] private GameObject _lackAccountLvPanel;
    [SerializeField] private GameObject _fullFacilityLvPanel;

    public void Init()
    {
        _upgradeBackButton.onClick.AddListener(CloseUpgradePanel);
        
        _popUpPanel.SetActive(false);
    }

    public void CallUpgradePanel(Facility facility)
    {
        _lvTitleText.text = facility.FacilityData.statTargetText;
        _beforeText.text = $"{facility.Amount}%";
        _afterText.text = $"{facility.Amount + facility.FacilityData.valuePerLevel}%";
        _descriptionText.text = facility.FacilityData.levelUpTexts[facility.CurrentLevel + 1];
        _costText.text = facility.CurrentCost.ToString();
        
        _confirmButton.onClick.RemoveAllListeners();
        _confirmButton.onClick.AddListener(facility.UpgradeButtonClick);
        
        _popUpPanel.SetActive(true);
        _upgradePanel.SetActive(true);
    }

    public void CloseUpgradePanel()
    {
        _upgradePanel.SetActive(false);
        _popUpPanel.SetActive(false);
    }

    public void CallInfoPanel(string facilityName, string info)
    {
        _infoTitleText.text = facilityName;
        _infoText.text = info;
        _popUpPanel.SetActive(true);
        _infoPanel.SetActive(true);
    }

    public void CallLackResourcePanel()
    {
        _popUpPanel.SetActive(true);
        _lackResourcePanel.SetActive(true);
    }
    
    public void CallLackAccountLvPanel()
    {
        _popUpPanel.SetActive(true);
        _lackAccountLvPanel.SetActive(true);
    }
    
    public void CallFullFacilityLvPanel()
    {
        _popUpPanel.SetActive(true);
        _fullFacilityLvPanel.SetActive(true);
    }
}
