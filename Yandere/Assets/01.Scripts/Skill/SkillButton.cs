using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
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
        SkillManager manager = FindObjectOfType<SkillManager>();
        if (!manager.equippedSkills.Contains(skill))
            manager.EquipSkill(skill);
        else
            skill.LevelUp();
        FindObjectOfType<SkillSelectUI>().Hide();
    }
}
