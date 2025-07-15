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
    [SerializeField] private Toggle _bgmMute;
    [SerializeField] private Toggle _sfxMute;
    
    // 이전 음량값 저장
    private float _prebgmValue;
    
    private void Awake()
    {
        // 슬라이더의 값이 변경될 때 AddListener를 통해 값을 저장
        _bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        // 토클의 값이 변경될 때 AddListener를 통해 값을 저장
        _bgmMute.onValueChanged.AddListener(SetBGMMute);
    }
    void Start()
    {
        Init(_settingPanel);
        _settingPanel.SetActive(false);
        
        _backButton.onClick.RemoveAllListeners();
        _backButton.onClick.AddListener(OnClickReturnButton);
        
        _bgmSlider = _bgmSlider.GetComponent<Slider>();
        
    }
    
    // 아이콘 클릭시 음소거 버튼 구현
    public void OnMuteClick(bool isOn)
    {
        if (isOn)
        {
            // 음소거 되기 전 값 저장
            _prebgmValue = _bgmSlider.value;
            
            _bgmSlider.value = 0f;
        }
        else
        {
            // 이전 음량값이 0이 아닐 때만 음소거 취소시 이전에 저장된 값으로 변경
            if (_prebgmValue != 0)
            _bgmSlider.value = _prebgmValue;
        }
    }
    
    public override UIState GetUIState()
    {
        return UIState.Setting;
    }

    public void OnClickReturnButton()
    {
        UIManager.Instance.SetUIState(UIState.Pause);
    }
    
    // Slider를 통해 걸어놓은 이벤트
    public void SetBGMVolume(float volume)
    {
        // 변경된 Slider의 값 volume으로 audioMixer의 Volume 변경하기
        _bgmMixer.SetFloat("BGM", Mathf.Log10(volume) * 20);
        // 변경된 Volume 값 저장하기
        PlayerPrefs.SetFloat("Volume", _bgmSlider.value);
    }

    private void SetBGMMute(bool mute)
    {
        // AudioListener : Audio를 듣는 객체. 보통 카메라에 달려있다.
        AudioListener.volume = (mute ? 0 : 1);
    }
    
}
