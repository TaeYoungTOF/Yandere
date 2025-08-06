using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_MainHUD : MonoBehaviour
{
    [SerializeField] TMP_Text _accountlevelText;
    [SerializeField] TMP_Text _expText;
    [SerializeField] Slider _expSlider;
    [SerializeField] TMP_Text _obsessionCrystalsText;
    [SerializeField] TMP_Text _premiumCurrencyText;
    
    [SerializeField] Button[] _selectButton =  new Button[5];
    [SerializeField] GameObject[] _panel = new GameObject[5];
    [SerializeField] GameObject[] _selected = new GameObject[5];
    
    private DataManager _dataManager;

    [SerializeField] private Button _settingButton;

    private void Start()
    {
        _dataManager = DataManager.Instance;
        UpdateUI();

        // 버튼에 클릭 이벤트 연결
        for (int i = 0; i < _selectButton.Length; i++)
        {
            int idx = i; // 클로저 문제 해결용
            _selectButton[i].onClick.AddListener(() => OnSelectButtonClicked(idx));
        }
        
        OnSelectButtonClicked(2);
        
        _settingButton.onClick.AddListener(()=> SoundManager.Instance.OpenSettingPanel());
    }

    public void UpdateUI()
    {
        _accountlevelText.text = _dataManager.accountLevel.ToString();
        _expText.text = $"{_dataManager.currentExp:F0} / { _dataManager.RequiredExp:F0}";
        _expSlider.value = _dataManager.currentExp /  _dataManager.RequiredExp;
        
        _obsessionCrystalsText.text = _dataManager.obsessionCrystals.ToString();
        _premiumCurrencyText.text = _dataManager.premiumCurrency.ToString();
    }

    private void OnSelectButtonClicked(int index)
    {
        SoundManager.Instance.Play("LobbyClick01_SFX");

        if (index == 1 || index == 3 || index == 4)
        {
            UIManager_Title.Instance.CallPreparingPopUp();
            return;
        }
        
        for (int i = 0; i < _selected.Length; i++)
        {
            _panel[i].SetActive(i == index);
            _selected[i].SetActive(i == index);
        }

        UpdateUI();
    }
}
