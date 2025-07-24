using UnityEngine;
using TMPro;


public class Facility : MonoBehaviour
{
    [SerializeField] private int _index;
    [SerializeField] protected FacilityData facilityData;
    
    
    [Header("시설 인포")]
    [SerializeField] protected int currentLevel;
    [SerializeField] protected float currentCost;
    
    [Header("UI 표시")]
    [SerializeField] protected TextMeshProUGUI facilityNameText;
    [SerializeField] protected TextMeshProUGUI levelText;
    [SerializeField] protected TextMeshProUGUI levelDescriptionText;

    [Header("Lock")]
    [SerializeField] private GameObject _lockGO;
    [SerializeField] private TMP_Text _requireLevel;
    
    [SerializeField] protected float amount;
    
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

        if (_lockGO)
        {
            _lockGO.SetActive(true);
            _requireLevel.text = facilityData.requiredAccountLevel.ToString();
        }
    }
    
    public void UpgradeButtonClick()
    {
        if (currentLevel >= facilityData.maxLevel)
        {
            SoundManagerTest.Instance.Play("LobbyClick02_SFX");
            Debug.Log("[시설] 업그레이드 불가! 시설이 최대 레벨입니다!");
            return;
        }

        if (DataManager.Instance.obsessionCrystals < currentCost)
        {
            SoundManagerTest.Instance.Play("LobbyClick02_SFX");
            Debug.Log("[시설] 업그레이드 불가! 재화가 부족합니다!");
            return;
        }
        
        SoundManagerTest.Instance.Play("LobbyClick01_SFX");
        DataManager.Instance.obsessionCrystals -= currentCost;
        currentLevel++;
        currentCost = Mathf.FloorToInt(currentCost * facilityData.costMultiplier);
        amount += facilityData.valuePerLevel;
        
        UpdateUI();
        UIManager_Title.Instance.UpdateUI();
    }

    protected virtual void UpdateUI()
    {
        levelText.text = $"Lv.{currentLevel.ToString()}";
        levelDescriptionText.text = $"{facilityData.statTargetText} + {amount}%";
        
        DataManager.Instance.SetData(_index, currentLevel, amount);
    }

    public void OnClickUnLockButton()
    {
        if (facilityData.requiredAccountLevel > DataManager.Instance.accountLevel)
        {
            Debug.Log("[시설] 레벨이 부족합니다");
        }
        else
        {
            _lockGO.SetActive(false);
        }
    }
}
