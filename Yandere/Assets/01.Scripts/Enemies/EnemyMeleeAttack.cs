using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour, IEnemyAttack
{
    
    [SerializeField] private EnemySkill_Dash dashSkill;
    [SerializeField] private EnemySkill_Boom boomSkill;
    
    public void Attack(float damage)
    {
        StageManager.Instance.Player.TakeDamage(damage);
        
        
        //dashSkill?.TryDash();                                               // 대쉬 스킬이 할당 되어 있다면 실행
        boomSkill?.TryBoom();
        //Debug.Log($"[Melee] 플레이어에게 {damage} 근접 피해");
    }
}
