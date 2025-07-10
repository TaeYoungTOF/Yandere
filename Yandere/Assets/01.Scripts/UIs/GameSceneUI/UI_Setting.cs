using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.EventSystems;

public class UI_Setting : ToggleableUI
{
    [SerializeField] private GameObject _settingPanel;
    [SerializeField] private Button _backButton;
    [SerializeField] private AudioMixer _bgmMixer;
    [SerializeField] private Slider _bgmSlider;
    [SerializeField] private Slider _sfxSlider;

    void Start()
    {
        Init(_settingPanel);
        _settingPanel.SetActive(false);
        
        _backButton.onClick.RemoveAllListeners();
        _backButton.onClick.AddListener(OnClickReturnButton);
        
        _bgmSlider.onValueChanged.AddListener(delegate { AudioControl(); });
    }
    
    public void OnSliderValueChanged()
    {
        AudioControl();
    }

    public void onPointerUp(PointerEventData eventData)
    {
        OnSliderValueChanged();
    }

    public void onPointerDown(PointerEventData eventData)
    {
        _bgmSlider = eventData.selectedObject.GetComponent<Slider>();
    }

    public override UIState GetUIState()
    {
        return UIState.Setting;
    }

    public void OnClickReturnButton()
    {
        UIManager.Instance.SetUIState(UIState.Pause);
    }
    
    public void AudioControl()
    {
        float sound = _bgmSlider.value;
        
        if (sound == -40f) _bgmMixer.SetFloat("BGM", -80f);
        else _bgmMixer.SetFloat("BGM", sound);
    }
    
}
