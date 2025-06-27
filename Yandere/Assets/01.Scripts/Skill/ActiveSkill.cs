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
    public float projectileSpeed;

    public GameObject projectilePrefab;
    public LayerMask enemyLayer;

    public override void Activate(Transform caster)
    {
        if (projectilePrefab == null || caster == null)
            return;

        GameObject proj = Instantiate(projectilePrefab, caster.position + caster.up * 1f, caster.rotation);
        var projectile = proj.GetComponent<Projectile>();
        if (projectile != null)
        {
            projectile.Initialize(
                damage: damage + (level - 1) * levelDamageBonus,
                speed: projectileSpeed,
                range: range,
                pierceCount: pierceCount,
                enemyLayer: enemyLayer
            );
        }
    }
}
