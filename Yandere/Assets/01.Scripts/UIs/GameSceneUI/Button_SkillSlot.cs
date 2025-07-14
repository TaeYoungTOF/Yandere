using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Button_SkillSlot : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _levelText;
    [SerializeField] private int _level;
    [SerializeField] private Image _bg;
    
    public void SetSkillSlot(Sprite icon, int level)
    {
        _icon.sprite = icon;
        _level = level;

        if (level == SkillManager.Instance.MaxLevel)
        {
            _bg.color = Color.white;
            _levelText.text = "M";
        }
        else
        {
            _levelText.text = level.ToString();
        }
    }
}