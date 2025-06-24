using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewActiveSkill", menuName = "Skills/ActiveSkill")]
public class ActiveSkill : BaseSkill
{
    [Header("Active Skill Stats")]
    public float damage;
    public float cooldown;
    public float range;
    public int pierceCount;
    public GameObject projectilePrefab;

    public override void Activate(Transform caster)
    {
        if (projectilePrefab != null)
        {
            GameObject proj = Object.Instantiate(projectilePrefab, caster.position, caster.rotation);
            // projectile에 필요한 속도, 데미지 설정 전달 가능
        }
    }
}
