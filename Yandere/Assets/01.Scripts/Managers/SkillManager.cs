using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public List<BaseSkill> equippedSkills = new List<BaseSkill>(); //���� �������� ��ų ��� 
    private Dictionary<BaseSkill, float> cooldownTimers = new Dictionary<BaseSkill, float>();  //�� ��Ƽ�� ��Ŭ�� ���� ��ٿ� �ð��� �����ϴ� ��ųʸ�(��Ŭ���� ���� ��ٿ� �ð��� ������ ����)

    void Start()
    {
        foreach (var skill in equippedSkills) //������ ��� ��ų �ݺ�
        {
            if (skill.skillType == SkillType.Passive) //���� �нú꽺ų�̸� onequip ����
                skill.OnEquip(transform);

            if (skill.skillType == SkillType.Active) //���� ��Ƽ�� ��ų�̸�, ��ųʸ��� �ش� ��Ŭ�� �߰��ϰ� ó�� ��ٿ��� 0���μ���
                cooldownTimers.Add(skill, 0f);
        }
    }

    void Update()
    {
        foreach (var skill in equippedSkills) //������ ��� ��ų�ݺ�
        {
            if (skill.skillType == SkillType.Active) //��Ƽ�� ��ų�� ��츸 �ߵ� üũ ����
            {
                cooldownTimers[skill] -= Time.deltaTime; //���� ��ٿ� �ð� ���� (�� �����Ӹ��� �ð� �帧 �ݿ�)

                ActiveSkill activeSkill = skill as ActiveSkill; //BaseSkill�� ActiveSkill�� ĳ����
                if (cooldownTimers[skill] <= 0f) //��ٿ��� 0���� �۰ų�������
                {
                    skill.Activate(transform);  //��ų �ߵ�
                    cooldownTimers[skill] = activeSkill.cooldown;  // �ٽ� ��ٿ��(��ų �浿 �� ���� ��ٿ� ����)
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
