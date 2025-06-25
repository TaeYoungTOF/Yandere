using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPassiveSkill", menuName = "Skills/PassiveSkill")]
public class PassiveSkill : BaseSkill
{
    [Header("Passive Skill Stats")]
    public float bonusHealth;
    public float bonusMoveSpeed;

    public override void OnEquip(Transform caster)
    {
        /*
        Player player = caster.GetComponent<Player>();

        if (player != null)
        {
            player.maxHealth += bonusHealth;
            player.moveSpeed += bonusMoveSpeed;
        }
        */
    }

    public override void OnUnequip(Transform caster)
    {
        /*
        Player player = caster.GetComponent<Player>();

        if (player != null)
        {
            player.maxHealth -= bonusHealth;
            player.moveSpeed -= bonusMoveSpeed;
        }
        */
    }
}
