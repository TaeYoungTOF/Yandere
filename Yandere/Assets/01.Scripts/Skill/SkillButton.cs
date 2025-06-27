using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
        Debug.Log($"[SkillButton] 클릭된 스킬: {skill?.skillName ?? "null"}");

        SkillManager manager = FindObjectOfType<SkillManager>();
        if (skill == null)
        {
            Debug.LogError("SkillButton: skill이 null입니다!");
            return;
        }

        if (!manager.equippedSkills.Contains(skill))
            manager.EquipSkill(skill);
        else
            skill.LevelUp();

        FindObjectOfType<SkillSelectUI>().Hide();
    }
}
