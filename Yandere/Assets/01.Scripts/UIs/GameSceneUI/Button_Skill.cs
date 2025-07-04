using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Button_Skill : MonoBehaviour
{
    public Image icon;
    public TMP_Text nameText;
    public TMP_Text descText;
    private BaseSkill skill;

    public void Setup(BaseSkill skillData)
    {
        skill = skillData;
        icon.sprite = skillData.skillIcon;
        nameText.text = skillData.skillName;
        descText.text = skillData.description;
    }

    public void OnClick()
    {
        SkillManager.Instance.EquipSkill(skill);

        UIManager.Instance.SetUIState(UIState.None);
        UIManager.Instance.GetPanel<UI_GameHUD>().UpdateExpImage();
    }
}
