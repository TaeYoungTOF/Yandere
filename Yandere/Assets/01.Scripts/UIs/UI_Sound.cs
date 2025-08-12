using UnityEngine;
using UnityEngine.UI;

public class UI_Sound : MonoBehaviour
{
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    private void OnEnable()
    {
        // ✅ 현재 사운드 값을 UI에만 반영 (콜백 미발생)
        masterSlider.SetValueWithoutNotify(SoundManager.MasterVol);
        bgmSlider.SetValueWithoutNotify(SoundManager.BgmVol);
        sfxSlider.SetValueWithoutNotify(SoundManager.SfxVol);
    }

    // ✅ 아래 메서드를 슬라이더 OnValueChanged에 연결
    public void OnMasterChanged(float v) => SoundManager.Instance.SetMasterVolume(v);
    public void OnBgmChanged(float v)    => SoundManager.Instance.SetBGMVolume(v);
    public void OnSfxChanged(float v)    => SoundManager.Instance.SetSFXVolume(v);
}
