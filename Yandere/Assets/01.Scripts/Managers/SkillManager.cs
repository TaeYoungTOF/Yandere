using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public List<BaseSkill> equippedSkills = new List<BaseSkill>(); //현재 장착중인 스킬 목록 
    private Dictionary<BaseSkill, float> cooldownTimers = new Dictionary<BaseSkill, float>();  //각 액티브 스클의 현재 쿨다운 시간을 추적하는 딕셔너리(스클별로 남은 쿨다운 시간이 얼마인지 저장)

    void Start()
    {
        foreach (var skill in equippedSkills) //장착된 모든 스킬 반복
        {
            if (skill.skillType == SkillType.Passive) //만약 패시브스킬이면 onequip 실행
                skill.OnEquip(transform);

            if (skill.skillType == SkillType.Active) //만약 액티브 스킬이면, 딕셔너리에 해당 스클을 추가하고 처음 쿨다운은 0으로세팅
                cooldownTimers.Add(skill, 0f);
        }
    }

    void Update()
    {
        foreach (var skill in equippedSkills) //장착된 모든 스킬반복
        {
            if (skill.skillType == SkillType.Active) //액티브 스킬인 경우만 발동 체크 진행
            {
                cooldownTimers[skill] -= Time.deltaTime; //남은 쿨다운 시간 감소 (매 프레임마다 시간 흐름 반영)

                ActiveSkill activeSkill = skill as ActiveSkill; //BaseSkill을 ActiveSkill로 캐스팅
                if (cooldownTimers[skill] <= 0f) //쿨다운이 0보다 작거나같으면
                {
                    skill.Activate(transform);  //스킬 발동
                    cooldownTimers[skill] = activeSkill.cooldown;  // 다시 쿨다운세팅(스킬 방동 후 다음 쿨다운 시작)
                }
            }
        }
    }

    public void EquipSkill(BaseSkill skill)
    {
        equippedSkills.Add(skill);
        if (skill.skillType == SkillType.Passive)
            skill.OnEquip(transform);
    }

    public void UnequipSkill(BaseSkill skill)
    {
        equippedSkills.Remove(skill);
        if (skill.skillType == SkillType.Passive)
            skill.OnUnequip(transform);
    }

}
