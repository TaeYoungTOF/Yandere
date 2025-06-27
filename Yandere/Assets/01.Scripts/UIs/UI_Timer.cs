using TMPro;
using UnityEngine;

public class UI_Timer : MonoBehaviour
{
    [SerializeField] private TMP_Text _timeText;

    public void UpdateTime(int minutes, int seconds)
    {
        _timeText.text = $"{minutes:00}:{seconds:00}";
    }
}
