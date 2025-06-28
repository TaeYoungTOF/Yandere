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

    private PlayerStat _stat;
    private int _gold = 0;
    private int _killCount = 0;

    private void Start()
    {
        UIManager.Instance.RegisterPanel(this);

        _stat = StageManager.Instance.Player.stat;

        UpdateGold(0);
        UpdateKillCount(0);
        UpdateTime(0, 0);
        UpdateExpImage();
        UpdateLevel();

        _pauseButton.onClick.RemoveAllListeners();
        _pauseButton.onClick.AddListener(OnClickPauseBUtton);
    }

    public void UpdateGold(int amount)
    {
        _gold = _gold + amount;
        _goldCountText.text = _gold.ToString();
        Debug.Log("[GameHUD UI] Increase gold");
    }

    public void UpdateKillCount(int amount)
    {
        _killCount = _killCount + amount;
        _killCountText.text = _killCount.ToString();
        Debug.Log("[GameHUD UI] Increase kill count");
    }

    public void UpdateTime(int minutes, int seconds)
    {
        _timeText.text = $"{minutes:00}:{seconds:00}";
    }

    public void UpdateExpImage()
    {
        float ratio = _stat.currentExp / _stat.requiredExp;
        _expImage.fillAmount = Mathf.Clamp01(ratio);
    }

    public void UpdateLevel()
    {
        _levelText.text = $"LV. {_stat.level}";
    }

    public void OnClickPauseBUtton()
    {
        UIManager.Instance.SetUIState(UIState.Pause);
    }
}
