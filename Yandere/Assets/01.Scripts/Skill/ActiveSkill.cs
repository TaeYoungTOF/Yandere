using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewActiveSkill", menuName = "Skills/ActiveSkill")]
public class ActiveSkill : BaseSkill
{
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
        }
    }
}
