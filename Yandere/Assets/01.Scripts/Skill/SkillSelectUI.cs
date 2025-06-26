using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSelectUI : MonoBehaviour
{
    public GameObject panel;
    public List<SkillButton> skillButtons;

    void Start()
    {
        panel.SetActive(false);
    }

    public void Show(List<BaseSkill> options)
    {
        panel.SetActive(true);
        for (int i = 0; i < skillButtons.Count; i++)
        {
            if (i < options.Count)
                skillButtons[i].Setup(options[i]);
            else
                skillButtons[i].gameObject.SetActive(false);
        }
        Time.timeScale = 0f;
    }

    public void Hide()
    {
        panel.SetActive(false);
        Time.timeScale = 1f;
    }
}
