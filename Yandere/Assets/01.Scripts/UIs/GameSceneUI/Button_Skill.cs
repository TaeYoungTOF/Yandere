using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum DataType
{
    Active,
    Passive,
    Upgrade,
}

public class Button_Skill : MonoBehaviour
{
    [SerializeField] private Button _button;
    
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _dialogueText;
    [SerializeField] private TMP_Text _descText;
    [SerializeField] private TMP_Text _levelText;
    [SerializeField] private BaseSkill _skill;

    [SerializeField] private GameObject[] _visual;

    public void Setup(BaseSkill skill)
    {
        _skill = skill;

        _icon.sprite = _skill.nextLevelData.skillIcon;
        _nameText.text = _skill.nextLevelData.skillName;
        _dialogueText.text = _skill.nextLevelData.dialogue;
        _descText.text = _skill.nextLevelData.levelupTooltip;
        _levelText.text = $"LV. {_skill.nextLevelData.level}";
        
        _button.onClick.AddListener(OnClick);
        
        DataType type = DataType.Active;
        switch (_skill)
        {
            case PassiveSkill:
                type = DataType.Passive;
                break;
            case UpgradeSkill:
                type = DataType.Upgrade;
                break;
        }

        _visual[(int)type].SetActive(true);
    }

    private void OnClick()
    {
        SoundManager.Instance.Play("LobbyClick01_SFX");
        
        _skill.LevelUp();

        UIManager.Instance.SetUIState(UIState.None);
        UIManager.Instance.gameHUD.UpdateExpImage();

        UIManager.Instance.GetPanel<UI_SkillSelect>().DestroyButtons();
    }
}
