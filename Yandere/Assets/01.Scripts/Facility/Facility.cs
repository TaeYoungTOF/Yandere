using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Facility : MonoBehaviour
{
    [SerializeField] private int _index;
    [SerializeField] protected FacilityData facilityData;
    public FacilityData FacilityData => facilityData;
    [SerializeField] private Button _infoButton;
    [SerializeField] private Button _levelUpButton;
    
    [Header("시설 인포")]
    [SerializeField] protected int currentLevel;
    public  int CurrentLevel => currentLevel;
    [SerializeField] protected float currentCost;
    public float CurrentCost => currentCost;
    
    [Header("UI 표시")]
    [SerializeField] protected TextMeshProUGUI facilityNameText;
    [SerializeField] protected TextMeshProUGUI levelText;
    [SerializeField] protected TextMeshProUGUI levelDescriptionText;

    [Header("Lock")]
    [SerializeField] private GameObject _lockGO;
    [SerializeField] private TMP_Text _requireLevel;
    [SerializeField] private Button _unlockButton;
    
    [SerializeField] protected float amount;
    public float Amount => amount;
    
    void Start()
    {
        Init();
        UpdateUI();
    }
    
    protected virtual void Init()
    {
        currentLevel = 0;
        currentCost = facilityData.baseCost;
        facilityNameText.text = facilityData.facilityName;
        amount = facilityData.basevalue;
        _infoButton.onClick.AddListener(OnClickInfoButton);
        _levelUpButton.onClick.AddListener(CallLevelUpPanel);

        if (_lockGO)
        {
            _lockGO.SetActive(true);
            _requireLevel.text = facilityData.requiredAccountLevel.ToString();
            _unlockButton.onClick.AddListener(OnClickUnLockButton);
        }
    }

    private void CallLevelUpPanel()
    {
        if (currentLevel >= facilityData.maxLevel)
        {
            SoundManagerTest.Instance.Play("LobbyClick02_SFX");
            UIManager_Title.Instance.popUp.CallFullFacilityLvPanel();
            return;
        }

        UIManager_Title.Instance.popUp.CallUpgradePanel(this);
    }
    
    public void UpgradeButtonClick()
    {
        if (DataManager.Instance.obsessionCrystals < currentCost)
        {
            SoundManagerTest.Instance.Play("LobbyClick02_SFX");
            UIManager_Title.Instance.popUp.CallLackResourcePanel();
            return;
        }
        
        SoundManagerTest.Instance.Play("LobbyClick01_SFX");
        
        ResourceManager.Instance.UseObsessionCrystals(currentCost);
        currentLevel++;
        currentCost = Mathf.FloorToInt(currentCost * facilityData.costMultiplier);
        amount += facilityData.valuePerLevel;
        
        UIManager_Title.Instance.popUp.CloseUpgradePanel();
        UpdateUI();
    }

    protected virtual void UpdateUI()
    {
        levelText.text = $"Lv.{currentLevel.ToString()}";
        levelDescriptionText.text = $"{facilityData.statTargetText} + {amount}%";
        
        DataManager.Instance.SetData(_index, currentLevel, amount);
    }

    private void OnClickUnLockButton()
    {
        if (facilityData.requiredAccountLevel > DataManager.Instance.accountLevel)
        {
            UIManager_Title.Instance.popUp.CallLackAccountLvPanel();
        }
        else
        {
            _lockGO.SetActive(false);
        }
    }

    private void OnClickInfoButton()
    {
        UIManager_Title.Instance.popUp.CallInfoPanel(facilityData.facilityName, facilityData.description);
    }
}
