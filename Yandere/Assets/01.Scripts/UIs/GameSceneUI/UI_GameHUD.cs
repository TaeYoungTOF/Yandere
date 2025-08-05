using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameHUD : MonoBehaviour
{
    [SerializeField] private TMP_Text _goldCountText;
    [SerializeField] private TMP_Text _killCountText;
    [SerializeField] private TMP_Text _timeText;
    [SerializeField] private Image _expImage;
    [SerializeField] private TMP_Text _levelText;
    [SerializeField] private Button _pauseButton;
    [SerializeField] private Button _achievementButton;

    [SerializeField] private Image _healthImage;

    private PlayerStat _stat;

    private void Start()
    {
        UIManager.Instance.RegisterPanel(this);

        _stat = StageManager.Instance.Player.stat;
        
        UpdateTime(0);
        UpdateExpImage();
        UpdateLevel();

        _pauseButton.onClick.AddListener(OnClickPauseButton);
        _achievementButton.onClick.AddListener(OnClickAchievementButton);
    }

    public void UpdateKillCount()
    {
        _killCountText.text = StageManager.Instance.KillCount.ToString();
    }

    public void UpdateGold()
    {
        _goldCountText.text = StageManager.Instance.GoldCount.ToString();
    }

    public void UpdateTime(float time)
    {
        _timeText.text = $"{Mathf.FloorToInt(time/60f):00}:{Mathf.FloorToInt(time%60f):00}";
    }

    public void UpdateExpImage()
    {
        float ratio = _stat.currentExp / _stat.requiredExp;
        _expImage.fillAmount = Mathf.Clamp01(ratio);
    }

    public void UpdateLevel()
    {
        _levelText.text = $"{_stat.level}";
    }

    public void UpdateHealthImage()
    {
        float ratio = _stat.CurrentHp / _stat.FinalHp;
        _healthImage.fillAmount = Mathf.Clamp01(ratio);
    }

    private void OnClickPauseButton()
    {
        UIManager.Instance.SetUIState(UIState.Pause);
        SoundManager.Instance.Play("LobbyClick01_SFX");
    }

    private void OnClickAchievementButton()
    {
        UIManager.Instance.SetUIState(UIState.Achievement);   
        SoundManager.Instance.Play("LobbyClick01_SFX");
    }
}
