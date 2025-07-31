using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Button_Skill : MonoBehaviour
{
    [SerializeField] private Button _button;
    
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _dialogueText;
    [SerializeField] private TMP_Text _descText;
    [SerializeField] private TMP_Text _levelText;
    [SerializeField] private BaseSkill _skill;

    public void Setup(BaseSkill skill)
    {
        _skill = skill;

        _icon.sprite = _skill.nextLevelData.skillIcon;
        _nameText.text = _skill.nextLevelData.skillName;
        _dialogueText.text = _skill.nextLevelData.dialogue;
        _descText.text = _skill.nextLevelData.levelupTooltip;
        _levelText.text = $"LV. {_skill.nextLevelData.level}";
        
        _button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        SoundManager.Instance.Play("LobbyClick01_SFX");
        
        _skill.LevelUp();

        UIManager.Instance.SetUIState(UIState.None);
        UIManager.Instance.GetPanel<UI_GameHUD>().UpdateExpImage();

        UIManager.Instance.GetPanel<UI_SkillSelect>().DestroyButtons();
    }
}
